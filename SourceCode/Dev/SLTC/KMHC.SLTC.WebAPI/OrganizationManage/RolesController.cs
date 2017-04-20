using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/roles")]
    public class RolesController : BaseController
    {
        private IOrganizationManageService OrgService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(string roleName = "", string roleType = "", string orgid = "", int status = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<RoleFilter> request = new BaseRequest<RoleFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = 
                { 
                    RoleName = roleName
                    , OrgId = orgid
                    , RoleType = roleType
                    , Status = status
                    , OrgType = SecurityHelper.CurrentPrincipal.OrgType
                }
            };

            if (!SecurityHelper.CurrentPrincipal.IsAdmin)
            {
                request.Data.ParentroleId = SecurityHelper.CurrentPrincipal.RoleId;
            }

            if (string.IsNullOrEmpty(orgid))
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            var response = OrgService.QueryRole(request);
            return Ok(response);
        }


        // GET api/syteminfo/5
        [Route("{RoleId}")]
        public IHttpActionResult Get(string RoleId)
        {
            var response = OrgService.GetRole(RoleId);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Role baseRequest)
        {
            if (string.IsNullOrEmpty(baseRequest.RoleId))
            {
                //TODO id有重码可能,待改进
                baseRequest.RoleId = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                baseRequest.ParentRoleId = SecurityHelper.CurrentPrincipal.RoleId;
                baseRequest.OrgType = SecurityHelper.CurrentPrincipal.OrgType;
                baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            var response = OrgService.SaveRole(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{RoleId}")]
        public IHttpActionResult Delete(string RoleId)
        {
            var response = OrgService.DeleteRole(RoleId);
            return Ok(response);
        }
    }
}