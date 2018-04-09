using System.Linq;
using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Framework.Controllers
{
    /// <summary>
    /// 该特性将确保验证 表单是否存在指定名称项 的结果赋值到指定动作方法参数
    /// </summary>
    public class ParameterBasedOnFormNameAttribute : FilterAttribute, IActionFilter
    {
        private readonly string _name;
        private readonly string _actionParameterName;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">表单项名称</param>
        /// <param name="actionParameterName">动作方法参数名称</param>
        public ParameterBasedOnFormNameAttribute(string name, string actionParameterName)
        {
            this._name = name;
            this._actionParameterName = actionParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.ActionParameters[_actionParameterName] = filterContext.RequestContext
                .HttpContext.Request.Form.AllKeys.Any(x => x.Equals(_name));
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

    }
}
