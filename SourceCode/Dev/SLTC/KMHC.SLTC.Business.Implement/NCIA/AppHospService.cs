using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIA
{
    public class AppHospService : BaseService, IAppHospService
    {

        IAuditAppcertService service = IOCContainer.Instance.Resolve<IAuditAppcertService>();
        #region 查询住院申请列表数据
        /// <summary>
        /// 查询住院申请列表数据
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回信息</returns>
        public BaseResponse<IList<AppHospEntity>> QueryAppHospList(BaseRequest<AppHospEntityFilter> request)
        {
            BaseResponse<IList<AppHospEntity>> response = new BaseResponse<IList<AppHospEntity>>();
            Mapper.CreateMap<NCIA_APPHOSP, AppHospEntity>();
            var q = from m in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet
                    join o in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on m.APPLICANTID equals o.APPLICANTID into oApplicant
                    from applicant in oApplicant.DefaultIfEmpty()
                    select new
                    {
                        appHosp = m,
                        //姓名
                        Name = applicant.NAME,
                        //身份证号
                        IdNo = applicant.IDNO,
                        //社保卡号
                        SsNo = applicant.SSNO,
                        //性别
                        Gender = applicant.GENDER,
                        //年龄
                        Age = DateTime.Now.Year - applicant.BIRTHDATE.Year + 1,
                        //现住址
                        Address = applicant.ADDRESS,
                        //联系电话
                        Phone = applicant.PHONE,

                    };
            q = q.Where(m => m.appHosp.ISDELETE == false);

            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.appHosp.NAME.Contains(request.Data.Name));
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.appHosp.IDNO.ToUpper().Trim().Contains(request.Data.IdNo.ToUpper().Trim()));
            }
            if (request.Data.IcResult != -1)
            {
                q = q.Where(m => m.appHosp.AGENCYRESULT == request.Data.IcResult);
            }

            q = q.Where(m => m.appHosp.NSID == SecurityHelper.CurrentPrincipal.OrgId);

            q = q.OrderByDescending(m => m.appHosp.CREATETIME);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<AppHospEntity>();
                foreach (dynamic item in list)
                {
                    AppHospEntity newItem = Mapper.DynamicMap<AppHospEntity>(item.appHosp);
                    newItem.Name = item.Name;
                    newItem.IdNO = item.IdNo;
                    newItem.SsNO = item.SsNo;
                    newItem.Gender = item.Gender;
                    newItem.Address = item.Address;
                    newItem.Phone = item.Phone;
                    newItem.Age = item.Age;
                    response.Data.Add(newItem);
                }
            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;


            //var response = base.Query<NCIA_APPHOSP, AppHospEntity>(request, (q) =>
            // {
            //     q = q.Where(m => m.ISDELETE == false);

            //     if (!string.IsNullOrEmpty(request.Data.Name))
            //     {
            //         q = q.Where(m => m.NAME.Contains(request.Data.Name));
            //     }
            //     if (!string.IsNullOrEmpty(request.Data.IdNo))
            //     {
            //         q = q.Where(m => m.IDNO.ToUpper().Trim().Contains(request.Data.IdNo.ToUpper().Trim()));
            //     }
            //     if (request.Data.IcResult != -1)
            //     {
            //         q = q.Where(m => m.AGENCYRESULT == request.Data.IcResult);
            //     }

            //     q = q.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId);

            //     q = q.OrderByDescending(m => m.CREATETIME);
            //     return q;
            // });
            //return response;
        }
        #endregion

        #region 查询入院申请记录

        /// <summary>
        /// 查询参保人详情信息
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>入院申请记录信息</returns>
        public BaseResponse<AppHospEntity> QueryAppShopInfo(int appHospid)
        {
            BaseResponse<AppHospEntity> response = new BaseResponse<AppHospEntity>();
            try
            {
                response = base.Get<NCIA_APPHOSP, AppHospEntity>((q) => q.APPHOSPID == appHospid);
                if (response.Data != null)
                {
                    var applicant = base.Get<NCIA_APPLICANT, ApplicantEntity>((q) => q.APPLICANTID == response.Data.ApplicantId).Data;
                    if (applicant != null)
                    {
                        response.Data.Name = applicant.Name;
                        response.Data.IdNO = applicant.Idno;
                        response.Data.SsNO = applicant.Ssno;
                        response.Data.Gender = applicant.Gender;
                        response.Data.Age = DateTime.Now.Year - applicant.BirthDate.Year + 1;
                        response.Data.Address = applicant.Address;
                        response.Data.Phone = applicant.Phone;
                    }
                }
            }
            catch (Exception exception)
            {
                response.ResultMessage = exception.Message;
                throw;
            }
            return response;
        }
        #endregion

        #region 根据身份证号码或医保卡号查询资格信息
        /// <summary>
        /// 根据身份证号码或医保卡号查询资格信息
        /// </summary>
        /// <param name="keyNo">社保卡号或者身份证号码</param>
        /// <returns>资格证书信息</returns>
        public BaseResponse<AppCertBaseInfo> QueryAppcertInfo(string keyNo)
        {
            var response = new BaseResponse<AppCertBaseInfo>();
            var q = from a in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                    join n in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on a.APPLICANTID equals n.APPLICANTID into nrd
                    from nr in nrd.DefaultIfEmpty()
                    select new AppCertBaseInfo
                    {
                        ApplicantId = nr.APPLICANTID,
                        Appcertid = a.APPCERTID,
                        Name = nr.NAME,
                        Gender = nr.GENDER,
                        Age = DateTime.Now.Year - nr.BIRTHDATE.Year + 1,
                        SsNO = nr.SSNO,
                        IdNO = nr.IDNO,
                        Mctype = a.MCTYPE,
                        CareType = a.AGENCYAPPROVEDCARETYPE,
                        Disease = a.DISEASE,
                        NsID = nr.NSID,
                        Phone = nr.PHONE,
                        FamilyMemberphone = nr.FAMILYMEMBERPHONE,
                        Address = nr.ADDRESS,
                        Status = a.STATUS,
                        CertNo = a.CERTNO,
                        CertStartTime = a.CERTSTARTTIME,
                        CertExpiredTime = a.CERTEXPIREDTIME,
                        FamilyMemberName = nr.FAMILYMEMBERNAME,
                        Isdelete = a.ISDELETE,
                    };

            q = q.Where(m => m.Isdelete == false && m.Status == 6);
            if (!string.IsNullOrEmpty(keyNo))
            {
                q = q.Where(m => m.IdNO.ToUpper().Trim() == keyNo.ToUpper().Trim() || m.SsNO.ToUpper().Trim() == keyNo.ToUpper().Trim());
            }
            var careTypeInfo = service.QueryCareTypeList(SecurityHelper.CurrentPrincipal.OrgId);

            //var time = DateTime.Now;
            //q = q.Where(m => m.CertExpiredTime.HasValue && m.CertExpiredTime >= time);

            response.Data = q.FirstOrDefault();
            if (response.Data != null)
            {
                var careTypeList = careTypeInfo.Data.Where(m => m.CareType == response.Data.CareType.Value).ToList();
                if (careTypeList != null && careTypeList.Count != 1)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "当前机构对应的护理形式信息配置错误，请联系管理人员!";
                    return response;
                }
                else
                {
                    response.Data.CareType = careTypeList[0].CareTypeID;
                }
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "当前机构未查询到相对应的资格护理险信息!";
                return response;
            }

            return response;
        }
        #endregion

        #region 获取机构名称

        public string QueryNursingHomeName(string Nsid)
        {
            var apphosp = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(m => m.NSID == Nsid);
            return apphosp.NSNAME != null ? apphosp.NSNAME : "";
        }
        #endregion

        #region 保存申请住院信息
        public BaseResponse<AppHospEntity> SaveAppHosp(AppHospEntity request)
        {
            BaseResponse<AppHospEntity> response = new BaseResponse<AppHospEntity>();
            var now = DateTime.Now;
            try
            {
                var ishasHospData = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.OrderByDescending(m => m.CREATETIME).FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE != true && m.APPHOSPID != request.AppHospid);
                if (ishasHospData != null)
                {
                    var currentStatus = ishasHospData.AGENCYRESULT;
                    if (currentStatus == 0 || currentStatus == 1)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在未提交的申请表,请联系相关人员进行核实！";
                    }

                    else if (currentStatus == 3)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在未审核的申请表,请联系相关人员进行核实！";
                    }
                    else if (currentStatus == 6)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在申请通过的入院申请数据,无需做第二次申请！";
                    }
                }
                if (response.ResultCode != -1)
                {
                    if (request.AppHospid != 0)
                    {
                        request.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.UpdateTime = now;
                        return base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                    }
                    // 新增
                    else
                    {
                        request.AppHospsn = GenerateCode("AppHospSN", EnumCodeKey.AppHospSN);
                        request.NsID = SecurityHelper.CurrentPrincipal.OrgId;
                        request.NsoperateTime = now;
                        request.NsstaffName = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.AgencyResult = 0;
                        request.Createby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.CreateTime = now;
                        request.IsDelete = false;
                        return base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                    }

                    var appcant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE == false);
                    if (appcant != null)
                    {
                        appcant.LASTHOSPRESULT = 0;
                        unitOfWork.GetRepository<NCIA_APPLICANT>().Update(appcant);
                        unitOfWork.Save();
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultMessage = ex.Message;
                throw;
            }
            return response;
        }
        #endregion

        #region 逻辑删除入院申请数据
        /// <summary>
        /// 逻辑删除入院申请数据 
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>删除数据</returns>
        public BaseResponse<AppHospEntity> ChangeAppHosp(AppHospEntity request)
        {
            var response = new BaseResponse<AppHospEntity>();
            var deleteModel = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.APPHOSPID == request.AppHospid).FirstOrDefault();

            if (deleteModel != null)
            {
                deleteModel.ISDELETE = true;
                unitOfWork.GetRepository<NCIA_APPHOSP>().Update(deleteModel);
                var appcant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE == false);
                if (appcant != null)
                {
                    appcant.LASTHOSPRESULT = 0;
                    unitOfWork.GetRepository<NCIA_APPLICANT>().Update(appcant);
                }
                unitOfWork.Save();
                Mapper.DynamicMap(deleteModel, response.Data);
            }
            return response;
        }

        /// <summary>
        /// 撤回操作
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns>
        public BaseResponse<AppHospEntity> ChangeAgencyResult(AppHospEntity request)
        {
            BaseResponse<AppHospEntity> response = new BaseResponse<AppHospEntity>();
            //撤回操作
            if (request.ActionStatus == 1)
            {
                request.AgencyResult = 1;
                response = base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                var appcant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE == false);
                if (appcant != null)
                {
                    appcant.LASTHOSPRESULT = 1;
                    unitOfWork.GetRepository<NCIA_APPLICANT>().Update(appcant);
                    unitOfWork.Save();
                }
            }
            //  列表提交操作
            else if (request.ActionStatus == 3)
            {
                request.AgencyResult = 3;
                response = base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                var appcant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE == false);
                if (appcant != null)
                {
                    appcant.LASTHOSPRESULT = 3;
                    unitOfWork.GetRepository<NCIA_APPLICANT>().Update(appcant);
                    unitOfWork.Save();
                }

            }
            return response;
        }
        #endregion

        #region 提交操作
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回参数</returns>
        public BaseResponse<AppHospEntity> submitAppHosp(AppHospEntity request)
        {
            BaseResponse<AppHospEntity> response = new BaseResponse<AppHospEntity>();
            var now = DateTime.Now;
            try
            {
                var ishasHospData = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.OrderByDescending(m => m.CREATETIME).FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE != true && m.APPHOSPID != request.AppHospid);
                if (ishasHospData != null)
                {
                    var currentStatus = ishasHospData.AGENCYRESULT;
                    if (currentStatus == 0 || currentStatus == 1)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在未提交的申请表,请联系相关人员进行核实！";
                    }
                    else if (currentStatus == 3)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在未审核的申请表,请联系相关人员进行核实！";
                    }
                    else if (currentStatus == 6)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = QueryNursingHomeName(ishasHospData.NSID) + "已存在申请通过的入院申请数据,无需做第二次申请！";
                    }
                }

                if (response.ResultCode != -1)
                {
                    if (request.AppHospid != 0)
                    {
                        request.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.UpdateTime = now;
                        request.AgencyResult = 3;
                        return base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                    }
                    // 新增
                    else
                    {
                        request.AppHospsn = GenerateCode("AppHospSN", EnumCodeKey.AppHospSN);
                        request.NsID = SecurityHelper.CurrentPrincipal.OrgId;
                        request.NsoperateTime = now;
                        request.NsstaffName = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.AgencyResult = 3;
                        request.Createby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                        request.CreateTime = now;
                        request.IsDelete = false;
                        return base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid);
                    }
                    var appcant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.APPLICANTID == request.ApplicantId && m.ISDELETE == false);
                    if (appcant != null)
                    {
                        appcant.LASTHOSPRESULT = 3;
                        unitOfWork.GetRepository<NCIA_APPLICANT>().Update(appcant);
                        unitOfWork.Save();
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultMessage = ex.Message;
                throw;
            }
            return response;
        }
        #endregion
    }
}
