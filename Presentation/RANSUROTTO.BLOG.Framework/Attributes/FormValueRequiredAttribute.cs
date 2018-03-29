using System;
using System.Linq;
using System.Web.Mvc;
using System.Reflection;
using System.Diagnostics;

namespace RANSUROTTO.BLOG.Framework.Attributes
{
    /// <summary>
    /// 该特性筛选进入指定的动作方法必须存在某个表单项(或及其值)
    /// </summary>
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] _submitButtonNames;
        private readonly FormValueRequirement _requirement;
        private readonly bool _validateNameOnly;

        public FormValueRequiredAttribute(params string[] submitButtonNames) :
            this(FormValueRequirement.Equal, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames) :
            this(requirement, true, submitButtonNames)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="requirement">匹配类型</param>
        /// <param name="validateNameOnly">只验证表单项名称</param>
        /// <param name="submitButtonNames">提交的按钮名称</param>
        public FormValueRequiredAttribute(FormValueRequirement requirement, bool validateNameOnly, params string[] submitButtonNames)
        {
            this._submitButtonNames = submitButtonNames;
            this._validateNameOnly = validateNameOnly;
            this._requirement = requirement;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (var buttonName in _submitButtonNames)
            {
                try
                {
                    switch (_requirement)
                    {
                        case FormValueRequirement.Equal:
                            {
                                if (_validateNameOnly)
                                {
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(p =>
                                        p.Equals(buttonName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    var value = controllerContext.HttpContext.Request.Form[buttonName];
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                        case FormValueRequirement.StartsWith:
                            {
                                if (_validateNameOnly)
                                {
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(p =>
                                        p.StartsWith(buttonName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    foreach (var key in controllerContext.HttpContext.Request.Form.AllKeys
                                        .Where(p => p.StartsWith(buttonName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        var value = controllerContext.HttpContext.Request.Form[key];
                                        if (!string.IsNullOrEmpty(value))
                                            return true;
                                    }
                                }
                            }
                            break;
                        case FormValueRequirement.EndWith:
                            {
                                if (_validateNameOnly)
                                {
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(p =>
                                        p.EndsWith(buttonName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    foreach (var key in controllerContext.HttpContext.Request.Form.AllKeys
                                        .Where(p => p.EndsWith(buttonName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        var value = controllerContext.HttpContext.Request.Form[key];
                                        if (!string.IsNullOrEmpty(value))
                                            return true;
                                    }
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return false;
        }

    }

    /// <summary>
    /// 表单值匹配类型
    /// </summary>
    public enum FormValueRequirement
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal,

        /// <summary>
        /// 以此开始
        /// </summary>
        StartsWith,

        /// <summary>
        /// 以此结束
        /// </summary>
        EndWith

    }

}
