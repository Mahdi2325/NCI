using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExcelReport;
using KMHC.SLTC.Business.Entity.Filter;
using System.Net.Http;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class PersonStatusReport : TaskBaseExeclReport
    {
        protected override string FileNamePrefix
        {
            get { return "人员状态报表"; }
        }
        protected async override System.Threading.Tasks.Task CreatFormatter()
        {
            IPersonStatusReportService service = IOCContainer.Instance.Resolve<IPersonStatusReportService>();
            var response = new BaseResponse<PersonStatusReportModel>();
            response = service.QueryPersonStatusInfo(StartTime, EndTime.AddDays(1).AddSeconds(-1));

            var request = new PersonStatusFilter();
            request.startDate = StartTime;
            request.endDate = EndTime.AddDays(1).AddSeconds(-1);
            try
            {
                var http = HttpClientHelper.NciHttpClient;
                var result = await http.PostAsJsonAsync("/api/PersonStatusReport", request);
                var resultContent = await result.Content.ReadAsStringAsync();
                var ltcInfo = JsonConvert.DeserializeObject<PersonStatusReportModel>(resultContent);
                response.Data.DrugEntryNum = ltcInfo.DrugEntryNum;
                response.Data.NSCPLNum = ltcInfo.NSCPLNum;
                response.Data.BillV2Num = ltcInfo.BillV2Num;
                response.Data.CostNum = ltcInfo.CostNum;
                response.Data.InHospNum = ltcInfo.InHospNum;
                response.Data.OutHospOfOtherNum = ltcInfo.OutHospOfOtherNum;
                response.Data.OutHospOfDeadNum = ltcInfo.OutHospOfDeadNum;
                response.Data.LeaveHospNum = ltcInfo.LeaveHospNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response.ResultCode = -1;
            }
            var parameterContainer = new WorkbookParameterContainer();
            parameterContainer.Load(TemplateFormatterPath);
            #region 资格证待遇申请统计（期间）
            SheetParameterContainer sheetContainer = parameterContainer["资格证待遇申请统计（期间）"];
            var dataFormatter = new List<ElementFormatter>();
            #region 截止2016年年12月31日			
            #region 申请数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xlan"], response.Data.AppCertlastYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qlan"], response.Data.AppCertlastYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jlan"], response.Data.AppCertlastYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tlan"], response.Data.AppCertlastYearNum.TotalNum));
            #endregion
            #region 通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xlbn"], response.Data.ByCertlastYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qlbn"], response.Data.ByCertlastYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jlbn"], response.Data.ByCertlastYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tlbn"], response.Data.ByCertlastYearNum.TotalNum));
            #endregion
            #region 未通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xlun"], response.Data.UnAppCertlastYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qlun"], response.Data.UnAppCertlastYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jlun"], response.Data.UnAppCertlastYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tlun"], response.Data.UnAppCertlastYearNum.TotalNum));
            #endregion
            #region 取消数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xlcn"], response.Data.CancelCertlastYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qlcn"], response.Data.CancelCertlastYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jlcn"], response.Data.CancelCertlastYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tlcn"], response.Data.CancelCertlastYearNum.TotalNum));
            #endregion
            #endregion
            #region 截止2017年3月31日			
            #region 申请数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xan"], response.Data.AppCertYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qan"], response.Data.AppCertYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jan"], response.Data.AppCertYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tan"], response.Data.AppCertYearNum.TotalNum));
            #endregion
            #region 通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xbn"], response.Data.ByCertYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qbn"], response.Data.ByCertYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jbn"], response.Data.ByCertYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tbn"], response.Data.ByCertYearNum.TotalNum));
            #endregion
            #region 未通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xun"], response.Data.UnAppCertYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qun"], response.Data.UnAppCertYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jun"], response.Data.UnAppCertYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tun"], response.Data.UnAppCertYearNum.TotalNum));
            #endregion
            #region 取消数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xcn"], response.Data.CancelCertYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qcn"], response.Data.CancelCertYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jcn"], response.Data.CancelCertYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tcn"], response.Data.CancelCertYearNum.TotalNum));
            #endregion

            #endregion
            #region 查询期间			
            #region 申请数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xasn"], response.Data.AppCertSearchYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qasn"], response.Data.AppCertSearchYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jasn"], response.Data.AppCertSearchYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tasn"], response.Data.AppCertSearchYearNum.TotalNum));
            #endregion
            #region 通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xsbn"], response.Data.ByCertSearchYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qsbn"], response.Data.ByCertSearchYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jsbn"], response.Data.ByCertSearchYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tsbn"], response.Data.ByCertSearchYearNum.TotalNum));
            #endregion
            #region 未通过数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xsun"], response.Data.UnAppCertSearchYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qsun"], response.Data.UnAppCertSearchYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jsun"], response.Data.UnAppCertSearchYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tsun"], response.Data.UnAppCertSearchYearNum.TotalNum));
            #endregion
            #region 取消数量
            dataFormatter.Add(new CellFormatter(sheetContainer["xscn"], response.Data.CancelCertSearchYearNum.XyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["qscn"], response.Data.CancelCertSearchYearNum.QkyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["jscn"], response.Data.CancelCertSearchYearNum.JmyyNum));
            dataFormatter.Add(new CellFormatter(sheetContainer["tscn"], response.Data.CancelCertSearchYearNum.TotalNum));
            #endregion

            #endregion

            Formatter = new SheetFormatter("资格证待遇申请统计（期间）", dataFormatter.ToArray());
            #endregion

            #region 操作数据（期间）
            SheetParameterContainer billsheetContainer = parameterContainer["操作数据（期间）"];
            var billdataFormatter = new List<ElementFormatter>();
            #region 药品导入数
            billdataFormatter.Add(new CellFormatter(billsheetContainer["xden"], response.Data.DrugEntryNum.XyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["qden"], response.Data.DrugEntryNum.QkyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["jden"], response.Data.DrugEntryNum.JmyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["tden"], response.Data.DrugEntryNum.TotalNum));
            #endregion
            #region 护理计划
            billdataFormatter.Add(new CellFormatter(billsheetContainer["xnspn"], response.Data.NSCPLNum.XyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["qnspn"], response.Data.NSCPLNum.QkyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["jnspn"], response.Data.NSCPLNum.JmyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["tnspn"], response.Data.NSCPLNum.TotalNum));
            #endregion
            #region 账单数量
            billdataFormatter.Add(new CellFormatter(billsheetContainer["xbilln"], response.Data.BillV2Num.XyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["qbilln"], response.Data.BillV2Num.QkyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["jbilln"], response.Data.BillV2Num.JmyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["tbilln"], response.Data.BillV2Num.TotalNum));
            #endregion
            #region 总费用
            billdataFormatter.Add(new CellFormatter(billsheetContainer["xcostn"], response.Data.CostNum.XyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["qcostn"], response.Data.CostNum.QkyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["jcostn"], response.Data.CostNum.JmyyNum));
            billdataFormatter.Add(new CellFormatter(billsheetContainer["tcostn"], response.Data.CostNum.TotalNum));
            #endregion
            Formatter1 = new SheetFormatter("操作数据（期间）", billdataFormatter.ToArray());
            #endregion

            #region 在院不在院情况统计（状态）
            SheetParameterContainer hospsheetContainer = parameterContainer["在院不在院情况统计（状态）"];
            var hospdataFormatter = new List<ElementFormatter>();
            #region 在院人数
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["xinhospn"], response.Data.InHospNum.XyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["qinhospn"], response.Data.InHospNum.QkyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["jinhospn"], response.Data.InHospNum.JmyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["tinhospn"], response.Data.InHospNum.TotalNum));
            #endregion
            #region 出院人数	 - 其他
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["xotherhospn"], response.Data.OutHospOfOtherNum.XyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["qotherhospn"], response.Data.OutHospOfOtherNum.QkyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["jotherhospn"], response.Data.OutHospOfOtherNum.JmyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["totherhospn"], response.Data.OutHospOfOtherNum.TotalNum));
            #endregion
            #region 出院人数	 - 死亡
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["xdeadhospn"], response.Data.OutHospOfDeadNum.XyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["qdeadhospn"], response.Data.OutHospOfDeadNum.QkyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["jdeadhospn"], response.Data.OutHospOfDeadNum.JmyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["tdeadhospn"], response.Data.OutHospOfDeadNum.TotalNum));
            #endregion
            #region 请假天数
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["xleahospn"], response.Data.LeaveHospNum.XyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["qleahospn"], response.Data.LeaveHospNum.QkyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["jleahospn"], response.Data.LeaveHospNum.JmyyNum));
            hospdataFormatter.Add(new CellFormatter(hospsheetContainer["tleahospn"], response.Data.LeaveHospNum.TotalNum));
            #endregion
            Formatter2 = new SheetFormatter("在院不在院情况统计（状态）", hospdataFormatter.ToArray());
            #endregion
        }
    }
}
