using FluentValidation;

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

    }
}
