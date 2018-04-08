using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RANSUROTTO.BLOG.Core.Infrastructure.Mapper;

namespace RANSUROTTO.BLOG.Admin.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

    }
}