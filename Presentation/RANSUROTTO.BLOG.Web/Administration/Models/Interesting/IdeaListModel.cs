using System;
using System.ComponentModel.DataAnnotations;

namespace RANSUROTTO.BLOG.Admin.Models.Interesting
{
    public class IdeaListModel
    {

        [UIHint("DateTimeNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? CreatedOnTo { get; set; }

    }
}