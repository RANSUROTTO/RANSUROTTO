using System;

namespace RANSUROTTO.BLOG.Framework.Mvc
{
    /// <summary>
    /// 该特性标识引用的属性不会被清空
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NoTrimAttribute : Attribute
    {
    }
}
