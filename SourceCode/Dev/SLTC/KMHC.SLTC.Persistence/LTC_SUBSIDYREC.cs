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
    
    public partial class LTC_SUBSIDYREC
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string ITEMNAME { get; set; }
        public Nullable<System.DateTime> APPLYDATE { get; set; }
        public Nullable<System.DateTime> NEXTAPPLYDATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string APPLYBY { get; set; }
        public string NEXTAPPLYBY { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
    }
}
