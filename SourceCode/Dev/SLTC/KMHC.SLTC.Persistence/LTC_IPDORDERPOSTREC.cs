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
    
    public partial class LTC_IPDORDERPOSTREC
    {
        public long ORDERPOSTRECNO { get; set; }
        public Nullable<long> ORDERNO { get; set; }
        public Nullable<System.DateTime> POSTDATE { get; set; }
        public string NURSENO { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}