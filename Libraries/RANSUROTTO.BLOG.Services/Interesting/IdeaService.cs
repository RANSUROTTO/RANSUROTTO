using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Interesting;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Interesting
{
    public class IdeaService : IIdeaService
    {

        #region Fields

        private readonly IRepository<Idea> _ideaRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public IdeaService(IRepository<Idea> ideaRepository, IEventPublisher eventPublisher)
        {
            _ideaRepository = ideaRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public IPagedList<Idea> GetAllIdeas(DateTime? createFromUtc = null, DateTime? createToUtc = null, int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _ideaRepository.Table;
            if (createFromUtc.HasValue)
                query = query.Where(i => createFromUtc.Value <= i.CreatedOnUtc);
            if (createToUtc.HasValue)
                query = query.Where(i => createToUtc.Value >= i.CreatedOnUtc);

            query = query.OrderByDescending(l => l.CreatedOnUtc);

            var ideas = new PagedList<Idea>(query, pageIndex, pageSize);
            return ideas;
        }

        public Idea GetIdeaById(int ideaId)
        {
            if (ideaId == 0)
                return null;

            return _ideaRepository.GetById(ideaId);
        }

        public void InsertIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Insert(idea);

            _eventPublisher.EntityInserted(idea);
        }

        public void UpdateIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Update(idea);

            _eventPublisher.EntityUpdated(idea);
        }

        public void DeleteIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));

            _ideaRepository.Delete(idea);

            _eventPublisher.EntityDeleted(idea);
        }

        #endregion

    }
}
