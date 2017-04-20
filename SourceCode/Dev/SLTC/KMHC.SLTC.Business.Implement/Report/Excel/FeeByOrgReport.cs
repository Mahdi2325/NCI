using System.Collections.Generic;
using ExcelReport;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class FeeByOrgReport : BaseExeclReport
    {
        protected override string FileNamePrefix {
            get { return "按机构汇总"; }
        }
        protected override void CreatFormatter()
        {
            IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            var data = reporService.GetFeeByOrgReport(StartTime, EndTime);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            dataFormatter.Add(new CellFormatter(sheetContainer["StartDate"], data.StartDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["EndDate"], data.EndDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurResNum"], data.SumCurrent.ResNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurHospDay"], data.SumCurrent.HospDay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurFee"], data.SumCurrent.Fee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurMedFee"], data.SumCurrent.MedFee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurNciPay"], data.SumCurrent.NciPay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurSvcDep"], data.SumCurrent.SvcDep));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurDecut"], data.SumCurrent.Decut));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurAccNciPay"], data.SumCurrent.AccNciPay));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalResNum"], data.SumTotal.ResNum));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalHospDay"], data.SumTotal.HospDay));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalFee"], data.SumTotal.Fee));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalMedFee"], data.SumTotal.MedFee));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalNciPay"], data.SumTotal.NciPay));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalSvcDep"], data.SumTotal.SvcDep));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalDecut"], data.SumTotal.Decut));
            //dataFormatter.Add(new CellFormatter(sheetContainer["SumTotalAccNciPay"], data.SumTotal.AccNciPay));

            var indexNum = 1;
            var tableFormatter = new TableFormatter<FeeReportComDetail>(sheetContainer["Index"], data.DataDetail,
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["Index"], t => indexNum++),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["OrgName"], t => t.GroupColumn1),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurResNum"], t => t.Current.ResNum),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurHospDay"], t => t.Current.HospDay),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurFee"], t => t.Current.Fee),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurMedFee"], t => t.Current.MedFee),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurNciPay"], t => t.Current.NciPay),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurSvcDep"], t => t.Current.SvcDep),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurDecut"], t => t.Current.Decut),
                                        new CellFormatter<FeeReportComDetail>(sheetContainer["CurAccNciPay"], t => t.Current.AccNciPay)
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalResNum"], t => t.Total.ResNum),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalHospDay"], t => t.Total.HospDay),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalFee"], t => t.Total.Fee),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalMedFee"], t => t.Total.MedFee),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalNciPay"], t => t.Total.NciPay),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalSvcDep"], t => t.Total.SvcDep),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalDecut"], t => t.Total.Decut),
                                        //new CellFormatter<FeeReportComDetail>(sheetContainer["TotalAccNciPay"], t => t.Total.AccNciPay)
            );
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }
    }

}
