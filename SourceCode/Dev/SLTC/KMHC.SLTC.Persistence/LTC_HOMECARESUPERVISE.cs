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
    
    public partial class LTC_HOMECARESUPERVISE
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> SUPERVISEDATE { get; set; }
        public string OPERATOR { get; set; }
        public string SUPERVISOR { get; set; }
        public string CONTACTTYPE { get; set; }
        public Nullable<int> MINUTES { get; set; }
        public string SUPERVISEDESC { get; set; }
        public string ASSESSMENT { get; set; }
        public string PROCESSDESC { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CONTACTDATE { get; set; }
        public string ORGID { get; set; }
    }
}
