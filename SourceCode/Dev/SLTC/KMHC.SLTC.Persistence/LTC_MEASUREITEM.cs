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
    
    public partial class LTC_MEASUREITEM
    {
        public LTC_MEASUREITEM()
        {
            this.LTC_MEASUREDRECORD = new HashSet<LTC_MEASUREDRECORD>();
        }
    
        public string ITEMCODE { get; set; }
        public string ITEMNAME { get; set; }
        public Nullable<float> UPPER { get; set; }
        public Nullable<float> LOWER { get; set; }
    
        public virtual ICollection<LTC_MEASUREDRECORD> LTC_MEASUREDRECORD { get; set; }
    }
}
