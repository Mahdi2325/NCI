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
    
    public partial class NCI_CODEDTL
    {
        public string ITEMTYPE { get; set; }
        public string ITEMCODE { get; set; }
        public string ITEMNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> ORDERSEQ { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public string UPDATEBY { get; set; }
    
        public virtual NCI_CODEFILE NCI_CODEFILE { get; set; }
    }
}
