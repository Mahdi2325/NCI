using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/appHosp")]
    public class AppHospController : BaseController
    {

        IAppHospService service = IOCContainer.Instance.Resolve<IAppHospService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string name, string idno, int status, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<AppHospEntity>>();
            var request = new BaseRequest<AppHospEntityFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new AppHospEntityFilter()
                {
                    Name = name,
                    IdNo = idno,
                    IcResult = status,

                }
            };
            response = service.QueryAppHospList(request);
            return Ok(response);
        }


        [Route(""), HttpGet]
        public IHttpActionResult Get(int appHospid)
        {
            var response = service.QueryAppShopInfo(appHospid);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string keyNo)
        {
            var response = service.QueryAppcertInfo(keyNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(AppHospEntity request)
        {
            var response = new BaseResponse<AppHospEntity>();
            //逻辑删除操作
            if (request.ActionStatus == -1)
            {
                response = service.ChangeAppHosp(request);
            }
            // 保存操作
            else if (request.ActionStatus == 0)
            {
               response = service.SaveAppHosp(request);
            }
            // 撤回操作
            else if (request.ActionStatus == 1)
            {
                response = service.ChangeAgencyResult(request);
            }
            else if (request.ActionStatus == 3)
            {
                 response = service.ChangeAgencyResult(request);
            }
            else if (request.ActionStatus == 5)
            {
                 response = service.submitAppHosp(request); 
            }
            return Ok(response);
        }

    }
}
