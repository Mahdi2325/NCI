using KM.Common;
using KMHC.SLTC.Business.Interface.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/OrgStatistics")]
    public class OrgStatisticsController : BaseController
    {
        IOrgStatistics service = IOCContainer.Instance.Resolve<IOrgStatistics>();
        [Route("GetAppcertRate"), HttpGet]
        public IHttpActionResult GetAppcertRate(string starttime, string endtime)
        {
            var response = service.GetAppcertRate(starttime,endtime);
            return Ok(response);
        }
        [Route("GetApphospRate"), HttpGet]
        public IHttpActionResult GetApphospRate(string starttime, string endtime)
        {
            var response = service.GetApphospRate(starttime, endtime);
            return Ok(response);
        }
        [Route("GetcertRate"), HttpGet]
        public IHttpActionResult GetcertRate(string NSID = "1")
        {
            var response = service.GetcertRate(NSID);
            return Ok(response);
        }
        [Route("GethospRate"), HttpGet]
        public IHttpActionResult GethospRate(string NSID = "1")
        {
            var response = service.GethospRate(NSID);
            return Ok(response);
        }
        [Route("PutAppcertRate"), HttpGet]
        public IHttpActionResult PutAppcertRate(string NSID, int? YEAR = null)
        {
            var response = service.PutAppcertRate(NSID,YEAR);
            return Ok(response);
        }
        [Route("PutApphospRate"), HttpGet]
        public IHttpActionResult PutApphospRate(string NSID, int? YEAR = null)
        {
            var response = service.PutApphospRate(NSID, YEAR);
            return Ok(response);
        }
        [Route("GetAppcert"), HttpGet]
        public IHttpActionResult GetAppcert(string NSID, int YEAR )
        {
            var response = service.GetAppcert(NSID, YEAR);
            return Ok(response);
        }
        [Route("GetApphosp"), HttpGet]
        public IHttpActionResult GetApphosp(string NSID, int YEAR)
        {
            var response = service.GetApphosp(NSID, YEAR);
            return Ok(response);
        }
        [Route("GetMonthFeeStatistics"), HttpGet]
        public IHttpActionResult GetMonthFeeStatistics(string sDate, string eDate, string nsId="")
        {
            var response = service.GetMonthFeeStatistics(sDate,eDate,nsId);
            return Ok(response);
        }
    }
}
