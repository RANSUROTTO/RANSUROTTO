using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using RANSUROTTO.BLOG.Core.Common;

namespace RANSUROTTO.BLOG.Framework.Mvc
{
    /// <summary>
    /// 返回使用自定义JSON转换器产生的JSON相应结果
    /// </summary>
    public class ConverterJsonResult : JsonResult
    {

        #region Fields

        private readonly JsonConverter[] _converters;

        #endregion

        #region Constructor

        public ConverterJsonResult(params JsonConverter[] converters)
        {
            _converters = converters;
        }

        #endregion

        #region Methods

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.HttpContext == null)
                return;

            context.HttpContext.Response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : MimeTypes.ApplicationJson;
            if (ContentEncoding != null)
                context.HttpContext.Response.ContentEncoding = ContentEncoding;

            if (Data != null)
                context.HttpContext.Response.Write(JsonConvert.SerializeObject(Data, _converters));
        }

        #endregion

    }
}
