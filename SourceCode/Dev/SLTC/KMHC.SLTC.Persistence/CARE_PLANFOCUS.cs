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
    
    public partial class CARE_PLANFOCUS
    {
        public int CF_NO { get; set; }
        public Nullable<int> CP_NO { get; set; }
        public string DIAPR { get; set; }
        public string FOCUSPR { get; set; }
    
        public virtual CARE_PLANPROBLEM CARE_PLANPROBLEM { get; set; }
    }
}
