using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class ResidentMonFeeDtlModel
    {
        /// <summary>
        /// 参保人月费用明细ID
        /// </summary>
        public long RsMonFeeDtlId { get; set; }
        /// <summary>
        /// 参保人月费用汇总ID
        /// </summary>
        public Nullable<long> RsMonFeeId { get; set; }
        /// <summary>
        /// 费用名称
        /// </summary>
        public string FeeName { get; set; }
        /// <summary>
        /// 费用类型(药品/耗材/服务)
        /// </summary>
        public string FeeType { get; set; }
        /// <summary>
        /// 医保编码(对应药品等)
        /// </summary>
        public string MCCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 是否护理险项目
        /// </summary>
        public Nullable<bool> IsNCIItem { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public System.DateTime TakeTime { get; set; }
        /// <summary>
        /// 经办人姓名
        /// </summary>
        public string OperatorName { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
