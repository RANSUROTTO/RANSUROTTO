using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Interesting
{
    public class IdeaModel : BaseEntityModel
    {

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.Deleted")]
        public bool Deleted { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.Private")]
        public bool Private { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.CreatedOn")]
        public override DateTime CreatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.UpdatedOn")]
        public DateTime? UpdatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.CustomerId")]
        public int CustomerId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Idea.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }

    }
}