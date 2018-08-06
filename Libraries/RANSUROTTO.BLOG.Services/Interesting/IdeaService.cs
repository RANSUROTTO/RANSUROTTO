using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Interesting;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Interesting
{
    public class IdeaService : IIdeaService
    {

        #region Fields

        private readonly IRepository<Idea> _ideaRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public IdeaService(IRepository<Idea> ideaRepository, IRepository<Customer> customerRepository, IEventPublisher eventPublisher)
        {
            _ideaRepository = ideaRepository;
            _customerRepository = customerRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Idea> GetAllIdeas(IList<int> customerIds, DateTime? createFromUtc = null,
            DateTime? createToUtc = null, int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            if (customerIds != null && customerIds.Contains(0))
                customerIds.Remove(0);

            var query = _ideaRepository.Table;
            if (createFromUtc.HasValue)
                query = query.Where(i => createFromUtc.Value <= i.CreatedOnUtc);
            if (createToUtc.HasValue)
                query = query.Where(i => createToUtc.Value >= i.CreatedOnUtc);

            if (customerIds != null && customerIds.Any())
            {
                query = from idea in query
                        join ct in _customerRepository.Table on idea.CustomerId equals ct.Id into idea_ct
                        from ct in idea_ct.DefaultIfEmpty()
                        where customerIds.Contains(ct.Id)
                        select idea;
            }

            query = query.OrderByDescending(l => l.CreatedOnUtc);

            var ideas = new PagedList<Idea>(query, pageIndex, pageSize);
            return ideas;
        }

        public virtual Idea GetIdeaById(int ideaId)
        {
            if (ideaId == 0)
                return null;

            return _ideaRepository.GetById(ideaId);
        }

        public virtual void InsertIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Insert(idea);

            _eventPublisher.EntityInserted(idea);
        }

        public virtual void UpdateIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Update(idea);

            _eventPublisher.EntityUpdated(idea);
        }

        public virtual void DeleteIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Delete(idea);

            _eventPublisher.EntityDeleted(idea);
        }

        #endregion

    }
}
