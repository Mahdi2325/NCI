using ExcelReport;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class FeeApprovalReport : BaseExeclReport
    {
        protected override string FileNamePrefix
        {
            get { return "待遇报批表"; }
        }
        protected override void CreatFormatter()
        {
            IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            var data = reporService.GetFeeByCareTypeMainReport(StartTime, EndTime);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            dataFormatter.Add(new CellFormatter(sheetContainer["StartDate"], data.StartDate+" - "));
            dataFormatter.Add(new CellFormatter(sheetContainer["EndDate"], data.EndDate));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumSpecCareResNum"], data.SumTotal.SpecCareResNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumSpecCareHospDay"], data.SumTotal.SpecCareHospDay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumSpecCareFee"], data.SumTotal.SpecCareFee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumSpecCareNciPay"], data.SumTotal.SpecCareNciPay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumOrgCareResNum"], data.SumTotal.OrgCareResNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumOrgCareHospDay"], data.SumTotal.OrgCareHospDay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumOrgCareFee"], data.SumTotal.OrgCareFee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumOrgCareNciPay"], data.SumTotal.OrgCareNciPay));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurResNum"], data.SumTotal.ResNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurFee"], data.SumTotal.Fee));
            dataFormatter.Add(new CellFormatter(sheetContainer["SumCurNciPay"], data.SumTotal.NciPay));
            var indexNum = 1;
            var tableFormatter = new TableFormatter<FeeByCareTypeDetail>(sheetContainer["Index"], data.Detail,
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["Index"], t => indexNum++),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["OrgName"], t => t.OrgName),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["SpecCareResNum"], t => t.SpecCareResNum),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["SpecCareHospDay"], t => t.SpecCareHospDay),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["SpecCareFee"], t => t.SpecCareFee),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["SpecCareNciPay"], t => t.SpecCareNciPay),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["OrgCareResNum"], t => t.OrgCareResNum),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["OrgCareHospDay"], t => t.OrgCareHospDay),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["OrgCareFee"], t => t.OrgCareFee),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["OrgCareNciPay"], t => t.OrgCareNciPay),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["CurResNum"], t => t.ResNum),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["CurFee"], t => t.Fee),
                                        new CellFormatter<FeeByCareTypeDetail>(sheetContainer["CurNciPay"], t => t.NciPay)
            );
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }
    }
}
