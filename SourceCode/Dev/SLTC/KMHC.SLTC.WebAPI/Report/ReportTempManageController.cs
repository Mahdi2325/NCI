using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace KMHC.SLTC.WebAPI.Report
{
    [RoutePrefix("api/ReportTempManage")]
    public class ReportTempManageController : BaseController
    {
        IReportService service = IOCContainer.Instance.Resolve<IReportService>();
        IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
        ILtcService ltcService = IOCContainer.Instance.Resolve<ILtcService>();

        [Route(""), HttpGet]
        public async Task<IHttpActionResult> Get(int CurrentPage, int PageSize, DateTime? startDate = null, DateTime? endDate = null, string mark = "",string nsno="")
        {

            switch (mark)
            {
                case "careType":
                    var FeeByCareTypeReportResponse = new BaseResponse<FeeReportComData>();
                    FeeByCareTypeReportResponse.Data = service.GetFeeByCareTypeReport(startDate, endDate);
                    if (FeeByCareTypeReportResponse.Data == null) FeeByCareTypeReportResponse.Data = new FeeReportComData();
                    FeeByCareTypeReportResponse.Data.reportType = "careType";
                    FeeByCareTypeReportResponse.RecordsCount = FeeByCareTypeReportResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeByCareTypeReportResponse.RecordsCount / PageSize;
                        if (FeeByCareTypeReportResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeByCareTypeReportResponse.PagesCount = count;
                        FeeByCareTypeReportResponse.Data.DataDetail = FeeByCareTypeReportResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeByCareTypeReportResponse.PagesCount = 1;
                    }
                    return Ok(FeeByCareTypeReportResponse);
                case "disease":
                    var FeeByDiseaseReportResponse = new BaseResponse<FeeByDiseaseReportData>();

                    FeeByDiseaseReportResponse.Data = service.GetFeeByDiseaseReport(startDate, endDate);
                    if (FeeByDiseaseReportResponse.Data == null) FeeByDiseaseReportResponse.Data = new FeeByDiseaseReportData();
                    FeeByDiseaseReportResponse.Data.reportType = "disease";
                    FeeByDiseaseReportResponse.Data.SumCountyResNum = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.ResNum);
                    FeeByDiseaseReportResponse.Data.SumCountyFee = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.Fee);
                    FeeByDiseaseReportResponse.Data.SumCountyNciPay = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.NciPay);
                    FeeByDiseaseReportResponse.Data.SumTotalResNum = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.ResNum);
                    FeeByDiseaseReportResponse.Data.SumTotalFee = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.Fee);
                    FeeByDiseaseReportResponse.Data.SumTotalNciPay = FeeByDiseaseReportResponse.Data.DataDetail.Sum(m => m.NciPay);
                    FeeByDiseaseReportResponse.RecordsCount = FeeByDiseaseReportResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeByDiseaseReportResponse.RecordsCount / PageSize;
                        if (FeeByDiseaseReportResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeByDiseaseReportResponse.PagesCount = count;
                        FeeByDiseaseReportResponse.Data.DataDetail = FeeByDiseaseReportResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeByDiseaseReportResponse.PagesCount = 1;
                    }
                    return Ok(FeeByDiseaseReportResponse);
                case "ns":
                    var FeeByOrgReportResponse = new BaseResponse<FeeReportComData>();

                    FeeByOrgReportResponse.Data = service.GetFeeByOrgReport(startDate, endDate);
                    if (FeeByOrgReportResponse.Data == null) FeeByOrgReportResponse.Data = new FeeReportComData();
                    FeeByOrgReportResponse.Data.reportType = "ns";
                    FeeByOrgReportResponse.RecordsCount = FeeByOrgReportResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeByOrgReportResponse.RecordsCount / PageSize;
                        if (FeeByOrgReportResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeByOrgReportResponse.PagesCount = count;
                        FeeByOrgReportResponse.Data.DataDetail = FeeByOrgReportResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeByOrgReportResponse.PagesCount = 1;
                    }
                    return Ok(FeeByOrgReportResponse);
                case "govArea":
                    var FeeByRegionReportResponse = new BaseResponse<FeeReportComData>();
                    FeeByRegionReportResponse.Data = service.GetFeeByRegionReport(startDate, endDate);
                    if (FeeByRegionReportResponse.Data == null) FeeByRegionReportResponse.Data = new FeeReportComData();
                    FeeByRegionReportResponse.Data.reportType = "govArea";
                    FeeByRegionReportResponse.RecordsCount = FeeByRegionReportResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeByRegionReportResponse.RecordsCount / PageSize;
                        if (FeeByRegionReportResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeByRegionReportResponse.PagesCount = count;
                        FeeByRegionReportResponse.Data.DataDetail = FeeByRegionReportResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeByRegionReportResponse.PagesCount = 1;
                    }
                    return Ok(FeeByRegionReportResponse);
                case "lvl":
                    var FeeByLevelReportResponse = new BaseResponse<FeeReportComData>();
                    FeeByLevelReportResponse.Data = service.GetFeeByLevelReport(startDate, endDate);
                    if (FeeByLevelReportResponse.Data == null) FeeByLevelReportResponse.Data = new FeeReportComData();
                    FeeByLevelReportResponse.Data.reportType = "lvl";
                    FeeByLevelReportResponse.RecordsCount = FeeByLevelReportResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeByLevelReportResponse.RecordsCount / PageSize;
                        if (FeeByLevelReportResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeByLevelReportResponse.PagesCount = count;
                        FeeByLevelReportResponse.Data.DataDetail = FeeByLevelReportResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeByLevelReportResponse.PagesCount = 1;
                    }
                    return Ok(FeeByLevelReportResponse);
                case "feeDtl":
                    var FeeDetailDataResponse = new BaseResponse<FeeDetailData>();
                    FeeDetailDataResponse.ResultMessage = "feeDtl";
                    FeeDetailDataResponse.Data = service.GetFeeDetailReport(startDate, endDate);
                    FeeDetailDataResponse.RecordsCount = FeeDetailDataResponse.Data.DataDetail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeDetailDataResponse.RecordsCount / PageSize;
                        if (FeeDetailDataResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeDetailDataResponse.PagesCount = count;
                        FeeDetailDataResponse.Data.DataDetail = FeeDetailDataResponse.Data.DataDetail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeDetailDataResponse.PagesCount = 1;
                    }
                    return Ok(FeeDetailDataResponse);
                case "feeTreat":
                    var FeeTreatMentResponse = new BaseResponse<FeeByCareTypeMain>();
                    FeeTreatMentResponse.Data = service.GetFeeByCareTypeMainReport(startDate, endDate);
                    if (FeeTreatMentResponse.Data == null) FeeTreatMentResponse.Data = new FeeByCareTypeMain();
                    FeeTreatMentResponse.Data.reportType = "feeTreat";
                    FeeTreatMentResponse.RecordsCount = FeeTreatMentResponse.Data.Detail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeTreatMentResponse.RecordsCount / PageSize;
                        if (FeeTreatMentResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeTreatMentResponse.PagesCount = count;
                        FeeTreatMentResponse.Data.Detail = FeeTreatMentResponse.Data.Detail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeTreatMentResponse.PagesCount = 1;
                    }
                    return Ok(FeeTreatMentResponse);
                case "feeApproval":
                    var FeeApprovalResponse = new BaseResponse<FeeByCareTypeMain>();
                    FeeApprovalResponse.Data = service.GetFeeByCareTypeMainReport(startDate, endDate);
                    FeeApprovalResponse.Data.reportType = "feeApproval";
                    FeeApprovalResponse.RecordsCount = FeeApprovalResponse.Data.Detail.Count;
                    if (PageSize > 0)
                    {
                        var count = FeeApprovalResponse.RecordsCount / PageSize;
                        if (FeeApprovalResponse.RecordsCount % PageSize > 0)
                        {
                            count += 1;
                        }
                        FeeApprovalResponse.PagesCount = count;
                        FeeApprovalResponse.Data.Detail = FeeApprovalResponse.Data.Detail.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        FeeApprovalResponse.PagesCount = 1;
                    }
                    return Ok(FeeApprovalResponse);
                case "MonthFee":
                    BaseResponse<object> response = new BaseResponse<object>();
                    var sDate = startDate.Value.ToString("yyyy-MM");
                    var eDate = endDate.Value.ToString("yyyy-MM");
                    BaseRequest request = new BaseRequest() { CurrentPage = CurrentPage, PageSize = PageSize };
                    var resTa = ltcService.GetResMonthData(request, sDate, eDate, nsno) as BaseResponse<IList<TreatmentAccount>>;
                    response.PagesCount = resTa.PagesCount;
                    response.RecordsCount = resTa.RecordsCount;
                    List<PrintMonthFee> list = new List<PrintMonthFee>();
                    var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ipd?nsno=" + nsno);
                    var resIpd = result.Content.ReadAsAsync<List<Ipdregout>>().Result;
                     try
                    {
                        list = service.GetPrintData(resTa, resIpd);
                    }
                    catch
                    {
                        return Ok(response);
                    }
                    object obj = new
                    {
                        dataList = list,
                        tHospDay = list.Sum(s => s.HospDay),
                        tTotalAmount = list.Sum(s => s.TotalAmount),
                        tNCIPay = list.Sum(s => s.NCIPay)
                    };
                    response.Data = obj;
                    return Ok(response);
                default:
                    var ResultResponse = new BaseResponse();
                    ResultResponse.ResultCode = -1;
                    ResultResponse.ResultMessage = "请先选择报表名称！";
                    return Ok(ResultResponse);

            }



        }
    }
}
