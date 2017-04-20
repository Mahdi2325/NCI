using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Deduction")]
    public class DeductionController : BaseController
    {
        IMonFeeService service = IOCContainer.Instance.Resolve<IMonFeeService>();
        [Route(""), HttpGet]
        public async Task<object> Get(int nsId, string sDate, string eDate, int CurrentPage, int PageSize)
        {
            var http = HttpClientHelper.NciHttpClient;
            object resultContent = new object();
            try
            {
                var request = new DeductionFilter
                {

                    StartTime = sDate,
                    EndTime = eDate,
                    NsNo = service.QueryOrgNsHomeByID(nsId.ToString()) == null ? "-1" : service.QueryOrgNsHomeByID(nsId.ToString()).NSNo,
                    CurrentPage = CurrentPage,
                    PageSize = PageSize
                };

                var result = await http.PostAsJsonAsync("/api/DeductionInfo", request);
                resultContent = await result.Content.ReadAsAsync<object>();
            }
            catch (Exception ex)
            {
                resultContent = "-1";
            }
            return resultContent;
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(long NSMONFEEID, int CurrentPage, int PageSize)
        {
            var request = new BaseRequest()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
            };
            var response = service.GetDeductionList(request,NSMONFEEID);
            return Ok(response);
        }

        [Route("")]
        public async Task<object> Post(NCIDeductionModel request)
        {
            var http = HttpClientHelper.NciHttpClient;
            object resultContent = new object();
            try
            {
                if (request.ID == 0)
                {
                    request.CreateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    request.CreatTime = DateTime.Now;
                }
                else
                {
                    request.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    request.UpdateTime = DateTime.Now;
                }
                request.Debitmonth = request.Debitmonth;
                request.DeductionStatus = (int)DeductionStatus.UnCharge;
                request.DeductionType = (int)DeductionType.NCIOpr;
                var result = await http.PostAsJsonAsync("/api/DeductionInfo/saveDeduction", request);
                resultContent = await result.Content.ReadAsAsync<object>();
            }
            catch (Exception ex)
            {
                resultContent = "-1";
            }
            return resultContent;
        }
    }
}
