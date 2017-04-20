using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.LTC
{
    [RoutePrefix("api/MonthFee")]
    public class MonthFeeController : BaseController
    {
        ILtcService service = IOCContainer.Instance.Resolve<ILtcService>();
        [Route("")]
        public void Post(MonthFeeModel model)
        {
            service.SaveData(model);
        }
        [Route("")]
        public IHttpActionResult Get(string date, string nsno)
        {
            return Ok(service.CancelData(date, nsno));

        }
        [Route("GetOrgMonthDataList"), HttpGet]
        public IHttpActionResult GetOrgMonthDataList(string beginTime, string endTime, string nsno)
        {
            var response = service.GetOrgMonthDataList(beginTime, endTime, nsno);
            return Ok(response);
        }
        [Route("GetOrgMonthData"), HttpGet]
        public IHttpActionResult GetOrgMonthData(long NSMonFeeID)
        {
            var response = service.GetOrgMonthData(NSMonFeeID);
            return Ok(response);
        }
        [Route("GetResMonthData"), HttpGet]
        public IHttpActionResult GetResMonthData(long NSMonFeeID, int currentPage, int pageSize)
        {
            BaseRequest<MonthFeeFilter> request = new BaseRequest<MonthFeeFilter>() { CurrentPage = currentPage, PageSize = pageSize, Data = { NSMonFeeID = NSMonFeeID } };
            var response = service.GetResMonthData(request);
            return Ok(response);
        }
        [Route(""),HttpGet]
        public IHttpActionResult GetResMonthData(string sDate, string eDate, string nsno, int currentPage, int pageSize)
        {
            BaseRequest request = new BaseRequest() { CurrentPage = currentPage, PageSize = pageSize };
            var response = service.GetResMonthData(request, sDate, eDate, nsno);
            return Ok(response);
        }
    }
}
