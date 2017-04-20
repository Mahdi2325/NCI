using System;
using System.Collections.Generic;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System.Linq;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using System.Text;
using System.Net;
using System.IO;
using AutoMapper;

namespace KMHC.SLTC.Business.Implement.NCIA
{
    public class ApplicantService : BaseService, IApplicantService
    {
        #region 获取参保人信息

        /// <summary>
        /// 获取参保人信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>参保人信息</returns>
        public BaseResponse<IList<ApplicantEntity>> QueryApplicantList(BaseRequest<ApplicantFiletr> request)
        {
            BaseResponse<IList<ApplicantEntity>> response = new BaseResponse<IList<ApplicantEntity>>();

            try
            {
                var q = from a in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet
                        select new ApplicantEntity
                        {
                            ApplicantId = a.APPLICANTID,
                            NSID = a.NSID,
                            Idno = a.IDNO,
                            Name = a.NAME,
                            Gender = a.GENDER,
                            BirthDate = a.BIRTHDATE,
                            Age = DateTime.Now.Year - a.BIRTHDATE.Year + 1,
                            Ssno = a.SSNO,
                            Residence = a.RESIDENCE,
                            McType = a.MCTYPE,
                            Disease = a.DISEASE,
                            DiseaseDesc = a.DISEASEDESC,
                            Address = a.ADDRESS,
                            Phone = a.PHONE,
                            Occupation = a.OCCUPATION,
                            FamilyMemberName = a.FAMILYMEMBERNAME,
                            Familymemberphone = a.FAMILYMEMBERPHONE,
                            Nativeplace = a.NATIVEPLACE,
                            Maritalstatus = a.MARITALSTATUS,
                            Economystatus = a.ECONOMYSTATUS,
                            Livecondition = a.LIVECONDITION,
                            Housingproperty = a.HOUSINGPROPERTY,
                            Habithos = a.HABITHOS,
                            Zip = a.ZIP,
                            Familymemberrelationship = a.FAMILYMEMBERRELATIONSHIP,
                            Lastcertresult = a.LASTCERTRESULT,
                            Lastcertdate = a.LASTCERTDATE,
                            Lasthospresult = a.LASTHOSPRESULT,
                            Lasthospdate = a.LASTHOSPDATE,
                            Createby = a.CREATEBY,
                            Createtime = a.CREATETIME,
                            Updateby = a.UPDATEBY,
                            Updatetime = a.UPDATETIME,
                            Isdelete = a.ISDELETE,
                        };

                if (!string.IsNullOrEmpty(request.Data.Name))
                {
                    q = q.Where(p => p.Name.Contains(request.Data.Name));
                }

                if (!string.IsNullOrEmpty(request.Data.IdNo))
                {
                    q = q.Where(p => p.Idno.ToUpper().Trim().Contains(request.Data.IdNo.ToUpper().Trim()) || p.Ssno.ToUpper().Trim().Contains(request.Data.IdNo.ToUpper().Trim()));
                }

                q = q.Where(p => p.Isdelete == false);
                //如果是定点机构则加载
                if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.NursingHome)
                {
                    q = q.Where(p => p.NSID == SecurityHelper.CurrentPrincipal.OrgId);
                }
                else if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.Agency)
                {
                    if (!String.IsNullOrEmpty(request.Data.NsId))
                    {
                        q = q.Where(p => p.NSID == request.Data.NsId);
                    }

                }

                q = q.OrderByDescending(m => m.Createtime);
                response.RecordsCount = q.Count();
                if (request.PageSize > 0)
                {
                    response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = q.ToList();
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

        #region 修改参保人删除状态

        /// <summary>
        /// 修改参保人删除状态
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回实体</returns>
        public BaseResponse<ApplicantEntity> SaveApplicant(ApplicantEntity request)
        {
            BaseResponse<ApplicantEntity> response = new BaseResponse<ApplicantEntity>();
            try
            {
                //var appcertModel = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.APPLICANTID == request.ApplicantId && m.STATUS != (int)EnumAppCertStatus.未提交 && m.STATUS != (int)EnumAppCertStatus.已撤回 && m.ISDELETE != true).ToList();
                var appcertModel = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.APPLICANTID == request.ApplicantId  && m.ISDELETE != true).ToList();
                if (appcertModel != null && appcertModel.Count > 0)
                {
                    response.ResultCode = 1001; //  当前住民存在申请数据， 建议不要删除
                    return response;
                }
                else
                {
                    if (request.Lastcertresult == 0 && request.Lasthospresult == 0)
                    {
                        return base.Save<NCIA_APPLICANT, ApplicantEntity>(request, (q) => q.IDNO.ToUpper().Trim() == request.Idno.ToUpper().Trim());
                    }
                    else
                    {
                        response.ResultCode = 1001; //  当前住民存在申请数据， 建议不要删除
                        return response;
                    }
                }
            }
            catch (Exception exception)
            {
                response.ResultMessage = exception.Message;
                throw;
            }
        }

