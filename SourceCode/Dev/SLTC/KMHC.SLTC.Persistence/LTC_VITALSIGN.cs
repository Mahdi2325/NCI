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
    
    public partial class LTC_VITALSIGN
    {
        public long SEQNO { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> SBP { get; set; }
        public Nullable<int> DBP { get; set; }
        public Nullable<int> PULSE { get; set; }
        public Nullable<decimal> BODYTEMP { get; set; }
        public Nullable<int> BREATHE { get; set; }
        public Nullable<int> OXYGEN { get; set; }
        public Nullable<decimal> BLOODSUGAR { get; set; }
        public string BSTYPE { get; set; }
        public Nullable<decimal> HEIGHT { get; set; }
        public Nullable<decimal> WEIGHT { get; set; }
        public string COMA { get; set; }
        public Nullable<int> PAIN { get; set; }
        public Nullable<int> BOWELS { get; set; }
        public string CLASSTYPE { get; set; }
        public Nullable<System.DateTime> RECORDDATE { get; set; }
        public string ORGID { get; set; }
    }
}
