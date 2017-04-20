using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report.Excel
{
    public class FeeReportComData
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string reportType { get; set; }
        public FeeReportComItem SumCurrent { get; set; }
        public FeeReportComItem SumTotal { get; set; }

        public List<FeeReportComDetail> DataDetail { get; set; }
    }

    public class FeeReportComDetail
    {
        public int Index { get; set; }
        public string GroupColumn1 { get; set; }
        public string GroupColumn2 { get; set; }
        public FeeReportComItem Current { get; set; }
        public FeeReportComItem Total { get; set; }
    }
}
