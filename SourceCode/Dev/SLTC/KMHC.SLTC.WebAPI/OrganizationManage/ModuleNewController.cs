#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :
** 说明     :
******************************************************************/
#endregion

using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using KMHC.Infrastructure;


namespace KMHC.SLTC.WebAPI.OrganizationManage
{
    [RoutePrefix("api/ModuleNew")]
    public class ModuleNewController : BaseController
    {
        readonly IOrganizationManageService _service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route("{roleId}")]
        public IHttpActionResult Get(string roleId, string type = "", bool loadTreeByRole = false)
        {

            if (type == "")
            {
                RoleFilter request = new RoleFilter
                {
                    RoleId = string.IsNullOrWhiteSpace(roleId) ? null : roleId
                };
                var response = _service.GetRoleModule(request);
                return Ok(response);
            }
            else if (type == "tree")
            {
                BaseRequest<RoleFilter> requestByRole = new BaseRequest<RoleFilter>
                {
                    Data =
                    {
                        RoleId = string.IsNullOrWhiteSpace(roleId) ? null : roleId,
                        OrgId = SecurityHelper.CurrentPrincipal.OrgId
                    }
                };

                BaseRequest<RoleFilter> requestByTree = new BaseRequest<RoleFilter>();
                if (loadTreeByRole)
                {
                    requestByTree.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    requestByTree.Data.RoleId = SecurityHelper.CurrentPrincipal.RoleId;
                    requestByTree.Data.RoleType = SecurityHelper.CurrentPrincipal.RoleType;
                }
                var response = _service.GetModuleByRole(requestByRole, requestByTree);
                return Ok(response);
            }
            else
            {
                return Ok("type參數不正確");
            }
        }
    }
}
