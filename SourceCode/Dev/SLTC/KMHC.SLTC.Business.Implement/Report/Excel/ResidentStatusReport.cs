using ExcelReport;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Interface.NCIP;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.IO;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class ResidentStatusReport : BaseExeclReport
    {

        protected override string FileNamePrefix
        {
            get { return "人员状态列表"; }
        }
        protected override void CreatFormatter()
        {
            //IReportService reporService = IOCContainer.Instance.Resolve<IReportService>();

            //var data = reporService.GetFeeDetailReport(StartTime, EndTime);


            var resultContent = new BaseResponse<IList<RegInHosStatusListEntity>>();
            resultContent = GetSLTCRegInHosStatus();
            IRegInHosStatusListService service = IOCContainer.Instance.Resolve<IRegInHosStatusListService>();
            var response = new BaseResponse<RegInHosStatusDtlData>();
            response = service.QueryRegInHosStatustList(Name, IdNo, NsId, Status, resultContent);

            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            SheetParameterContainer sheetContainer = parameterContainer["Sheet1"];

            var dataFormatter = new List<ElementFormatter>();
            var indexNum = 1;
            var tableFormatter = new TableFormatter<RegInHosStatusDtlEntity>(sheetContainer["Index"], response.Data.RegInHosStatusDtl,
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["Index"], t => indexNum++),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["Name"], t => t.Name),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["IDNo"], t => t.IdNo),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["NCINo"], t => t.SsNo),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["Sex"], t => t.Gender),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["Age"], t => t.Age),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["InHosStatus"], t => t.InHosStatus.ToString().Replace("0", "在院").Replace("1", "未入院").Replace("2", "请假").Replace("3", "出院")),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["HospInDate"], t => t.InDate == null ? "" : t.InDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["HospOutDate"], t => t.OutDate == null ? "" : t.OutDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["StartDate"], t => t.StartDate == null ? "" : t.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["ReturnDate"], t => t.ReturnDate == null ? "" : t.ReturnDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                                             new CellFormatter<RegInHosStatusDtlEntity>(sheetContainer["LeHour"], t => t.LeHour));
            dataFormatter.Add(tableFormatter);
            Formatter = new SheetFormatter("Sheet1", dataFormatter.ToArray());
        }


        public BaseResponse<IList<RegInHosStatusListEntity>> GetSLTCRegInHosStatus()
        {
            string apiAdd = "/api/leaveHosp/QueryRegInHosStatusList";
            string ServiceAddress = System.Configuration.ConfigurationManager.AppSettings["nciAddress"].ToString();
            BaseResponse<IList<RegInHosStatusListEntity>> response = new BaseResponse<IList<RegInHosStatusListEntity>>();
            string Address = string.Format("{0}{1}", ServiceAddress, apiAdd);
            string Json = CallService(Address);
            var JsonData = JsonConvert.DeserializeObject<BaseResponse<IList<RegInHosStatusListEntity>>>(Json);
            response.Data = JsonData.Data;
            return response;
        }


        public static string CallService(string address)
        {
            string timeStamp = DateTime.Now.ToString();
            var myReq = System.Net.WebRequest.Create(address) as System.Net.HttpWebRequest;
            myReq.Method = "GET";
            myReq.ContentType = "application/x-www-form-urlencoded";
            var myResponse = myReq.GetResponse() as System.Net.HttpWebResponse;
            var reader = new StreamReader(myResponse.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}
