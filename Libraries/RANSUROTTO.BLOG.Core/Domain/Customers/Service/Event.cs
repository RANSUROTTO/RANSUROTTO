namespace RANSUROTTO.BLOG.Core.Domain.Customers.Service
{

    /// <summary>
    /// 用户登录完成事件
    /// </summary>
    public class CustomerLoggedinEvent
    {
        public CustomerLoggedinEvent(Customer customer)
        {
            this.Customer = customer;
        }

        public Customer Customer { get; }

    }

    /// <summary>
    /// 用户退出登录完成事件
    /// </summary>
    public class CustomerLoggedOutEvent
    {
        public CustomerLoggedOutEvent(Customer customer)
        {
            this.Customer = customer;
        }

        public Customer Customer { get; }
    }

    /// <summary>
    /// 用户注册完成事件
    /// </summary>
    public class CustomerRegisteredEvent
    {
        public CustomerRegisteredEvent(Customer customer)
        {
            this.Customer = customer;
        }

        public Customer Customer { get; }
    }

    /// <summary>
    /// 用户修改密码完成事件
    /// </summary>
    public class CustomerPasswordChangedEvent
    {
        public CustomerPasswordChangedEvent(CustomerPassword password)
        {
            this.Password = password;
        }

        public CustomerPassword Password { get; }
    }

}
