using System.Collections.Generic;
using ExcelReport;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class FeeByLevelReport : BaseExeclReport
    {
        protected override string FileNamePrefix {
            get { return "按级别汇总"; }
        }
        protected override void CreatFormatter()
        {
            IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            var data = reporService.GetFeeByLevelReport(StartTime, EndTime);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            dataFormatter.Add(new CellFormatter(sheetContainer["StartDate"], data.StartDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["EndDate"], data.EndDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurResNum"], data.SumCurrent.ResNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurFee"], data.SumCurrent.Fee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurMedFee"], data.SumCurrent.MedFee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurNciPay"], data.SumCurrent.NciPay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurHospDay"], data.SumCurrent.HospDay));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalResNum"], data.SumTotal.ResNum));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalFee"], data.SumTotal.Fee));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalMedFee"], data.SumTotal.MedFee));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalNciPay"], data.SumTotal.NciPay));

            var indexNum = 1;
            var tableFormatter = new TableFormatter<FeeReportComDetail>(sheetContainer["Index"], data.DataDetail,
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["Index"], t => indexNum++),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["Level"], t => t.GroupColumn1),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurResNum"], t => t.Current.ResNum),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurFee"], t => t.Current.Fee),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurMedFee"], t => t.Current.MedFee),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurNciPay"], t => t.Current.NciPay),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurHospDay"], t => t.Current.HospDay)
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalResNum"], t => t.Total.ResNum),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalFee"], t => t.Total.Fee),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalMedFee"], t => t.Total.MedFee),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalNciPay"], t => t.Total.NciPay)
            );
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }
    }

}
