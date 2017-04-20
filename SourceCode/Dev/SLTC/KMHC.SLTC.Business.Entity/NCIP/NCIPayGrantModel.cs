using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
   public class NCIPayGrantModel
    {

       public int GrantID { get; set; }
       public Nullable<int> ServiceDepositID { get; set; }
       public string NSID { get; set; }
       public string GrantYear { get; set; }
       public int TotalResident { get; set; }
       public int TotalHospDay { get; set; }
       public decimal TotalAmount { get; set; }
       public decimal TotalNCIpay { get; set; }
       public int Status { get; set; }
       public string CreatorName { get; set; }
       public int ICResult { get; set; }
       public string ICComment { get; set; }
       public Nullable<System.DateTime> ICOperateTime { get; set; }
       public string ICUserID { get; set; }
       public string ICID { get; set; }
       public Nullable<int> AgencyResult { get; set; }
       public string AgencyComment { get; set; }
       public Nullable<System.DateTime> AgencyOperateTime { get; set; }
       public string AgencyUserID { get; set; }
       public string AgencyID { get; set; }
       public Nullable<System.DateTime> GrantTime { get; set; }
       public Nullable<decimal> AdjustAmount { get; set; }
       public string AdjustReason { get; set; }
       public string CreateBy { get; set; }
       public Nullable<System.DateTime> CreateTime { get; set; }
       public string UpdateBy { get; set; }
       public Nullable<System.DateTime> UpdateTime { get; set; }
       public Nullable<bool> IsDelete { get; set; }
    }

   public class NCIPayGrantEntity : NCIPayGrantModel
   {
       public NCIPayGrantEntity()
       {
           monfeeList = new List<MonFeeEntity>();
       }
       /// <summary>
       /// 结算月份
       /// </summary>
       public string GrantMonTh { get; set; }

       public decimal ServiceSecurity { get; set; }

       /// <summary>
       /// 实际支付金额
       /// </summary>
       public decimal ActualPayment { get; set; }


       public List<MonFeeEntity> monfeeList { get; set; }
   }
}
