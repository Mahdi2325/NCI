﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResidentDtl
    {
        public long FeeNo { get; set; }
        public string SourceType { get; set; }
        public string InsType { get; set; }
        public string InReason { get; set; }
        public string InType { get; set; }
        public string EconomyFlag { get; set; }
        public string CaseType { get; set; }
        public string HospFile { get; set; }
        public string InsMark { get; set; }
        public bool CustodyFlag { get; set; }
        public bool AssistFlag { get; set; }
        public Nullable<System.DateTime> LpappDate { get; set; }
        public Nullable<System.DateTime> LpcheckDate { get; set; }
        public string AppdocNo { get; set; }
        public string IllFlag { get; set; }
        public string CertifyNo { get; set; }
        public bool Needcertify { get; set; }
        public Nullable<System.DateTime> CertifyDate { get; set; }
        public bool BookFlag { get; set; }
        public string BookNo { get; set; }
        public Nullable<System.DateTime> BookissueDate { get; set; }
        public Nullable<System.DateTime> BookexpDate { get; set; }
        public string BookType { get; set; }
        public string Disabdegree { get; set; }
        public string Iq { get; set; }
        public string DisabCause { get; set; }
        public string DisabTypeDtl { get; set; }
        public string DisabCheckDesc { get; set; }
        public string Procdoc { get; set; }
        public bool InsFlag { get; set; }
        public string DoclackDesc { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
        public string PType { get; set; }
        public Nullable<int> RegNo { get; set; }

        public string InsuranceDesc { get; set; }
    }
}





