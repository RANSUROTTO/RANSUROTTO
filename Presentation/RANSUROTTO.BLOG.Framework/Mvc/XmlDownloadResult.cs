using System.IO;
using System.Xml;
using System.Text;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Common;

namespace RANSUROTTO.BLOG.Framework.Mvc
{
    /// <summary>
    /// 返回下载xml文件响应结果
    /// </summary>
    public class XmlDownloadResult : ActionResult
    {

        #region Fields

        private readonly string _fileDownloadName;
        private readonly string _xml;

        #endregion

        #region Constructor

        public XmlDownloadResult(string xml, string fileDownloadName)
        {
            _fileDownloadName = fileDownloadName;
            _xml = xml;
        }

        #endregion

        #region Methods

        public override void ExecuteResult(ControllerContext context)
        {
            var document = new XmlDocument();
            document.LoadXml(_xml);
            if (document.FirstChild is XmlDeclaration decl)
            {
                decl.Encoding = "UTF-8";
            }
            context.HttpContext.Response.Charset = "UTF-8";
            context.HttpContext.Response.ContentType = MimeTypes.TextXml;
            context.HttpContext.Response.AddHeader("content-disposition", $"attachment; filename={_fileDownloadName}");
            /*Format Write*/
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.Indentation = 2;
                    xmlWriter.Formatting = Formatting.Indented;
                    document.WriteContentTo(xmlWriter);
                }
                context.HttpContext.Response.BinaryWrite(Encoding.UTF8.GetBytes(sw.ToString()));
            }
            /*Write*/
            //context.HttpContext.Response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));
            context.HttpContext.Response.End();
        }

        #endregion

    }
}
