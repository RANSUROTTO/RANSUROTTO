using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Data;

namespace RANSUROTTO.BLOG.Services.Common
{
    public static class GenericAttributeExtensions
    {

        /// <summary>
        /// 获取通用属性值
        /// </summary>
        /// <typeparam name="TPropType">值类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <returns>通用属性值</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            return GetAttribute<TPropType>(entity, key, genericAttributeService);
        }

        /// <summary>
        /// 获取通用属性值
        /// </summary>
        /// <typeparam name="TPropType">值类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <param name="genericAttributeService">通用属性业务实例</param>
        /// <returns>通用属性值</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity,
            string key, IGenericAttributeService genericAttributeService)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = genericAttributeService.GetAttributesForEntity(entity.Id, keyGroup);

            if (props == null)
                return default(TPropType);

            props = props.ToList();

            if (!props.Any())
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);
        }

    }
}
