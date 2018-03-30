using System;
using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Framework.Attributes
{
    /// <summary>
    /// 该特性将确保验证 表单值是否与预设值 是否相等的结果赋值到指定动作方法参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ParameterBasedOnFormNameAndValueAttribute : FilterAttribute, IActionFilter
    {
        private readonly string _name;
        private readonly string _value;
        private readonly string _actionParameterName;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">表单项名称</param>
        /// <param name="value">比较值</param>
        /// <param name="actionParameterName">动作方法参数名</param>
        public ParameterBasedOnFormNameAndValueAttribute(string name, string value, string actionParameterName)
        {
            this._name = name;
            this._value = value;
            this._actionParameterName = actionParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var formValue = filterContext.RequestContext.HttpContext.Request.Form[_name];
            filterContext.ActionParameters[_actionParameterName] = !string.IsNullOrEmpty(formValue) &&
                                                                   formValue.Equals(_value, StringComparison.OrdinalIgnoreCase);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

    }
}
