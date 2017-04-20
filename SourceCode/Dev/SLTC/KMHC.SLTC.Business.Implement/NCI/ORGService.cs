using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Interface.NCI;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Filter;

namespace KMHC.SLTC.Business.Implement.NCI
{
    public class ORGService : BaseService, IORGService
    {
        /// <summary>
        /// 获取定点服务机构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<Agency>> QueryOrgAgency(BaseRequest<ORGFilter> request)
        {
            var response = base.Query<NCI_AGENCY, Agency>(request, (q) =>
            {
                if (request != null && !string.IsNullOrEmpty(request.Data.OrgName))
                {
                    q = q.Where(m => m.AGENCYNAME.Contains(request.Data.OrgName));
                }
                q = q.OrderBy(m => m.AGENCYID);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 获取经办机构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NursingHome>> QueryOrgNsHome(BaseRequest<ORGFilter> request)
        {
            var response = base.Query<NCI_NURSINGHOME, NursingHome>(request, (q) =>
            {
                if (request != null && !string.IsNullOrEmpty(request.Data.OrgName))
                {
                    q = q.Where(m => m.NSNAME.Contains(request.Data.OrgName));
                }
                q = q.OrderBy(m => m.NSID);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 获取用户基本信息 定点机构登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NCI_User>> QueryNsHomeUserExtend(BaseRequest<NCI_UserFilter> request)
        {
            BaseResponse<IList<NCI_User>> response = new BaseResponse<IList<NCI_User>>();


            var q = (from u in unitOfWork.GetRepository<NCI_USER>().dbSet
                     select new
                     {
                         UserId = u.USERID,
                         Account = u.ACCOUNT,
                         Pwd = u.PASSWORD,
                         Staff = u.NCI_NURSINGHOMESTAFF,
                         Role = u.NCI_ROLE,
                         RoleType = u.NCI_ROLE.ROLETYPE,
                         RealName = u.USERNAME,
                         RoleId = u.ROLEID,
                         Status = u.STATUS,
                         OrgId = u.ORGID,
                         OrgType = u.ORGTYPE
                     }).AsEnumerable().Select(o => new NCI_User
            {
                UserId = o.UserId,
                Account = o.Account,
                Password = o.Pwd,
                OrgId = o.OrgId,
                RoleType = o.Role.ROLETYPE,
                OrgType = o.Role.ORGTYPE,
                RoleId = o.RoleId,
                BelongToGovId = GetBelongToGovId(o.Role.ORGID, o.OrgType)
            });

            if (request != null && !string.IsNullOrEmpty(request.Data.Account))
            {
                q = q.Where(m => m.Account == request.Data.Account);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.Password))
            {
                q = q.Where(m => m.Password == request.Data.Password);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId ==request.Data.OrgId);
            }

            q = q.OrderByDescending(m => m.CreateTime);
            response.RecordsCount = q.Count();
            List<NCI_User> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            return response;
        }


        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NCI_User>> QueryAgencyUserExtend(BaseRequest<NCI_UserFilter> request)
        {
            BaseResponse<IList<NCI_User>> response = new BaseResponse<IList<NCI_User>>();


            var q = (from u in unitOfWork.GetRepository<NCI_USER>().dbSet
                     select new
                     {
                         UserId = u.USERID,
                         Account = u.ACCOUNT,
                         Pwd = u.PASSWORD,
                         Staff = u.NCI_AGENCYSTAFF,
                         Role = u.NCI_ROLE,
                         RoleType = u.NCI_ROLE.ROLETYPE,
                         RealName = u.USERNAME,
                         RoleId = u.ROLEID,
                         Status = u.STATUS,
                         OrgId = u.ORGID,
                         OrgType = u.ORGTYPE
                     }).AsEnumerable().Select(o => new NCI_User
            {
                UserId = o.UserId,
                Account = o.Account,
                Password = o.Pwd,
                OrgId = o.OrgId,
                RoleType = o.Role.ROLETYPE,
                OrgType = o.Role.ORGTYPE,
                RoleId = o.RoleId,
                BelongToGovId = GetBelongToGovId(o.Role.ORGID, o.OrgType)
            });

            if (request != null && !string.IsNullOrEmpty(request.Data.Account))
            {
                q = q.Where(m => m.Account == request.Data.Account);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.Password))
            {
                q = q.Where(m => m.Password == request.Data.Password);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }

            q = q.OrderByDescending(m => m.CreateTime);
            response.RecordsCount = q.Count();
            List<NCI_User> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            return response;
        }


        private string GetBelongToGovId(string orgId, int orgType)
        {
            if (string.IsNullOrWhiteSpace(orgId))
            {
                //TODO 应该记录异常,没个用户都应该属于一个GOV
                return null;
            }
            var enumOrgType = (OrgType)orgType;
            switch (enumOrgType)
            {
                case OrgType.NursingHome:
                    {
                        var org = GetHursingHome(orgId);
                        return null == org ? null : org.GovId;
                    }
                case OrgType.Agency:
                    {
                        var org = GetAgency(orgId);
                        return null == org ? null : org.GovId;
                    }
                case OrgType.InsuranceCompany:
                    {
                        var org = GetInsuranceCompany(orgId);
                        return null == org ? null : org.GovId;
                    }
                case OrgType.Government:
                    {
                        var org = GetGovernment(orgId);
                        return null == org ? null : org.ParentGovId;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        //TODO 创建并调用各自Service的GET方法
        private NursingHome GetHursingHome(string orgId)
        {
            var nsData = unitOfWork.GetRepository<NCI_NURSINGHOME>().Get(orgId);
            Mapper.CreateMap<NCI_NURSINGHOME, NursingHome>();
            return null == nsData ? null : Mapper.Map<NursingHome>(nsData);
        }
        private Agency GetAgency(string orgId)
        {
            var nsData = unitOfWork.GetRepository<NCI_AGENCY>().Get(orgId);
            Mapper.CreateMap<NCI_AGENCY, Agency>();
            return null == nsData ? null : Mapper.Map<Agency>(nsData);
        }
        private InsuranceCompany GetInsuranceCompany(string orgId)
        {
            var nsData = unitOfWork.GetRepository<NCI_INSURANCECOMPANY>().Get(orgId);
            Mapper.CreateMap<NCI_INSURANCECOMPANY, InsuranceCompany>();
            return null == nsData ? null : Mapper.Map<InsuranceCompany>(nsData);
        }
        private Government GetGovernment(string orgId)
        {
            var nsData = unitOfWork.GetRepository<NCI_GOVERNMENT>().Get(orgId);
            Mapper.CreateMap<NCI_GOVERNMENT, Government>();
            return null == nsData ? null : Mapper.Map<Government>(nsData);
        }
        public BaseResponse<IList<NCI_User>> QueryUser(BaseRequest<NCI_UserFilter> request)
        {
            var response = base.Query<NCI_USER, NCI_User>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.Account) && !string.IsNullOrEmpty(request.Data.UserName))
                {
                    q = q.Where(m => m.ACCOUNT.Contains(request.Data.Account) || m.USERNAME.Contains(request.Data.UserName));
                }
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderByDescending(m => m.CREATEBY);
                return q;
            });
            return response;
        }
        public BaseResponse<NCI_User> GetUser(int userId)
        {
            Mapper.CreateMap<NCI_USER, NCI_User>();
            BaseResponse<NCI_User> response = new BaseResponse<NCI_User>();
            var findItem = unitOfWork.GetRepository<NCI_USER>().dbSet.FirstOrDefault(o => o.USERID == userId);
            if (findItem != null)
            {
                response.Data = Mapper.Map<NCI_User>(findItem);
            }
            return response;
        }

        public BaseResponse<NCI_User> SaveUser(NCI_User request)
        {
            return base.Save<NCI_USER, NCI_User>(request, (q) => q.USERID == request.UserId);
        }

        public BaseResponse DeleteUser(int userID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                //登陸中的賬號本身不能刪除
                if (userID == SecurityHelper.CurrentPrincipal.UserIdInt)
                {
                    response.ResultCode = -2;
                    response.ResultMessage = "无法删除登录中的账号";
                    return response;
                }

                //查詢刪除用戶信息
                var findItem = unitOfWork.GetRepository<LTC_USERS>().dbSet.FirstOrDefault(o => o.USERID == userID);
                var roleType = "";
                if (findItem.LTC_ROLES.Count > 0)
                {
                    var _roleTypeC = SecurityHelper.CurrentPrincipal.RoleType;
                    //獲取多角色的最高權限
                    if (_roleTypeC.Count() > 0)
                    {
                        foreach (var item in _roleTypeC)
                        {
                            if (item.ToString() == "SuperAdmin")
                            {
                                roleType = "SuperAdmin";
                                break;
                            }
                            if (item.ToString() == "Admin")
                            {
                                roleType = "Admin";
                                break;
                            }
                            if (item.ToString() == "Normal")
                            {
                                roleType = "Normal";
                                break;
                            }
                        }
                        if (roleType == "Admin")
                        {
                            foreach (var item in findItem.LTC_ROLES)
                            {
                                if (item.ROLETYPE == "SuperAdmin")
                                {
                                    response.ResultCode = -1;
                                    response.ResultMessage = "你沒有权限刪除该用戶";
                                    return response;
                                }
                            }
                        }
                        else if (roleType == "Normal")
                        {
                            foreach (var item in findItem.LTC_ROLES)
                            {
                                if (item.ROLETYPE == "SuperAdmin")
                                {
                                    response.ResultCode = -1;
                                    response.ResultMessage = "你沒有权限刪除该用戶";
                                    return response;
                                }
                                else if (item.ROLETYPE == "Admin")
                                {
                                    response.ResultCode = -1;
                                    response.ResultMessage = "你沒有权限刪除该用戶";
                                    return response;
                                }
                            }
                        }
                    }
                }



                unitOfWork.BeginTransaction();
                string strSql = String.Format("delete from LTC_USERORG where USERID='{0}'", userID);
                unitOfWork.GetRepository<LTC_USERS>().ExecuteSqlCommand(strSql);

                strSql = String.Format("delete from LTC_USERROLES where USERID='{0}'", userID);
                unitOfWork.GetRepository<LTC_USERS>().ExecuteSqlCommand(strSql);

                unitOfWork.GetRepository<LTC_USERS>().Delete(userID);

                if (!String.IsNullOrEmpty(findItem.EMPNO))
                {
                    strSql = String.Format("delete from LTC_EMPFILE where EMPNO='{0}'", findItem.EMPNO);
                    unitOfWork.GetRepository<LTC_EMPFILE>().ExecuteSqlCommand(strSql);
                    //unitOfWork.GetRepository<LTC_EMPFILE>().Delete(findItem.EMPNO);
                }
                unitOfWork.Save();
                response.ResultCode = 1;
                response.ResultMessage = "刪除成功";
                return response;
            }
            catch (Exception ex)
            {
                response.ResultCode = 0;
                response.ResultMessage = "删除异常";
                return response;
                throw ex;
            }
        }

        public BaseResponse ResetPassword(string orgID, string logonName, string password)
        {
            BaseResponse response = new BaseResponse();
            var model = unitOfWork.GetRepository<LTC_USERS>().dbSet.FirstOrDefault(m => m.LTC_ORG.Any(it => it.ORGID == orgID) && m.LOGONNAME == logonName);
            if (model != null)
            {
                model.PWD = password;
                model.UPDATEDATE = DateTime.Now;
                unitOfWork.GetRepository<LTC_USERS>().Update(model);
                unitOfWork.Save();
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "查询不到账号";
            }
            return response;
        }
        public BaseResponse ChangePassWord(string orgID, string logonName, string oldPassword, string newPassword)
        {
            BaseResponse response = new BaseResponse();
            var model = unitOfWork.GetRepository<LTC_USERS>().dbSet.FirstOrDefault(m => m.LTC_ORG.Any(it => it.ORGID == orgID) && m.LOGONNAME == logonName);
            if (model != null)
            {
                if (model.PWD == oldPassword)
                {
                    model.PWD = newPassword;
                    model.UPDATEDATE = DateTime.Now;
                    unitOfWork.GetRepository<LTC_USERS>().Update(model);
                    unitOfWork.Save();
                }
                else
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "旧密码输入错误";
                }

            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "用户信息为空";
            }
            return response;
        }

    }
}
