using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    //资格申请表
    public class ServiceDepositGrant
    {
        //年拨付记录ID
        public int SdGrantid { get; set; }
        //定点机构ID
        public string NsId { get; set; }
        //年份
        public string Year { get; set; }
        //月份
        public string Months { get; set; } 
        //应拨金额
        public decimal DueOfPay { get; set; }
        //实际拨金额
        public decimal ActrualPay { get; set; }
        //扣款金额
        public Nullable<decimal> Decut { get; set; }
        //扣款原因备注
        public string DecutReason { get; set; }
        //创建人
        public string CreateBy { get; set; }
        //创建时间
        public Nullable<System.DateTime> CreateTime { get; set; }
        //更新人
        public string UpdateBy { get; set; }
        //更新时间
        public Nullable<System.DateTime> UpdateTime { get; set; }
        //是否删除
        public Nullable<bool> IsDelete { get; set; } 
    
    }


}
