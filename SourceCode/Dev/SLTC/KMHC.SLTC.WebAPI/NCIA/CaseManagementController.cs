using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using KMHC.SLTC.Business.Entity.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/CaseMgr")]
    public class CaseManagementController : BaseController
    {
        private readonly IApplicantService _applicantService = IOCContainer.Instance.Resolve<IApplicantService>();
        [Route("GetResidents"), HttpGet]
        public async Task<IHttpActionResult> GetResidents(string nsId)
        {
            var applist = _applicantService.GetHadCertApplicantsByNsId(nsId);
            if (applist == null || applist.Data == null || applist.Data.Count <= 0)
            {
                return null;
            }
            var idNoList = applist.Data.Select(m => m.Idno).ToList();
            var result = await HttpClientHelper.NciHttpClient.PostAsJsonAsync("/api/Ext/Res/GetResByIdNos", idNoList);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            string resultContent = await result.Content.ReadAsStringAsync();
            var resultData = JsonConvert.DeserializeObject<dynamic>(resultContent);
            if (resultData == null || resultData.Data == null)
            {
                return null;
            }
            var resList = ((JArray)resultData.Data).ToObject<List<dynamic>>().Select(m => new
            {
                FeeNo = m.FeeNo,
                Name = m.Name,
                Sex = m.Sex,
                CareType = m.CareType,
                IpdFlag = m.IpdFlag
            }).ToList();

            var response = JsonConvert.SerializeObject(resList);
            return Ok(response);
        }
        [Route("GetFee"), HttpGet]
        public async Task<object> GetFee(int feeNo, DateTime st, DateTime et, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ext/Fee/GetFeeRecords?st=" + st + "&et=" + et + "&feeNo=" + feeNo + "");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var response = result.Content.ReadAsAsync<BaseResponse<object>>().Result;
            var data = ((JArray)response.Data).ToObject<List<object>>(); ;
            data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
            response.Data = data;
            response.CurrentPage = CurrentPage;
            return response;
        }
        [Route("GetNsRec"), HttpGet]
        public async Task<object> GetNsRec(int feeNo, DateTime st, DateTime et, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ext/Nursing/GetNsRecords?st=" + st + "&et=" + et + "&feeNo=" + feeNo + "");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var response = result.Content.ReadAsAsync<BaseResponse<object>>().Result;
            var data = ((JArray)response.Data).ToObject<List<object>>(); ;
            data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
            response.Data = data;
            response.CurrentPage = CurrentPage;
            return response;
        }
        [Route("GetMeasureRec"), HttpGet]
        public async Task<object> GetMeasureRec(int feeNo, DateTime st, DateTime et, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ext/Nursing/GetMeasuredRecords?st=" + st + "&et=" + et + "&feeNo=" + feeNo + "");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var dataResponse = result.Content.ReadAsAsync<BaseResponse<object>>().Result;
            var data = ((JArray)dataResponse.Data).ToObject<List<dynamic>>(); ;

            var response = new BaseResponse<object>();

            var extendData = data.Select(m => new
            {
                FeeNo = m.FeeNo,
                MeasureDay = ((DateTime)m.MeasureTime).ToString("yyyy-MM-dd"),
                MeasureTime = m.MeasureTime,
                ItemName = ((string)m.ItemCode).Contains("S006") ? "血糖" : m.ItemName,
                Value = m.Value,
                Desc = m.Desc
            }).ToList();

            var groupData = extendData.GroupBy(m => m.MeasureDay).Select(x => new
            {
                MeasureDay = x.Key,
                BloodSugar = x.Where(s => s.ItemName == "血糖").ToList(),
                BP = x.Where(m => m.ItemName == "血压(收缩压)" || m.ItemName == "血压(舒张压)")
                    .GroupBy(g => g.MeasureTime)
                    .Select(b => new
                    {
                        MeasureTime = b.Key,
                        SBP = b.FirstOrDefault(s => s.ItemName == "血压(收缩压)"),
                        DBP = b.FirstOrDefault(s => s.ItemName == "血压(舒张压)")
                    }),
                BO = x.Where(s => s.ItemName == "血氧").ToList(),
                Breath = x.Where(s => s.ItemName == "呼吸").ToList(),
                Pulse = x.Where(s => s.ItemName == "脉搏").ToList(),
                Temp = x.Where(s => s.ItemName == "体温").ToList()
            }).ToList();

            response.RecordsCount = groupData.Count;
            groupData = groupData.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
            response.Data = groupData;
            return response;
        }

        [Route("GetEvlRec"), HttpGet]
        public async Task<object> GetEvlRec(int feeNo, DateTime st, DateTime et, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ext/Nursing/GetEvalRecords?st=" + st + "&et=" + et + "&feeNo=" + feeNo + "");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var response = result.Content.ReadAsAsync<BaseResponse<object>>().Result;
            var data = ((JArray)response.Data).ToObject<List<object>>(); ;
            data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
            response.Data = data;
            response.CurrentPage = CurrentPage;
            return response;
        }
        [Route("GetCplRec"), HttpGet]
        public async Task<object> GetCplRec(int feeNo, DateTime st, DateTime et, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/Ext/Nursing/GetCplRecords?st=" + st + "&et=" + et + "&feeNo=" + feeNo + "");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var response = result.Content.ReadAsAsync<BaseResponse<object>>().Result;
            var data = ((JArray)response.Data).ToObject<List<object>>(); ;
            data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
            response.Data = data;
            response.CurrentPage = CurrentPage;
            return response;
        }
        private int GetPagesCount(int pageSize, int total)
        {
            if (pageSize <= 0)
            {
                return 1;
            }
            var count = total / pageSize;
            if (total % pageSize > 0)
            {
                count += 1;
            }
            return count;
        }
    }
}
