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
    public class FeeDetailReport : BaseExeclReport
    {

        protected override string FileNamePrefix
        {
            get { return "医疗机构明细汇总"; }
        }
        protected override void CreatFormatter()
        {
            IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            var data = reporService.GetFeeDetailReport(StartTime, EndTime);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            var indexNum = 1;
            var tableFormatter = new TableFormatter<FeeDetail>(sheetContainer["Index"], data.DataDetail,
                                             new CellFormatter<FeeDetail>(sheetContainer["Index"], t => indexNum++),
                                             new CellFormatter<FeeDetail>(sheetContainer["Name"], t => t.Name),
                                             new CellFormatter<FeeDetail>(sheetContainer["Sex"], t => t.Sex),
                                             new CellFormatter<FeeDetail>(sheetContainer["Age"], t => t.Age),
                                             new CellFormatter<FeeDetail>(sheetContainer["NCINo"], t => t.NCINo),
                                             new CellFormatter<FeeDetail>(sheetContainer["HospNo"], t => t.HospNo),
                                             new CellFormatter<FeeDetail>(sheetContainer["IDNo"], t => t.IDNo),
                                             new CellFormatter<FeeDetail>(sheetContainer["Phone"], t => t.Phone),
                                             new CellFormatter<FeeDetail>(sheetContainer["Address"], t => t.Address),
                                             new CellFormatter<FeeDetail>(sheetContainer["NSName"], t => t.NSName),
                                             new CellFormatter<FeeDetail>(sheetContainer["McType"], t => t.McType),
                                             new CellFormatter<FeeDetail>(sheetContainer["CareType"], t => t.CareType),
                                             new CellFormatter<FeeDetail>(sheetContainer["HospInDate"], t => t.HospInDate),
                                             new CellFormatter<FeeDetail>(sheetContainer["HospOutDate"], t => t.HospOutDate),
                                             new CellFormatter<FeeDetail>(sheetContainer["HospDay"], t => t.HospDay),
                                             new CellFormatter<FeeDetail>(sheetContainer["Disease"], t => t.Disease),
                                             new CellFormatter<FeeDetail>(sheetContainer["DrugFee"], t => t.DrugFee),
                                             new CellFormatter<FeeDetail>(sheetContainer["NCIDrugFee"], t => t.NCIDrugFee),
                                             new CellFormatter<FeeDetail>(sheetContainer["TotalAmount"], t => t.TotalAmount),
                                             new CellFormatter<FeeDetail>(sheetContainer["NCILevel"], t => t.NCILevel),
                                             new CellFormatter<FeeDetail>(sheetContainer["NCIScale"], t => t.NCIScale));
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }
    }
}
