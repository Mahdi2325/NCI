using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report.Excel
{
    public class FeeByCareTypeMain
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string reportType { get; set; }
        public FeeByCareTypeDetail SumTotal { get; set; }
        public List<FeeByCareTypeDetail> Detail { get; set; }
    }

    public class FeeByCareTypeDetail
    {
        public int Index { get; set; }
        public string OrgName { get; set; }

        public long SpecCareResNum { get; set; }
        public long SpecCareHospDay { get; set; }
        public decimal SpecCareFee { get; set; }
        public decimal SpecCareNciPay { get; set; }

        public long OrgCareResNum { get; set; }
        public long OrgCareHospDay { get; set; }
        public decimal OrgCareFee { get; set; }
        public decimal OrgCareNciPay { get; set; }

        public long ResNum { get; set; }
        public decimal Fee { get; set; }
        public decimal NciPay { get; set; }

    }

    public class TempFeeByCareTypeDetail
    {
        public int Index { get; set; }
        public string OrgName { get; set; }
        public int? CareType { get; set; }

        public long ResNum { get; set; }
        public long HospDay { get; set; }
        public decimal Fee { get; set; }
        public decimal NciPay { get; set; }
    }
}
