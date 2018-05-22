using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Common;

namespace RANSUROTTO.BLOG.Services.Common
{
    /// <summary>
    /// 通用属性业务层接口
    /// </summary>
    public interface IGenericAttributeService
    {

        /// <summary>
        /// 通过标识符获取通用属性
        /// </summary>
        /// <param name="attributeId">通用属性标识符</param>
        /// <returns>通用属性</returns>
        GenericAttribute GetAttributeById(int attributeId);

        /// <summary>
        /// 通过实体标识符和键分组获取通用属性集合
        /// </summary>
        /// <param name="entityId">实体标识符</param>
        /// <param name="keyGroup">键分组</param>
        /// <returns>通用属性列表</returns>
        IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

        /// <summary>
        /// 添加通用属性
        /// </summary>
        /// <typeparam name="TPropType">值类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value);

        /// <summary>
        /// 添加通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        void InsertAttribute(GenericAttribute attribute);

        /// <summary>
        /// 更新通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        void UpdateAttribute(GenericAttribute attribute);

        /// <summary>
        /// 删除通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        void DeleteAttribute(GenericAttribute attribute);

        /// <summary>
        /// 删除多个通用属性
        /// </summary>
        /// <param name="attributes">通用属性列表</param>
        void DeleteAttributes(IList<GenericAttribute> attributes);

    }
}
