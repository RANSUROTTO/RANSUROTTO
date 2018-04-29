using System;
using System.ComponentModel.DataAnnotations;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Common
{
    public class MaintenanceModel : BaseModel
    {
        public MaintenanceModel()
        {
            DeleteGuests = new DeleteGuestsModel();
            DeleteExportedFiles = new DeleteExportedFilesModel();
        }

        public DeleteGuestsModel DeleteGuests { get; set; }

        public DeleteExportedFilesModel DeleteExportedFiles { get; set; }

        #region Nested classes

        public class DeleteGuestsModel : BaseModel
        {

            /// <summary>
            /// 删除游客按创建时间来限制的开始时间
            /// </summary>
            [UIHint("DateNullable")]
            [ResourceDisplayName("Admin.System.Maintenance.DeleteGuests.StartDate")]
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// 删除游客按创建时间来限制的结束时间
            /// </summary>
            [UIHint("DateNullable")]
            [ResourceDisplayName("Admin.System.Maintenance.DeleteGuests.EndDate")]
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// 是否只删除没有动作的游客
            /// </summary>
            [ResourceDisplayName("Admin.System.Maintenance.DeleteGuests.OnlyWithoutAction")]
            public bool OnlyWithoutAction { get; set; }

            /// <summary>
            /// 成功删除的游客数量
            /// </summary>
            public int? NumberOfDeletedCustomers { get; set; }

        }

        public class DeleteExportedFilesModel : BaseModel
        {

            /// <summary>
            /// 删除导出文件按导出时间来限制的开始时间
            /// </summary>
            [ResourceDisplayName("Admin.System.Maintenance.DeleteExportedFiles.StartDate")]
            [UIHint("DateNullable")]
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// 删除导出文件按导出时间来限制的结束时间
            /// </summary>
            [ResourceDisplayName("Admin.System.Maintenance.DeleteExportedFiles.EndDate")]
            [UIHint("DateNullable")]
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// 成功删除的导出文件数量
            /// </summary>
            public int? NumberOfDeletedFiles { get; set; }

        }

        #endregion

    }
}