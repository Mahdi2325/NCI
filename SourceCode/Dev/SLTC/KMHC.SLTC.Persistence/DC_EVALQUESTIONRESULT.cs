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
    
    public partial class DC_EVALQUESTIONRESULT
    {
        public long ID { get; set; }
        public Nullable<long> RECORDID { get; set; }
        public Nullable<int> QUESTIONID { get; set; }
        public Nullable<int> MAKERID { get; set; }
        public Nullable<decimal> MAKERVALUE { get; set; }
        public Nullable<int> LIMITEDVALUEID { get; set; }
    
        public virtual DC_EVALQUESTION DC_EVALQUESTION { get; set; }
    }
}
