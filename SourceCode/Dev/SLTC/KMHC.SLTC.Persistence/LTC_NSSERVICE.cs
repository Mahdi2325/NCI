//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KMHC.SLTC.Persistence
{
    using System;
    using System.Collections.Generic;
    
    public partial class LTC_NSSERVICE
    {
        public LTC_NSSERVICE()
        {
            this.LTC_SERVICERECORD = new HashSet<LTC_SERVICERECORD>();
        }
    
        public int SERVICEID { get; set; }
        public string ACCOUNTINGID { get; set; }
        public string CHARGETYPEID { get; set; }
        public string NSID { get; set; }
        public bool ISNCIITEM { get; set; }
        public string MCSERVICECODE { get; set; }
        public string NSSERVICECODE { get; set; }
        public string MCRULEID { get; set; }
        public string SERVICENAME { get; set; }
        public string SERVICEDESC { get; set; }
        public string UNITS { get; set; }
        public decimal UNITPRICE { get; set; }
        public Nullable<decimal> GUIDEPRICE { get; set; }
        public Nullable<decimal> MAXPRICE { get; set; }
        public string COMMENT { get; set; }
        public Nullable<System.DateTime> LASTUPDATETIME { get; set; }
        public string PINYIN { get; set; }
        public int STATUS { get; set; }
        public bool ISREQUIREUPDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    
        public virtual ICollection<LTC_SERVICERECORD> LTC_SERVICERECORD { get; set; }
    }
}