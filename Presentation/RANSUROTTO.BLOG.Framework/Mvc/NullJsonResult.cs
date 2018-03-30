using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using RANSUROTTO.BLOG.Core.Common;

namespace RANSUROTTO.BLOG.Framework.Mvc
{
    /// <summary>
    /// 返回空JSON相应结果
    /// </summary>
    public class NullJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : MimeTypes.ApplicationJson;
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            this.Data = null;

            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);
            response.Write(serializedObject);
        }
    }
}
