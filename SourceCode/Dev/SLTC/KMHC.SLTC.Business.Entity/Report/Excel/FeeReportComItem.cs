using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report.Excel
{
    public class FeeReportComItem
    {
        public long ResNum { get; set; }
        public long HospDay { get; set; }
        public decimal Fee { get; set; }
        public decimal MedFee { get; set; }
        public decimal NciPay { get; set; }
        public decimal SvcDep { get; set; }
        public decimal Decut { get; set; }
        public decimal AccNciPay { get; set; }
    }
}
