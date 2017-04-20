using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_RegTotalResult
    {
        public int Id { get; set; }
        public string IdNo { get; set; }
        public int? ConsistencyFlag { get; set; }
        public int? AdlScore { get; set; }
        public string AdlResult { get; set; }
        public int? MmseScore { get; set; }
        public string MmseResult { get; set; }
        public int? IadlScore { get; set; }
        public string IadlResult { get; set; }
        public int? GdsScore { get; set; }
        public string GdsResult { get; set; }
        public decimal? TotalScore { get; set; }
        public string Result { get; set; }
        public int? CarelevelId { get; set; }
        public Nullable<DateTime> EvalDate { get; set; }
        public Nullable<DateTime> ExpDate { get; set; }
        public bool IsvalId { get; set; }
        public Nullable<bool> IsAudit { get; set; }
        public string Serveadvise { get; set; }
        public string Conclusion { get; set; }

    }
}
