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
    
    public partial class LTC_SCENARIO_ITEM_OPTION
    {
        public int ID { get; set; }
        public Nullable<int> SCENARIOITEMID { get; set; }
        public Nullable<int> OPTIONID { get; set; }
        public string OPTIONNAME { get; set; }
    
        public virtual LTC_SCENARIO_ITEM LTC_SCENARIO_ITEM { get; set; }
    }
}
