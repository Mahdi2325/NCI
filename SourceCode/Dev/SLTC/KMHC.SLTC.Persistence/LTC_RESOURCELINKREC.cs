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
    
    public partial class LTC_RESOURCELINKREC
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string RECORDBY { get; set; }
        public Nullable<System.DateTime> CONTACTDATE { get; set; }
        public Nullable<System.DateTime> FINISHDATE { get; set; }
        public string TYPE { get; set; }
        public string NAME { get; set; }
        public string EVALRESULT { get; set; }
        public string UNIT { get; set; }
        public string RESOURCESTATUS { get; set; }
        public string REASON { get; set; }
        public string REGSTATE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
    }
}