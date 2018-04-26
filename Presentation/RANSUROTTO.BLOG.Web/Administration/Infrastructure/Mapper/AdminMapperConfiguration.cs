using System;
using AutoMapper;
using RANSUROTTO.BLOG.Admin.Models.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Infrastructure.Mapper;

namespace RANSUROTTO.BLOG.Admin.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : IMapperConfiguration
    {
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = cfg =>
            {

                #region Localization

                cfg.CreateMap<Language, LanguageModel>()
                    .ForMember(model => model.CustomProperties, entity => entity.Ignore());
                cfg.CreateMap<LanguageModel, Language>()
                    .ForMember(entity => entity.LocaleStringResources, model => model.Ignore());

                #endregion



            };
            return action;
        }

        public int Order => 0;

    }
}