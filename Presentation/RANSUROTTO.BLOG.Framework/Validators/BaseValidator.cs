using System.Linq;
using FluentValidation;
using System.Linq.Dynamic;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Framework.Validators
{
    public class BaseValidator<T> : AbstractValidator<T> where T : class
    {

        protected BaseValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// 可以在继承类中实现该方法
        /// 提供在构造对象时使用的自定义代码
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        protected virtual void SetDatabaseValidationRules<TObject>(IDbContext dbContext, params string[] filterStringPropertyNames)
        {
            SetStringPropertiesMaxLength<TObject>(dbContext, filterStringPropertyNames);
        }

        /// <summary>
        /// 设置字符串属性长度限制
        /// </summary>
        protected virtual void SetStringPropertiesMaxLength<TObject>(IDbContext dbContext,
            params string[] filterPropertyNames)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !filterPropertyNames.Contains(p.Name))
                .Select(p => p.Name).ToArray();

            var maxLength = dbContext.GetColumnsMaxLength(dbObjectType.Name, names);
            var expression = maxLength.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, string>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Length(0, maxLength[expr.Key]);
            }
        }

    }
}
