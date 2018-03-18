using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Service.Events;
using RANSUROTTO.BLOG.Service.Logging;

namespace RANSUROTTO.BLOG.Service.Localization
{
    public class LocalizationService : ILocalizationService
    {

        #region Constants

        /// <summary>
        /// 语言键缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言ID
        /// </remarks>
        private const string LOCALSTRAINGRESOURCES_ALL_KEY = "Ransurotto.lsr.all-{0}";

        /// <summary>
        /// 语言键-资源键-缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言ID
        /// {1} : 资源键ID
        /// </remarks>
        private const string LOCALSTRAINGRESOURCES_BY_RESOURCENAME_KEY = "Ransurotto.lsr.{0}-{1}";

        /// <summary>
        /// 语言资源缓存键清空匹配模式
        /// </summary>
        private const string LOCALSTRAINGRESOURCES_PATTERN_KEY = "Ransurotto.lsr.";

        #endregion

        #region Fields

        private readonly IRepository<LocaleStringResource> _lsrRepository;
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public LocalizationService(IRepository<LocaleStringResource> lsrRepository, IWorkContext workContext, ILanguageService languageService, ILogger logger, ICacheManager cacheManager, IDataProvider dataProvider, IDbContext dbContext, IEventPublisher eventPublisher)
        {
            _lsrRepository = lsrRepository;
            _workContext = workContext;
            _languageService = languageService;
            _logger = logger;
            _cacheManager = cacheManager;
            _dataProvider = dataProvider;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods



        #endregion

    }
}
