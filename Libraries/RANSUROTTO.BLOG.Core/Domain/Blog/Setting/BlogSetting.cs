namespace RANSUROTTO.BLOG.Core.Domain.Blog.Setting
{

    public class BlogSetting
    {

        /// <summary>
        /// 获取或设置是否允许未注册用户(游客)进行评论
        /// </summary>
        public bool AllowNotRegisteredUserToLeaveComments { get; set; }

        /// <summary>
        /// 获取或设置博文可有最多标签个数
        /// </summary>
        public int MaxNumberOfTags { get; set; }

    }

}
