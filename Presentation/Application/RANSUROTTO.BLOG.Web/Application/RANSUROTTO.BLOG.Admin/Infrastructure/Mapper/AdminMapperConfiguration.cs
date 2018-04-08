using System;
using AutoMapper;
using RANSUROTTO.BLOG.Core.Infrastructure.Mapper;

namespace RANSUROTTO.BLOG.Admin.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : IMapperConfiguration
    {
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = cfg =>
            {



            };
            return action;
        }

        public int Order => 0;

    }
}