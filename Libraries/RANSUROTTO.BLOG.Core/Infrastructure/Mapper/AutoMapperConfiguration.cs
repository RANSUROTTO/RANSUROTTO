using System;
using System.Collections.Generic;
using AutoMapper;

namespace RANSUROTTO.BLOG.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 自动映射 配置
    /// </summary>
    public static class AutoMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        /// <summary>
        /// 初始化映射
        /// </summary>
        /// <param name="configurationActions">配置动作</param>
        public static void Init(List<Action<IMapperConfigurationExpression>> configurationActions)
        {
            if (configurationActions == null)
                throw new ArgumentNullException(nameof(configurationActions));

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var ca in configurationActions)
                    ca(cfg);
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// 映射处理对象
        /// </summary>
        public static IMapper Mapper => _mapper;

        /// <summary>
        /// 映射配置
        /// </summary>
        public static MapperConfiguration MapperConfiguration => _mapperConfiguration;

    }
}
