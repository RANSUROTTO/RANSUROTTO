using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Kendoui;

namespace RANSUROTTO.BLOG.Framework.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> PagedForCommand<T>(this IEnumerable<T> current, DataSourceRequest command)
        {
            return current.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        }

    }
}
