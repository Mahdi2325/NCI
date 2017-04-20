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
    
    public partial class NCI_DRUGBAK
    {
        public string DRUGCODE { get; set; }
        public string MCRULEID { get; set; }
        public string CNNAME { get; set; }
        public string ENNAME { get; set; }
        public string DRUGTYPE { get; set; }
        public string MCTYPE { get; set; }
        public string SFDAAPPROVALNO { get; set; }
        public bool ISPRESCRIPTION { get; set; }
        public bool ISCHINAPRODUCT { get; set; }
        public string ORGIN { get; set; }
        public string MANUFACTURER { get; set; }
        public string SPEC { get; set; }
        public string UNITS { get; set; }
        public string FORM { get; set; }
        public Nullable<int> CONVERSIONRATIO { get; set; }
        public string MINPACKAGE { get; set; }
        public Nullable<int> STANDARDUSAGE { get; set; }
        public string PROVIDER { get; set; }
        public string FREQUENCY { get; set; }
        public string DRUGUSAGEMODE { get; set; }
        public string DRUGUSAGE { get; set; }
        public string ADVERSEREACTION { get; set; }
        public string ATTENTION { get; set; }
        public string ATTENTIONOLDMAN { get; set; }
        public Nullable<decimal> GUIDEPRICE { get; set; }
        public Nullable<decimal> MAXPRICE { get; set; }
        public Nullable<int> NCIMONTHLYMAXUSAGE { get; set; }
        public string COMMENT { get; set; }
        public string PINYIN { get; set; }
        public int STATUS { get; set; }
        public Nullable<System.DateTime> LASTUPDATETIME { get; set; }
        public Nullable<System.DateTime> BAKTIME { get; set; }
        public string BAKBY { get; set; }
    }
}
