using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class MonFeeFilter
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgID { get; set; }
        /// <summary>
        /// 定点机构月费用ID
        /// </summary>
        public int OrgMonFeeId { get; set; }
        /// <summary>
        /// 申报费用类型(护理险、自费）
        /// </summary>
        public string FeeType { get; set; }
    }

    public class DeductionFilter
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string NsNo { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
