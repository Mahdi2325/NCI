using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report.Excel
{
    //TODO 因为目前数据无法区分乡镇 县级 和县级以上, 所以暂时只用县级数据统计, 注释掉的代码用于后续参考
    public class FeeByDiseaseReportData
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string reportType { get; set; }
        //public FeeByDiseaseReportItem SumTotal { get; set; }
        //public FeeByDiseaseReportItem SumTown { get; set; }
        //public FeeByDiseaseReportItem SumCounty { get; set; }
        //public FeeByDiseaseReportItem SumAboveCounty { get; set; }
        public decimal SumCountyResNum { get; set; }
        public decimal SumCountyFee { get; set; }
        public decimal SumCountyNciPay { get; set; }
        public decimal SumTotalResNum { get; set; }
        public decimal SumTotalFee { get; set; }
        public decimal SumTotalNciPay { get; set; }

        public List<FeeByDiseaseReportItem> DataDetail { get; set; }
    }
    
    //public class FeeByDiseaseReportDetail
    //{
    //    public string Disease { get; set; }
    //    public FeeByDiseaseReportItem Total { get; set; }
    //    public FeeByDiseaseReportItem Town { get; set; }
    //    public FeeByDiseaseReportItem County { get; set; }
    //    public FeeByDiseaseReportItem AboveCounty { get; set; }
    //}

    public class FeeByDiseaseReportItem
    {
        public int Index { get; set; }
        public string Disease { get; set; }
        public long ResNum { get; set; }
        public decimal Fee { get; set; }
        public decimal NciPay { get; set; }
    }
}
