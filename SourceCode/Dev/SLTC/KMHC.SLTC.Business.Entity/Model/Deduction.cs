#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/21 16:35:23
** 说明     :
******************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Deduction : ISoftDeleteEntity
    {
        public int ID { get; set; }
        public Nullable<long> NSMONFEEID { get; set; }
        public string NSNO { get; set; }
        public string RESIDENTSSID { get; set; }
        public int DEDUCTIONTYPE { get; set; }
        public string Debitmonth { get; set; }
        public Nullable<int> DEBITDAYS { get; set; }
        public double Amount { get; set; }
        public string DeductionReason { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
    }
}
