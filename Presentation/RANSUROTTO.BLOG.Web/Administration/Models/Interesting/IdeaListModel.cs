using System;
using System.ComponentModel.DataAnnotations;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Interesting
{
    public class IdeaListModel
    {

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.ContentManagement.Idea.List.Fields.CreatedOnFrom")]
        public DateTime? CreatedOnFrom { get; set; }

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.ContentManagement.Idea.List.Fields.CreatedOnTo")]
        public DateTime? CreatedOnTo { get; set; }

    }
}