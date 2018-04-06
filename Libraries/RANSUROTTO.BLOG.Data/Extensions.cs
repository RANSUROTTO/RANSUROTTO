using System;
using System.Data.Entity.Core.Objects;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Data
{
    public static class Extensions
    {

        /// <summary>
        /// 获取实体unproxied类型
        /// </summary>
        /// <remarks>
        /// 如果您的实体框架上下文是代理启用的，
        /// 运行时将创建实体的代理实例，
        /// 即一个动态生成的类，它从实体类继承并通过插入特定的代码来重写其虚拟属性，例如用于跟踪更改和延迟加载。
        /// </remarks>
        public static Type GetUnproxiedEntityType(this BaseEntity entity)
        {
            var userType = ObjectContext.GetObjectType(entity.GetType());
            return userType;
        }

    }
}
