using System;
using System.IO;
using System.Linq;
using ImageResizer;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Services.Events;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Core.Domain.Media;
using RANSUROTTO.BLOG.Core.Domain.Media.Enum;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Media.Setting;

namespace RANSUROTTO.BLOG.Services.Media
{
    public class PictureService : IPictureService
    {

        #region Constants

        private const int MULTIPLE_THUMB_DIRECTORIES_LENGTH = 3;

        #endregion

        #region Fields

        private readonly IRepository<Picture> _pictureRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly MediaSettings _mediaSettings;
        private readonly ISettingService _settingService;

        #endregion

        #region Constructor

        public PictureService(IRepository<Picture> pictureRepository, IEventPublisher eventPublisher, IWebHelper webHelper, ILogger logger)
        {
            _pictureRepository = pictureRepository;
            _eventPublisher = eventPublisher;
            _webHelper = webHelper;
            _logger = logger;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }

        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var result = LoadPictureFromFile(picture.Id, picture.MimeType);
            return result;
        }

        public virtual string GetDefaultPictureUrl(int targetSize = 0, PictureType defaultPictureType = PictureType.Entity)
        {
            string defaultImageFileName;
            switch (defaultPictureType)
            {
                /*从设定中获取默认头像和实体默认图片名*/
                case PictureType.Avatar:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.Customer.DefaultAvatarImageName", "default-avatar.jpg");
                    break;
                case PictureType.Entity:
                default:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", "default-image.png");
                    break;
            }
            var filePath = GetPictureLocalPath(defaultImageFileName);
            if (!File.Exists(filePath))
                return "";

            if (targetSize == 0)
            {
                var url = _webHelper.GetLocation() + "Content/Images/" + defaultImageFileName;
                return url;
            }
            else
            {
                string fileExtension = Path.GetExtension(filePath);
                string thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    targetSize,
                    fileExtension);//生成目标尺寸图片的存储路径

                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        using (var destStream = new MemoryStream())
                        {
                            var newSize = CalculateDimensions(b.Size, targetSize);
                            ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                            {
                                Width = newSize.Width,
                                Height = newSize.Height,
                                Scale = ScaleMode.Both,
                                Quality = _mediaSettings.DefaultImageQuality
                            });
                            var destBinary = destStream.ToArray();
                            SaveThumb(thumbFilePath, thumbFileName, "", destBinary);
                        }
                    }
                }

                var url = GetThumbUrl(thumbFileName);
                return url;
            }
        }

        public virtual string GetPictureUrl(int pictureId, int targetSize = 0, bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture, defaultPictureType);
        }

        public virtual string GetPictureUrl(Picture picture, int targetSize = 0, bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;

            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);

            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    //返回默认图片URL
                    url = GetDefaultPictureUrl(targetSize, defaultPictureType);
                }
                return url;
            }

            var seoFileName = picture.SeoFilename;

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;

            if (targetSize == 0)
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}.{lastPart}"
                    : $"{picture.Id:0000000}.{lastPart}";
            }
            else
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}_{targetSize}.{lastPart}"
                    : $"{picture.Id:0000000}_{targetSize}.{lastPart}";
            }
            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;

                        if (targetSize != 0)
                        {
                            using (var stream = new MemoryStream(pictureBinary))
                            {
                                Bitmap b = null;
                                try
                                {
                                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                                    b = new Bitmap(stream);
                                }
                                catch (ArgumentException exc)
                                {
                                    _logger.Error(string.Format("Error generating picture thumb. ID={0}", picture.Id),
                                        exc);
                                }

                                if (b == null)
                                {
                                    //bitmap could not be loaded for some reasons
                                    return url;
                                }

                                using (var destStream = new MemoryStream())
                                {
                                    var newSize = CalculateDimensions(b.Size, targetSize);
                                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                                    {
                                        Width = newSize.Width,
                                        Height = newSize.Height,
                                        Scale = ScaleMode.Both,
                                        Quality = _mediaSettings.DefaultImageQuality
                                    });
                                    pictureBinaryResized = destStream.ToArray();
                                    b.Dispose();
                                }
                            }
                        }
                        else
                        {
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }
            }
            url = GetThumbUrl(thumbFileName);
            return url;
        }

        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = new Picture
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute
            };
            _pictureRepository.Insert(picture);

            SavePictureInFile(picture.Id, pictureBinary, mimeType);

            _eventPublisher.EntityInserted(picture);

            return picture;
        }

        public Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;

            _pictureRepository.Update(picture);

            SavePictureInFile(picture.Id, pictureBinary, mimeType);

            _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            DeletePictureThumbs(picture);
            DeletePictureOnFileSystem(picture);

            _pictureRepository.Delete(picture);

            _eventPublisher.EntityDeleted(picture);
        }

        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            using (var destStream = new MemoryStream())
            {
                ImageBuilder.Current.Build(pictureBinary, destStream, new ResizeSettings
                {
                    MaxWidth = _mediaSettings.MaximumImageSize,
                    MaxHeight = _mediaSettings.MaximumImageSize,
                    Quality = _mediaSettings.DefaultImageQuality
                });
                return destStream.ToArray();
            }
        }

        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (String.IsNullOrEmpty(url))
                return String.Empty;

            return GetThumbLocalPath(Path.GetFileName(url));
        }

        public virtual IDictionary<int, string> GetPicturesHash(int[] picturesIds)
        {
            //TODO 没有获取图片的哈希实现
            throw new NotImplementedException();
        }

        #endregion

        #region Utilities

        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = $"{pictureId:0000000}_0.{lastPart}"; //:0000000拼成7位数,不足补0
            File.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }

        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = $"{pictureId:0000000}_0.{lastPart}"; //:0000000拼成7位数,不足补0
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];
            return File.ReadAllBytes(filePath);
        }

        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
                case "gif":
                    lastPart = "gif";
                    break;
            }
            return lastPart;
        }

        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapPath("~/Content/Images/"), fileName);
        }

        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapPath("~/Content/Images/Thumbs");
            if (_mediaSettings.MultipleThumbDirectories)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null &&
                    fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
                {
                    //获取图片前三个字符创建文件夹
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, MULTIPLE_THUMB_DIRECTORIES_LENGTH);
                    thumbsDirectoryPath = Path.Combine(thumbsDirectoryPath, subDirectoryName);
                    if (!System.IO.Directory.Exists(thumbsDirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
                    }
                }
            }
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return File.Exists(thumbFilePath);
        }

        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;
            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered - http://www.nopcommerce.com/boards/t/40616/image-resizing-bug.aspx
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            File.WriteAllBytes(thumbFilePath, binary);
        }

        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            var url = _webHelper.GetLocation() + "Content/Images/Thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, MULTIPLE_THUMB_DIRECTORIES_LENGTH);
                    url = url + subDirectoryName + "/";
                }
            }

            url = url + thumbFileName;
            return url;
        }

        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = $"{picture.Id:0000000}*.*";
            var thumbDirectoryPath = CommonHelper.MapPath("~/Content/Images/Thumbs");
            string[] currentFiles = System.IO.Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (string currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }

        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string fileName = $"{picture.Id:0000000}_0.{lastPart}";
            string filePath = GetPictureLocalPath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion

    }
}
