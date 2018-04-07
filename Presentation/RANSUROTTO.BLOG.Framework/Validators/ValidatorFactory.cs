using System;
using FluentValidation;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Validators
{
    public class ValidatorFactory : AttributedValidatorFactory
    {
        public override IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
                if (attribute != null && attribute.ValidatorType != null)
                {
                    var instance = EngineContext.Current.ContainerManager.ResolveUnregistered(attribute.ValidatorType);
                    return instance as IValidator;
                }
            }
            return null;
        }
    }
}
