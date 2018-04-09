using System.Collections.Generic;
using System.Web.Routing;

namespace RANSUROTTO.BLOG.Framework.Menu
{
    /// <summary>
    /// 站点菜单节点
    /// </summary>
    public class SiteMapNode
    {
        public SiteMapNode()
        {
            RouteValues = new RouteValueDictionary();
            ChildNodes = new List<SiteMapNode>();
        }

        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 获取或设置动作方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 获取或设置路由参数值
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }

        /// <summary>
        /// 获取或设置Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置子节点
        /// </summary>
        public IList<SiteMapNode> ChildNodes { get; set; }

        /// <summary>
        /// 获取或设置图标class(Font Awesome: http://fontawesome.io/)
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// 获取或设置是否可视
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 获取或设置指示是否在新的窗口打开链接（选项卡
        /// </summary>
        public bool OpenUrlInNewTab { get; set; }

    }
}
