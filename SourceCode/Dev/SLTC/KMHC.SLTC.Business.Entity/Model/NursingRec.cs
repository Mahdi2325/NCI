using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NursingRec
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string ClassType { get; set; }
        public string Content { get; set; }
        public string RecordBy { get; set; }
        public string RecordNameBy { get; set; }
        public string PrintFlag { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}





