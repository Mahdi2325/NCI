using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    //资格申请表
    public class ServiceDeposit
    {
        //服务保证金记录id
        public int ServiceDepositId { get; set; }
        //年拨付记录ID
        public int SdGrantId { get; set; }
        //定点服务机构ID
        public string NsId { get; set; } 
        //报销年月份
        public string YearMonth{ get; set; } 
        //金额
        public decimal Amount { get; set; }
        //保证金生成日期
        public DateTime ServiceDepositDate{ get; set; }
        //状态
        public int Status { get; set; }
       //创建人
        public string CreateBy { get; set; }
        //创建时间
        public DateTime? CreateTime { get; set; }
        //更新人
        public string UpdateBy { get; set; }
        //更新时间
        public DateTime? UpdateTime { get; set; }
        //是否删除
        public bool? IsDelete { get; set; }
    }


}
