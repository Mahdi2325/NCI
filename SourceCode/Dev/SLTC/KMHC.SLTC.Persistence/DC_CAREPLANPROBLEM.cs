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
    
    public partial class DC_CAREPLANPROBLEM
    {
        public DC_CAREPLANPROBLEM()
        {
            this.DC_CAREPLANACTIVITY = new HashSet<DC_CAREPLANACTIVITY>();
            this.DC_CAREPLANDATA = new HashSet<DC_CAREPLANDATA>();
            this.DC_CAREPLANEVAL = new HashSet<DC_CAREPLANEVAL>();
            this.DC_CAREPLANGOAL = new HashSet<DC_CAREPLANGOAL>();
            this.DC_CAREPLANREASON = new HashSet<DC_CAREPLANREASON>();
        }
    
        public int CPNO { get; set; }
        public string LEVELPR { get; set; }
        public string CATEGORYTYPE { get; set; }
        public string DIAPR { get; set; }
        public string ORGID { get; set; }
    
        public virtual ICollection<DC_CAREPLANACTIVITY> DC_CAREPLANACTIVITY { get; set; }
        public virtual ICollection<DC_CAREPLANDATA> DC_CAREPLANDATA { get; set; }
        public virtual ICollection<DC_CAREPLANEVAL> DC_CAREPLANEVAL { get; set; }
        public virtual ICollection<DC_CAREPLANGOAL> DC_CAREPLANGOAL { get; set; }
        public virtual ICollection<DC_CAREPLANREASON> DC_CAREPLANREASON { get; set; }
    }
}