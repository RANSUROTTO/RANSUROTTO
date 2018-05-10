using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Framework.Mvc
{
    /// <summary>
    /// 基础视图模型
    /// </summary>
    public partial class BaseModel
    {

        /// <summary>
        /// 此属性用于存储模型自定义值
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseModel()
        {
            CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        /// <summary>
        /// 模型在初始化时调用的方法
        /// 继承该类可以在这里写自定义代码在初始化时运行
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

    }

    /// <summary>
    /// 基础实体视图模型
    /// 参考实体类:RANSUROTTO.BLOG.Core.Data.BaseEntity
    /// </summary>
    public partial class BaseEntityModel : BaseModel
    {

        public virtual long Id { get; set; }

        public virtual Guid Guid { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual byte[] TimeStamp { get; set; }

    }

}