        /// <summary>
        /// 保存参保人数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>保存信息</returns>
        public BaseResponse<ApplicantEntity> SaveApplicantInfo(ApplicantEntity request)
        {
            BaseResponse<ApplicantEntity> response = new BaseResponse<ApplicantEntity>();
            try
            {
                if (string.IsNullOrEmpty(request.ApplicantId))
                {
                    #region 新增

                    var info = from a in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet
                               join nur in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on a.NSID equals nur.NSID 
                               select new ApplicantEntity()
                               {
                                   Idno = a.IDNO,
                                   Ssno = a.SSNO,
                                   NSID = a.NSID,
                                   Isdelete = a.ISDELETE,
                                   OrgName = nur.NSNAME
                               };
                    List<ApplicantEntity> List = null;
                    if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.Agency)
                    {
                        List = info.Where(m => (m.Idno.ToUpper().Trim() == request.Idno.ToUpper().Trim() || m.Ssno.ToUpper().Trim() == request.Ssno.ToUpper().Trim()) && m.Isdelete == false).ToList();
                    }
                    else if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.NursingHome)
                    {
                        List = info.Where(m => (m.Idno.ToUpper().Trim() == request.Idno.ToUpper().Trim() || m.Ssno.ToUpper().Trim() == request.Ssno.ToUpper().Trim())&& m.Isdelete == false).ToList();
                    }

                    if (List != null && List.Count > 0)  //存在重复数据
                    {
                        response.ResultCode = 1001;  //存在相同的数据
                        if (List[0].NSID == SecurityHelper.CurrentPrincipal.OrgId)
                        {
                            response.ResultMessage = "保存失败，当前机构存在相同的身份证号码或者社保卡号的参保人信息！";
                        }
                        else
                        {
                            response.ResultMessage = "保存失败，当前申请人信息已存在于" + List[0].OrgName + "，在护理险资格申请中可通过身份证或社保卡号获取申请人基本资料！";
                        }
                        return response;
                    }
                    else  // 不存在数据
                    {
                        request.ApplicantId = GenerateCode("ApplicId", EnumCodeKey.ApplicantId);
                        return base.Save<NCIA_APPLICANT, ApplicantEntity>(request, (q) => q.APPLICANTID == request.ApplicantId);
                    }
                    #endregion
                }
                else
                {
                    #region 修改
                    var info = from a in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet
                               join nur in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on a.NSID equals nur.NSID
                               select new ApplicantEntity()
                               {
                                   Idno = a.IDNO,
                                   Ssno = a.SSNO,
                                   NSID = a.NSID,
                                   Isdelete = a.ISDELETE,
                                   ApplicantId =  a.APPLICANTID,
                                   OrgName = nur.NSNAME
                               };
                    info = info.Where(m => m.ApplicantId != request.ApplicantId);
                    List<ApplicantEntity> List = null;
                    if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.Agency)
                    {
                        List = info.Where(m => (m.Idno.ToUpper().Trim() == request.Idno.ToUpper().Trim() || m.Ssno.ToUpper().Trim() == request.Ssno.ToUpper().Trim()) && m.Isdelete == false).ToList();
                    }
                    else if (SecurityHelper.CurrentPrincipal.OrgType == (int)OrgType.NursingHome)
                    {
                        List = info.Where(m => (m.Idno.ToUpper().Trim() == request.Idno.ToUpper().Trim() || m.Ssno.ToUpper().Trim() == request.Ssno.ToUpper().Trim()) && m.Isdelete == false).ToList();
                    }
                    if (List != null && List.Count > 0)  //存在重复数据
                    {
                        response.ResultCode = 1001;  //存在相同的数据
                        if (List[0].NSID == SecurityHelper.CurrentPrincipal.OrgId)
                        {
                            response.ResultMessage = "保存失败，当前机构存在相同的身份证号码或者社保卡号的参保人信息！";
                        }
                        else
                        {
                            response.ResultMessage = "保存失败，当前申请人信息已存在于" + List[0].OrgName + "，在护理险资格申请中可通过身份证或社保卡号获取申请人基本资料！";
                        }
                        return response;
                    }
                    else  // 不存在数据
                    {
                        Mapper.CreateMap<ApplicantEntity, NCIA_APPLICANT>();
                        var applicantModel = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.APPLICANTID == request.ApplicantId).FirstOrDefault();
                        if (applicantModel != null)
                        {
                            //前端js已经控制：编辑时必须数据有变动才可以保存，所以request.IsInsertHistory只会传True过来
                            if (request.IsInsertHistory)
                            {
                                Mapper.CreateMap<NCIA_APPLICANT, NCIA_APPLICANTHISTORY>();
                                var applicantHistoryModel = Mapper.Map<NCIA_APPLICANTHISTORY>(applicantModel);
                                applicantHistoryModel.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                                applicantHistoryModel.CREATETIME = DateTime.Now;
                                unitOfWork.GetRepository<NCIA_APPLICANTHISTORY>().Insert(applicantHistoryModel);
                            }
                            //如果IdNo有变动，同时更新定点机构精简版长照对应人员的的IdNo
                            if (request.IsIdNoChaged)
                            {
                                var regFileModle = unitOfWork.GetRepository<LTC_REGFILE>().dbSet.Where(m => m.IDNO == applicantModel.IDNO).FirstOrDefault();
                                if (regFileModle != null)
                                {
                                    regFileModle.IDNO = request.Idno;
                                    unitOfWork.GetRepository<LTC_REGFILE>().Update(regFileModle);
                                    var noticeModel = new LTC_NOTIFICATION()
                                    {
                                        SUBJECTS = regFileModle.NAME + "身份证号变更",
                                        CONTENTS = regFileModle.NAME + "的身份证号由 " + applicantModel.IDNO + " 变更为 " + request.Idno,
                                        CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString(),
                                        CREATEDATE = DateTime.Now,
                                        ORGID = regFileModle.ORGID,
                                    };
                                    unitOfWork.GetRepository<LTC_NOTIFICATION>().Insert(noticeModel);
                                }
                                //更新NCIP_RESIDENTMONFEE表的身份证号
                                var residentMonFeeList = unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet.Where(m => m.RESIDENTSSID == applicantModel.IDNO).ToList();
                                residentMonFeeList.ForEach(p =>
                                {
                                    p.RESIDENTSSID = request.Idno;
                                    unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().Update(p);
                                });
                                //更新LTC_MONTHLYPAYLIMIT表的身份证号
                                var monthLyPayLimitList = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.Where(m => m.RESIDENTSSID == applicantModel.IDNO).ToList();
                                monthLyPayLimitList.ForEach(p =>
                                {
                                    p.RESIDENTSSID = request.Idno;
                                    unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().Update(p);
                                });
                            }
                            applicantModel.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                            applicantModel.UPDATETIME = DateTime.Now;
                            Mapper.Map(request, applicantModel);
                            unitOfWork.GetRepository<NCIA_APPLICANT>().Update(applicantModel);
                        }
                        else
                        {
                            applicantModel = Mapper.Map<NCIA_APPLICANT>(request);
                            unitOfWork.GetRepository<NCIA_APPLICANT>().Insert(applicantModel);
                        }
                        unitOfWork.Save();
                        Mapper.DynamicMap(applicantModel, request);
                        response.Data = request;
                    }
                    #endregion
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

