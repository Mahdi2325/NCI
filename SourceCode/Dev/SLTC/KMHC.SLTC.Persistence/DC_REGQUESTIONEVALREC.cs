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
    
    public partial class DC_REGQUESTIONEVALREC
    {
        public DC_REGQUESTIONEVALREC()
        {
            this.DC_REGEVALQUESTION = new HashSet<DC_REGEVALQUESTION>();
        }
    
        public int EVALRECID { get; set; }
        public Nullable<float> SCORE { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public string EVALRESULT { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CHECKDATE { get; set; }
        public string CHECKEDBY { get; set; }
        public string ORGID { get; set; }
    
        public virtual ICollection<DC_REGEVALQUESTION> DC_REGEVALQUESTION { get; set; }
    }
}
