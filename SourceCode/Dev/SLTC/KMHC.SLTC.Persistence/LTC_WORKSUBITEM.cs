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
    
    public partial class LTC_WORKSUBITEM
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string ITEMCODE { get; set; }
        public Nullable<int> MININUM { get; set; }
        public Nullable<int> MAXINUM { get; set; }
        public Nullable<int> ORDER { get; set; }
        public string REMARKS { get; set; }
    
        public virtual LTC_WORKITEM LTC_WORKITEM { get; set; }
    }
}
