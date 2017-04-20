using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/HlxDesk")]
    public class HlxDeskController : BaseController
    {
        IHlxDeskService service = IOCContainer.Instance.Resolve<IHlxDeskService>();
        [Route("GetHeadMsg"), HttpGet]
        public async Task<IHttpActionResult> GetHeadMsg()
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/residentV2/GetNursingHomeAndLeaveRes");
            var res = result.Content.ReadAsAsync<dynamic>().Result;
            var response = service.GetHeadMsg(res);
            return Ok(response);
        }
        [Route("GetAppcertStatistics"), HttpGet]
        public IHttpActionResult GetAppcertStatistics()
        {
            var response = service.GetAppcertStatistics();
            return Ok(response);
        }
        [Route("GetDeclareState"), HttpGet]
        public IHttpActionResult GetDeclareState()
        {
            var response = service.GetDeclareState();
            return Ok(response);
        }
        [Route("GetRequireAppItem"), HttpGet]
        public IHttpActionResult GetRequireAppItem()
        {
            var response = service.GetRequireAppItem();
            return Ok(response);
        }
        [Route("GetHlxSbaoHeadMsg"), HttpGet]
        public IHttpActionResult GetHlxSbaoHeadMsg()
        {
            var response = service.GetHlxSbaoHeadMsg();
            return Ok(response);
        }
        [Route("GetHlxSbaoAppcertStatistics"), HttpGet]
        public IHttpActionResult GetHlxSbaoAppcertStatistics()
        {
            var response = service.GetHlxSbaoAppcertStatistics();
            return Ok(response);
        }
    }
}
