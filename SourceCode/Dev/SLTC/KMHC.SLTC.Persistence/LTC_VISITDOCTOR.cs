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
    
    public partial class LTC_VISITDOCTOR
    {
        public string DOCNO { get; set; }
        public string DOCNAME { get; set; }
        public string HOSPNO { get; set; }
        public string DEPTNO { get; set; }
    
        public virtual LTC_VISITDEPT LTC_VISITDEPT { get; set; }
        public virtual LTC_VISITHOSPITAL LTC_VISITHOSPITAL { get; set; }
    }
}