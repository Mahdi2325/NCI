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
    
    public partial class DC_MULTITEAMCAREPLAN
    {
        public long ID { get; set; }
        public string MAJORTYPE { get; set; }
        public string QUESTIONTYPE { get; set; }
        public string ACTIVITY { get; set; }
        public string TRACEDESC { get; set; }
        public Nullable<long> SEQNO { get; set; }
    
        public virtual DC_MULTITEAMCAREPLANREC DC_MULTITEAMCAREPLANREC { get; set; }
    }
}
