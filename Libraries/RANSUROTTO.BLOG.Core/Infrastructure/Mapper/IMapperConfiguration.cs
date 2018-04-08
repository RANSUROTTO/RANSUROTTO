using System;
using AutoMapper;

namespace RANSUROTTO.BLOG.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 映射配置注册接口
    /// </summary>
    public interface IMapperConfiguration
    {

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns>映射配置方法</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();

        /// <summary>
        /// 注册顺序
        /// </summary>
        int Order { get; }

    }
}
