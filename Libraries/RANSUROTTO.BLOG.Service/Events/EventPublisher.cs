using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSUROTTO.BLOG.Service.Events
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
