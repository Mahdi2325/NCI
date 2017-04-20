using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/users")]
    public class UsersController : BaseController
    {
        IOrganizationManageService usersService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string orgid="",int currentPage=1, int pageSize=10,string fingtype="")
        {
            BaseRequest<NCI_UserFilter> request = new BaseRequest<NCI_UserFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Account = keyWord, UserName = keyWord, OrgId = orgid, OrgType = SecurityHelper.CurrentPrincipal.OrgType }
            };
            //有传入值就按传入的值来查，没有就按默认当前机构
            if (string.IsNullOrEmpty(orgid))
            {
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            if (!SecurityHelper.CurrentPrincipal.IsAdmin)
            {
                request.Data.ParentUserId = SecurityHelper.CurrentPrincipal.UserId;
            }
       
            var response = usersService.QueryUser(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string userName, string account, string orgid ,int? userId)
        {
            BaseRequest<NCI_UserFilter> request = new BaseRequest<NCI_UserFilter>
            {
                Data = { Account = account, UserName = userName, OrgId = orgid, UserId = userId }
            };

            if (string.IsNullOrEmpty(orgid))
            {
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            var response = usersService.QueryUser(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Get(string account)
        {
            var response = usersService.QueryUserByAccount(account);
            return Ok(response.Data);
        }

        // GET api/syteminfo/5
        [Route("{UserId}")]
        public IHttpActionResult Get(int UserId)
        {
            var response = usersService.GetUser(UserId);
            return Ok(response.Data);
        }


        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(NCI_User user)
        {
            if (user.UserId==0)
            {
                if (user.OrgId==null)
                {
                    user.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    user.OrgType = SecurityHelper.CurrentPrincipal.OrgType;
                }
                user.ParentUserId = SecurityHelper.CurrentPrincipal.UserId;
                user.CreateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                user.CreateTime = DateTime.Now;
            }
            else {
                user.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                user.UpdateTime = DateTime.Now;
            }
            var response = usersService.SaveUser(user);
            return Ok(response);
        }
        [Route("ChangePassWord")]
        public IHttpActionResult ChangePassWord([FromBody] dynamic user)
        {
            var oldPass = user.oldPwd.Value;
            var newPass = user.newPwd.Value;
            var response = usersService.ChangePassWord(SecurityHelper.CurrentPrincipal.UserId, oldPass, newPass);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{UserId}")]
        public IHttpActionResult Delete(int UserId)
        {
            var response = usersService.DeleteUser(UserId);
            return Ok(response);
        }
    }
}