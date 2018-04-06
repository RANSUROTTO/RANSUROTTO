using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Common;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data;
using RANSUROTTO.BLOG.Service.Events;

namespace RANSUROTTO.BLOG.Service.Common
{
    public class GenericAttributeService : IGenericAttributeService
    {

        #region Constants

        /// <summary>
        /// 通用属性键缓存
        /// </summary>
        /// <remarks>
        /// {0} : 实体Id
        /// {1} : 键分组
        /// </remarks>
        private const string GENERICATTRIBUTE_KEY = "Ransurotto.genericattribute.{0}-{1}";

        /// <summary>
        /// 通用属性缓存键清空匹配模式
        /// </summary>
        private const string GENERICATTRIBUTE_PATTERN_KEY = "Nop.genericattribute.";

        #endregion

        #region Fields

        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public GenericAttributeService(IRepository<GenericAttribute> genericAttributeRepository, ICacheManager cacheManager, IEventPublisher eventPublisher)
        {
            _genericAttributeRepository = genericAttributeRepository;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通过标识符获取通用属性
        /// </summary>
        /// <param name="attributeId">通用属性标识符</param>
        /// <returns>通用属性</returns>
        public virtual GenericAttribute GetAttributeById(long attributeId)
        {
            if (attributeId == 0)
                return null;

            return _genericAttributeRepository.GetById(attributeId);
        }

        /// <summary>
        /// 通过实体标识符和键分组获取通用属性集合
        /// </summary>
        /// <param name="entityId">实体标识符</param>
        /// <param name="keyGroup">键分组</param>
        /// <returns>通用属性列表</returns>
        public virtual IList<GenericAttribute> GetAttributesForEntity(long entityId, string keyGroup)
        {
            string key = string.Format(GENERICATTRIBUTE_KEY, entityId, keyGroup);
            return _cacheManager.Get(key, () =>
            {
                var query = from ga in _genericAttributeRepository.Table
                            where ga.EntityId == entityId &&
                                  ga.KeyGroup == keyGroup
                            select ga;
                var attributes = query.ToList();
                return attributes;
            });
        }

        /// <summary>
        /// 添加通用属性
        /// </summary>
        /// <typeparam name="TPropType">值类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public virtual void SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = GetAttributesForEntity(entity.Id, keyGroup).ToList();

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    DeleteAttribute(prop);
                }
                else
                {
                    prop.Value = valueStr;
                    UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    prop = new GenericAttribute
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr
                    };
                    InsertAttribute(prop);
                }
            }
        }

        /// <summary>
        /// 添加通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        public virtual void InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            _genericAttributeRepository.Insert(attribute);

            //缓存
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //发布事件
            _eventPublisher.EntityInserted(attribute);
        }

        /// <summary>
        /// 更新通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        public virtual void UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            _genericAttributeRepository.Update(attribute);

            //缓存
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //发布事件
            _eventPublisher.EntityUpdated(attribute);
        }

        /// <summary>
        /// 删除通用属性
        /// </summary>
        /// <param name="attribute">通用属性</param>
        public virtual void DeleteAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            _genericAttributeRepository.Delete(attribute);

            //缓存
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //发布事件
            _eventPublisher.EntityDeleted(attribute);
        }

        /// <summary>
        /// 删除多个通用属性
        /// </summary>
        /// <param name="attributes">通用属性列表</param>
        public virtual void DeleteAttributes(IList<GenericAttribute> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            _genericAttributeRepository.Delete(attributes);

            //缓存
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //发布事件
            foreach (var attribute in attributes)
            {
                _eventPublisher.EntityDeleted(attribute);
            }
        }

        #endregion

    }
}
