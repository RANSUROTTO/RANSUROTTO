using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Media;
using RANSUROTTO.BLOG.Core.Domain.Media.Enum;

namespace RANSUROTTO.BLOG.Services.Media
{
    public interface IPictureService
    {

        /// <summary>
        /// 通过标识符获取图片
        /// </summary>
        /// <param name="pictureId">图片标识符</param>
        /// <returns>图片</returns>
        Picture GetPictureById(int pictureId);

        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>图片列表</returns>
        IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取图片文件加载其二进制值
        /// </summary>
        /// <param name="picture">图片</param>
        /// <returns>图片的二进制值</returns>
        byte[] LoadPictureBinary(Picture picture);

        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize">目标图片尺寸(长度)</param>
        /// <param name="defaultPictureType">图片类型</param>
        /// <returns>图片URL</returns>
        string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="pictureId">图片标识符</param>
        /// <param name="targetSize">目标图片尺寸(长度)</param>
        /// <param name="showDefaultPicture">指示未找到图片时是否返回默认图片的值</param>
        /// <param name="defaultPictureType">图片类型</param>
        /// <returns>图片URL</returns>
        string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="picture">图片</param>
        /// <param name="targetSize">目标图片尺寸(长度)</param>
        /// <param name="showDefaultPicture">指示未找到图片时是否返回默认图片的值</param>
        /// <param name="defaultPictureType">图片类型</param>
        /// <returns>图片URL</returns>
        string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="pictureBinary">图片二进制值</param>
        /// <param name="mimeType">图片Mime类型</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">alt属性</param>
        /// <param name="titleAttribute">title属性</param>
        /// <param name="validateBinary">指示是否验证提供的图片二进制值</param>
        /// <returns>图片</returns>
        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null, bool validateBinary = true);

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="pictureId">图片标识符</param>
        /// <param name="pictureBinary">图片二进制值</param>
        /// <param name="mimeType">图片Mime类型</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">alt属性</param>
        /// <param name="titleAttribute">title属性</param>
        /// <param name="validateBinary">指示是否验证提供的图片二进制值</param>
        /// <returns>图片</returns>
        Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool validateBinary = true);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture">图片</param>
        void DeletePicture(Picture picture);

        /// <summary>
        /// 验证图片的尺寸
        /// </summary>
        /// <param name="pictureBinary">图片二进制值</param>
        /// <param name="mimeType">图片Mime类型</param>
        /// <returns>图片二进制值或抛出异常</returns>
        byte[] ValidatePicture(byte[] pictureBinary, string mimeType);

        /// <summary>
        /// 获取图片本地路径
        /// </summary>
        /// <param name="picture">图片</param>
        /// <param name="targetSize">目标图片尺寸(长度)</param>
        /// <param name="showDefaultPicture">指示未找到图片时是否返回默认图片的值</param>
        /// <returns></returns>
        string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

        /// <summary>
        /// 获取图片哈希
        /// </summary>
        /// <param name="picturesIds">图片标识符列表</param>
        /// <returns>图片哈希列表</returns>
        IDictionary<int, string> GetPicturesHash(int[] picturesIds);

    }
}
