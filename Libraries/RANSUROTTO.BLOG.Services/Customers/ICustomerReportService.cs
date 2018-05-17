namespace RANSUROTTO.BLOG.Services.Customers
{
    public interface ICustomerReportService
    {

        /// <summary>
        /// 统计指定最后天数内的注册用户数
        /// </summary>
        /// <param name="days">最后天数</param>
        /// <returns>最后天数内的注册用户数</returns>
        int GetRegisteredCustomersReport(int days);

    }
}
