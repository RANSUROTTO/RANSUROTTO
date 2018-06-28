using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Services.Tasks;

namespace RANSUROTTO.BLOG.Services.Customers
{
    /// <summary>
    /// 删除游客的计划任务实现
    /// </summary>
    public class DeleteGuestsTask : ITask
    {

        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;

        public DeleteGuestsTask(ICustomerService customerService, CustomerSettings customerSettings)
        {
            _customerService = customerService;
            _customerSettings = customerSettings;
        }

        public virtual void Execute()
        {
            //TODO 未实现删除游客计划任务
            //throw new System.NotImplementedException();
        }

    }
}
