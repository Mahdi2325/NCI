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
    
    public partial class NCI_NURSINGHOMESTAFF
    {
        public string NSSTAFFID { get; set; }
        public Nullable<int> USERID { get; set; }
        public string NSID { get; set; }
        public string DEPARMENT { get; set; }
        public string POSITION { get; set; }
        public string LEVEL { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    
        public virtual NCI_NURSINGHOME NCI_NURSINGHOME { get; set; }
        public virtual NCI_USER NCI_USER { get; set; }
    }
}