        #region 查询参保人详情信息

        /// <summary>
        /// 查询参保人详情信息
        /// </summary>
        /// <param name="idNo">身份证号码</param>
        /// <returns>参保人信息</returns>
        public BaseResponse<ApplicantEntity> QueryApplicantInfo(string id)
        {
            BaseResponse<ApplicantEntity> response = new BaseResponse<ApplicantEntity>();
            try
            {

                response = base.Get<NCIA_APPLICANT, ApplicantEntity>((q) => q.APPLICANTID == id);
                var appcertModel = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.APPLICANTID == response.Data.ApplicantId && m.STATUS != (int)EnumAppCertStatus.未提交 && m.STATUS != (int)EnumAppCertStatus.已撤回).FirstOrDefault();
                if (response.Data != null)
                {
                    if (appcertModel != null)
                    {
                        response.Data.IsApply = true;
                    }
                    else
                    {
                        response.Data.IsApply = false;
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

        public BaseResponse<IList<ApplicantEntity>> GetHadCertApplicantsByNsId(string nsId)
        {
            var queryApp =
                from c in
                unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.ISDELETE != true && m.CERTNO != null && m.NSID==nsId)
                select c.APPLICANTID;
            var applicants = queryApp.Distinct().ToList();

            var response = base.Query<NCIA_APPLICANT, ApplicantEntity>(null, (q) =>
            {
                q = q.Where(m => applicants.Contains(m.APPLICANTID));
                return q;
            });
            return response;
        }

        #endregion
    }
}
