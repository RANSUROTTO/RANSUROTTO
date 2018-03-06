using System.Collections;

namespace RANSUROTTO.BLOG.Framework.Kendoui
{

    public class DataSourceResult
    {

        public object ExtraData { get; set; }

        public IEnumerable Data { get; set; }

        public object Errors { get; set; }

        public int Total { get; set; }

    }

}
