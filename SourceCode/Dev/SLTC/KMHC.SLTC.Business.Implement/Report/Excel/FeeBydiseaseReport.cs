using System.Collections.Generic;
using System.Linq;
using ExcelReport;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class FeeByDiseaseReport : BaseExeclReport
    {
        protected override string FileNamePrefix {
            get { return "按疾病汇总"; }
        }
        protected override void CreatFormatter()
        {
            IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            var data = reporService.GetFeeByDiseaseReport(StartTime, EndTime);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            dataFormatter.Add(new CellFormatter(sheetContainer["StartDate"], data.StartDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["EndDate"], data.EndDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCountyResNum"], data.DataDetail.Sum(m=>m.ResNum)));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCountyFee"], data.DataDetail.Sum(m => m.Fee)));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCountyNciPay"], data.DataDetail.Sum(m => m.NciPay)));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalResNum"], data.DataDetail.Sum(m => m.ResNum)));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalFee"], data.DataDetail.Sum(m => m.Fee)));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalNciPay"], data.DataDetail.Sum(m => m.NciPay)));

            var indexNum = 1;
            var tableFormatter = new TableFormatter<FeeByDiseaseReportItem>(sheetContainer["Index"], data.DataDetail,
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["Index"], t => indexNum++),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["Disease"], t => t.Disease),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["CountyResNum"], t => t.ResNum),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["CountyFee"], t => t.Fee),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["CountyNciPay"], t => t.NciPay),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["TotalResNum"], t => t.ResNum),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["TotalFee"], t => t.Fee),
                                        new CellFormatter<FeeByDiseaseReportItem>(sheetContainer["TotalNciPay"], t => t.NciPay)
            );
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }
    }

}
