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
    
    public partial class DC_TEAMACTIVITY
    {
        public DC_TEAMACTIVITY()
        {
            this.DC_TEAMACTIVITYDTL = new HashSet<DC_TEAMACTIVITYDTL>();
        }
    
        public int SEQNO { get; set; }
        public string ACTIVITYCODE { get; set; }
        public string ACTIVITYNAME { get; set; }
        public string ORGID { get; set; }
    
        public virtual ICollection<DC_TEAMACTIVITYDTL> DC_TEAMACTIVITYDTL { get; set; }
    }
}