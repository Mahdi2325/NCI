using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResourceLinkModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 收費編號
        /// </summary>
        public long? FeeNo { get; set; }
        /// <summary>
        /// 病歷號
        /// </summary>
        public int? FegNo { get; set; }
        /// <summary>
        /// 負責人員
        /// </summary>
        public string RecordBy { get; set; }

        public string RecordByName { get; set; }
        /// <summary>
        /// 首次連絡日期
        /// </summary>
        public DateTime? ContactDate { get; set; }
        /// <summary>
        /// 連結完成日期
        /// </summary>
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// 需求類型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 需求名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 需求評估結果
        /// </summary>
        public string EvalResult { get; set; }
        /// <summary>
        /// 提供單位名稱
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 連結情形
        /// </summary>
        public string ResourceStatus { get; set; }
        /// <summary>
        /// 未連結原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 資源連結時機
        /// </summary>
        public string RegState { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 記錄創建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 記錄創建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 機構ID
        /// </summary>
        public string OrgId { get; set; }
    }
}
