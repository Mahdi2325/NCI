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
    
    public partial class LTC_CONSTRAINSBEVAL
    {
        public long ID { get; set; }
        public Nullable<long> SEQNO { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public string REASON { get; set; }
        public string CONSTRAINTWAY { get; set; }
        public string BODYPART { get; set; }
        public string DURATION { get; set; }
        public string TEMPCANCELDESC { get; set; }
        public string SKINDESC { get; set; }
        public string BLOODCIRCLEDESC { get; set; }
        public string EVALUATEBY { get; set; }
    
        public virtual LTC_CONSTRAINTREC LTC_CONSTRAINTREC { get; set; }
    }
}
