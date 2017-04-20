using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCI;
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
    public class AppcertService : BaseService, IAppcertService
    {
        /// <summary>
        /// 获取护理保险待遇申请信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<AppcertEntity>> QueryAppcertList(BaseRequest<AppcertEntityFilter> request)
        {
            BaseResponse<IList<AppcertEntity>> response = new BaseResponse<IList<AppcertEntity>>();
            Mapper.CreateMap<NCIA_APPCERT, AppcertEntity>();
            var q = from m in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                    join o in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on m.APPLICANTID equals o.APPLICANTID into oApplicant
                    join n in unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().dbSet on m.NSASSTRECORDID equals n.NSASSTRECORDID into nrd
                    from applicant in oApplicant.DefaultIfEmpty()
                    from nr in nrd.DefaultIfEmpty()
                    select new
                    {
                        appcert = m,
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

                        adlTotalScore = nr.TOTALSCORE,

                    };
            q = q.Where(m => m.appcert.ISDELETE == false && m.appcert.NSID == SecurityHelper.CurrentPrincipal.OrgId);

            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.appcert.NAME.Contains(request.Data.Name));
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.appcert.IDNO.ToUpper().Contains(request.Data.IdNo.ToUpper()) || m.appcert.SSNO.ToUpper().Trim().Contains(request.Data.SsNo.ToUpper().Trim()));
            }
            if (request.Data.Status != -1)
            {
                q = q.Where(m => m.appcert.STATUS == request.Data.Status);
            }
            q = q.OrderByDescending(m => m.appcert.CREATETIME);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<AppcertEntity>();
                foreach (dynamic item in list)
                {
                    AppcertEntity newItem = Mapper.DynamicMap<AppcertEntity>(item.appcert);
                    newItem.Name = item.Name;
                    newItem.IdNo = item.IdNo;
                    newItem.SsNo = item.SsNo;
                    newItem.Gender = item.Gender;
                    newItem.Address = item.Address;
                    newItem.Phone = item.Phone;
                    newItem.Age = item.Age;
                    newItem.TotalScore = item.adlTotalScore;
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
        }
        /// <summary>
        /// 获取单个护理保险待遇申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> QueryAppcert(int id)
        {
            var response = base.Get<NCIA_APPCERT, AppcertEntity>((q) => q.APPCERTID == id);
            if (response.Data != null)
            {
                var applicant = base.Get<NCIA_APPLICANT, ApplicantEntity>((q) => q.APPLICANTID == response.Data.ApplicantId).Data;
                if (applicant != null)
                {
                    response.Data.Name = applicant.Name;
                    response.Data.IdNo = applicant.Idno;
                    response.Data.SsNo = applicant.Ssno;
                    response.Data.Gender = applicant.Gender;
                    response.Data.Age = DateTime.Now.Year - applicant.BirthDate.Year + 1;
                    response.Data.Address = applicant.Address;
                    response.Data.Phone = applicant.Phone;
                }
            }
            return response;
        }
        /// <summary>
        /// 根据社会保障号或身份证号带出基本信息
        /// </summary>
        /// <param name="keyNo">社会保障号或身份证号</param>
        /// <returns></returns>
        public BaseResponse<ApplicantEntity> QueryApplicant(string keyNo)
        {
            var response = new BaseResponse<ApplicantEntity>();
            var q = from a in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet
                    select new ApplicantEntity
                    {
                        ApplicantId = a.APPLICANTID,
                        Name = a.NAME,
                        Gender = a.GENDER,
                        Idno = a.IDNO,
                        Ssno = a.SSNO,
                        BirthDate = a.BIRTHDATE,
                        Age = DateTime.Now.Year - a.BIRTHDATE.Year + 1,
                        McType = a.MCTYPE,
                        Disease = a.DISEASE,
                        Address = a.ADDRESS,
                        Phone = a.PHONE,
                        Isdelete = a.ISDELETE,
                    };

            q = q.Where(m => m.Isdelete == false);
            if (!string.IsNullOrEmpty(keyNo))
            {
                q = q.Where(m => m.Idno.ToUpper().Trim() == keyNo.Trim().ToUpper() || m.Ssno.ToUpper().Trim() == keyNo.ToUpper().Trim());
                response.Data = q.FirstOrDefault();
            }
            return response;
        }


        public BaseResponse<AppcertLTCEntity> QueryLTCAppcertInfo(string idno, string type)
        {
            var response = new BaseResponse<AppcertLTCEntity>();
            var year = DateTime.Now.Year;
            var q = from m in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                    join a in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on m.APPLICANTID equals a.APPLICANTID into act
                    join h in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(o => o.ISDELETE != true && o.AGENCYRESULT == (int)ApplicationEnum.ExaminationPassed) on m.APPCERTID equals h.APPCERTID into h_rr
                    from hr in h_rr.DefaultIfEmpty()
                    join n in unitOfWork.GetRepository<NCI_CARETYPE>().dbSet on hr.CARETYPE equals n.CARETYPEID into nrd
                    from nr in nrd.DefaultIfEmpty()
                    from ac in act.DefaultIfEmpty()
                    select new AppcertLTCEntity
                    {
                        AppcertId = m.APPCERTID,
                        BaseAppcertIdOfReApp = m.BASEAPPCERTIDOFREAPP,
                        AgencyAsstRecordId = m.AGENCYASSTRECORDID,
                        NsAsstRecordId = m.NSASSTRECORDID,
                        NsId = m.NSID,
                        appHospNsId = hr.NSID,
                        AppcertSN = m.APPCERTSN,

                        Name = ac.NAME,
                        Gender = ac.GENDER,
                        Age = DateTime.Now.Year - ac.BIRTHDATE.Year + 1,
                        IdNo = ac.IDNO,
                        SsNo = ac.SSNO,

                        NsappcareType = m.NSAPPCARETYPE,
                        McType = m.MCTYPE,
                        Disease = m.DISEASE,

                        Address = ac.ADDRESS,
                        Phone = ac.PHONE,

                        AgencyapprovedcareType = hr.CARETYPE,
                        AppReason = m.APPREASON,
                        Status = m.STATUS,
                        UploadFiles = m.UPLOADFILES,
                        NsComment = m.NSCOMMENT,
                        NsOperateTime = m.NSOPERATETIME,
                        IcResult = m.ICRESULT,
                        IcComment = m.ICCOMMENT,
                        IcOperateTime = m.ICOPERATETIME,
                        AgencyResult = m.AGENCYRESULT,
                        AgencyComment = m.AGENCYCOMMENT,
                        AgencyOperateTime = m.AGENCYOPERATETIME,
                        CertNo = m.CERTNO,
                        CertStartTime = m.CERTSTARTTIME,
                        CertExpiredTime = m.CERTEXPIREDTIME,
                        CreateBy = m.CREATEBY,
                        CreateTime = m.CREATETIME,
                        UpdateBy = m.UPDATEBY,
                        UpdateTime = m.UPDATETIME,
                        IsDelete = m.ISDELETE,
                        CaretypeName = nr.CARETYPENAME,
                        NCIPayLevel = nr.NCIPAYLEVEL,
                        NCIPayScale = nr.NCIPAYSCALE,
                        InHospDate = hr.ENTRYTIME.Value,
                        AppHospCreatetime = hr.CREATETIME,
                        Residence = ac.RESIDENCE
                    };
            q = q.Where(m => m.IsDelete != true && m.AppHospCreatetime != null && m.CaretypeName != null && m.Status == (int)ApplicationEnum.ExaminationPassed);
            if (!string.IsNullOrEmpty(idno) && !string.IsNullOrEmpty(type))
            {
                q = q.Where(m => m.IdNo.ToUpper() == idno.ToUpper().Trim());

                Mapper.CreateMap<NCI_NURSINGHOME, NursingHome>();
                var nurhome = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(m => m.NSNO == type && m.ISDELETE != true);
                var orgingo = Mapper.Map<NursingHome>(nurhome);

                q = q.Where(m => m.appHospNsId == orgingo.NsId);
                try
                {
                    var appList = q.OrderByDescending(m => m.InHospDate).ToList();
                    if (appList != null && appList.Count > 0)
                    {
                        response.Data = appList.OrderByDescending(m => m.AppHospCreatetime).FirstOrDefault();
                        String[] dislist = response.Data.Disease.Split(',');
                        var codelist = unitOfWork.GetRepository<NCI_CODEDTL>().dbSet.Where(o => o.ITEMTYPE == "A003").ToList();
                        foreach (var code in dislist)
                        {
                            response.Data.DiseaseTxt += codelist.Where(m => m.ITEMCODE == code).FirstOrDefault().ITEMNAME + "  ";
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "当前住民资格证书有误，请联系相关管理员核实！";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }





        /// <summary>
        /// 保存护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> SaveAppcert(AppcertEntity request)
        {
            lock (this)
            {
                var response = new BaseResponse<AppcertEntity>();
                var otherNsAppcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.OrderByDescending(m => m.CREATETIME).FirstOrDefault(m => m.IDNO.ToUpper() == request.IdNo.ToUpper() && m.ISDELETE == false && m.NSID != SecurityHelper.CurrentPrincipal.OrgId && m.STATUS != 11 && m.APPCERTID != request.AppcertId);
                if (otherNsAppcert != null)
                {

                    var msg = ((EnumAppCertStatus)otherNsAppcert.STATUS).ToString();
                    response.ResultCode = -1;
                    if (otherNsAppcert.STATUS == 6)
                    {
                        response.ResultMessage = "在其他机构已存在" + msg + "的申请表,可以直接去申请住院";
                    }
                    else
                    {
                        response.ResultMessage = "在其他机构已存在" + msg + "的申请表";
                    }
                    return response;
                }
                var appcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.OrderByDescending(m => m.CREATETIME).FirstOrDefault(m => m.IDNO.ToUpper() == request.IdNo.ToUpper() && m.ISDELETE == false && m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.STATUS != 11 && m.APPCERTID != request.AppcertId);
                if (appcert != null)
                {
                    var currentStatus = appcert.STATUS;
                    //var nursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO.ToUpper() == request.IdNo.ToUpper() && m.ISDELETE == false && m.NSID == SecurityHelper.CurrentPrincipal.OrgId);
                    var now = DateTime.Now;
                    //DateTime lastCertdate = nursingHomeAsstRecord.LASTCERTDATE ?? DateTime.Now;
                    //var lastCertResult = nursingHomeAsstRecord.LASTCERTRESULT;

                    //if (nursingHomeAsstRecord != null)
                    //{
                        DateTime AuditAppcertCertdate = (appcert.UPDATETIME ?? DateTime.Now).AddMonths(6);
                        //2:check 申请过的不能再申请 提交过的不能再提交
                        //if (lastCertResult == 0 || lastCertResult == 9)
                        //{
                            if (currentStatus == 0 || currentStatus == 1)
                            {
                                response.ResultCode = -1;
                                response.ResultMessage = "已存在未提交的申请表";
                            }
                            else if (currentStatus == 3)
                            {
                                response.ResultCode = -1;
                                response.ResultMessage = "已存在未审核的申请表";
                            }
                            else if (currentStatus == 6)
                            {
                                response.ResultCode = -1;
                                response.ResultMessage = "申请通过的不能做第二次申请";
                            }
                            else if (currentStatus == 9)//3:check 申请不通过的半年之内不能做第二次申请
                            {
                                if (DateTime.Compare(AuditAppcertCertdate, now) > 0)
                                {
                                    response.ResultCode = -1;
                                    response.ResultMessage = "复审不通过的在半年之内不可以再次申请";
                                }
                            }
                        }

                        //4:check 申请通过的不能做第二次申请
                //        if (lastCertResult == 6)
                //        {
                //            response.ResultCode = -1;
                //            response.ResultMessage = "申请通过的不能做第二次申请";
                //        }

                //    }
                //}

                if (response.ResultCode != -1)
                {
                    if (request.AppcertId == 0)
                    {
                        request.IsDelete = false;
                        request.NsId = SecurityHelper.CurrentPrincipal.OrgId;
                        //资格申请流水号：
                        request.AppcertSN = GenerateCode("AppcertSN", EnumCodeKey.AppcertSN);
                        request.CreateBy = SecurityHelper.CurrentPrincipal.UserId.ToString(); ;
                        request.CreateTime = DateTime.Now;
                    }
                    request.NsOperateTime = DateTime.Now;
                    request.NsUserId = SecurityHelper.CurrentPrincipal.UserId.ToString(); ;
                    request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString(); ;
                    request.UpdateTime = DateTime.Now;
                    response.Data = base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId).Data;
                }
                return response;
            }
        }
        /// <summary>
        /// 提交护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> SubmitAppcert(AppcertEntity request)
        {
            lock (this)
            {
                var response = new BaseResponse<AppcertEntity>();


                //1:check a. 是否做过ADL评估  b. ADL总分是否小于60分
                var NursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().dbSet.FirstOrDefault(m => m.NSASSTRECORDID == request.NsAsstRecordId);


                if (NursingHomeAsstRecord == null)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "未做日常生活能力评定量表";
                }
                else
                {
                    var adlTotalScore = NursingHomeAsstRecord.TOTALSCORE;
                    if (request.TotalScore != adlTotalScore)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "请先保存日常生活能力评定量表数据";
                    }
                    else if (adlTotalScore >= 60)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "日常生活能力评定量表总分不能大于或等于60分";
                    }
                }

                //其他check放到保存里

                if (response.ResultCode != -1)
                {
                    request.Status = 3;
                    request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString(); ;
                    request.UpdateTime = DateTime.Now;
                    response.Data = base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId).Data;
                }
                return response;
            }
        }
        /// <summary>
        /// 护理保险待遇重新申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> SaveReAppcert(AppcertEntity request)
        {
            request.Status = 11;
            request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString(); ;
            request.UpdateTime = DateTime.Now;
            return base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId);
        }
        /// <summary>
        /// 撤销护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> CancelSubmitAppcert(AppcertEntity request)
        {
            request.Status = 1;
            request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
            request.UpdateTime = DateTime.Now;
            return base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId);
        }
        /// <summary>
        /// 删除护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AppcertEntity> DeleteAppcert(AppcertEntity request)
        {
            //删除重审表单 特殊处理
            if (request.BaseAppcertIdOfReApp != null)
            {
                var Appcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.FirstOrDefault(m => m.APPCERTID == request.BaseAppcertIdOfReApp);
                if (Appcert != null)
                {
                    Appcert.STATUS = 9;
                    unitOfWork.GetRepository<NCIA_APPCERT>().Update(Appcert);
                    unitOfWork.Save();
                }
            }
            request.IsDelete = true;
            request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
            request.UpdateTime = DateTime.Now;
            return base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId);
        }
        /// <summary>
        /// 保存ADL评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<NursingHomeAsstRecordData> SaveAdlRec(NursingHomeAsstRecordData request)
        {

            Mapper.Reset();
            Mapper.CreateMap<NursingHomeAsstRecord, NCIA_NURSINGHOMEASSTRECORD>();
            Mapper.CreateMap<NCIA_NURSINGHOMEASSTRECORD, NursingHomeAsstRecord>();
            var model = unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().dbSet.FirstOrDefault(m => m.NSASSTRECORDID == request.NursingHomeAsstRecord.NsAsstRecordId);
            if (model == null)
            {
                model = Mapper.Map<NCIA_NURSINGHOMEASSTRECORD>(request.NursingHomeAsstRecord);
                model.ISDELETE = false;
                model.CREATETIME = DateTime.Now;
                model.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.UPDATETIME = DateTime.Now;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.ASSTDATE = DateTime.Now;
                unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().Insert(model);
            }
            else
            {
                Mapper.Map(request.NursingHomeAsstRecord, model);
                model.ISDELETE = false;
                model.UPDATETIME = DateTime.Now;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.ASSTDATE = DateTime.Now;
                unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().Update(model);
            }
            unitOfWork.Save();
            Mapper.Map(model, request.NursingHomeAsstRecord);
            if (request.NursingHomeAsstRecordDetail != null && request.NursingHomeAsstRecordDetail.Count > 0)
            {
                foreach (var item in request.NursingHomeAsstRecordDetail)
                {
                    Mapper.Reset();
                    Mapper.CreateMap<NursingHomeAsstRecordDetail, NCIA_NURSINGHOMEASSTRECORDDETAIL>();
                    Mapper.CreateMap<NCIA_NURSINGHOMEASSTRECORDDETAIL, NursingHomeAsstRecordDetail>();
                    var subModel = unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORDDETAIL>().dbSet.FirstOrDefault(m => m.NSASSTRECORDDETAILID == item.NsAsstRecordDetailId);
                    if (subModel == null)
                    {
                        subModel = Mapper.Map<NCIA_NURSINGHOMEASSTRECORDDETAIL>(item);
                        subModel.NSASSTRECORDID = request.NursingHomeAsstRecord.NsAsstRecordId;
                        unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORDDETAIL>().Insert(subModel);
                    }
                    else
                    {
                        Mapper.Map(item, subModel);
                        subModel.NSASSTRECORDID = request.NursingHomeAsstRecord.NsAsstRecordId;
                        unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORDDETAIL>().Update(subModel);
                    }
                    unitOfWork.Save();
                    Mapper.Map(subModel, item);
                }
            }

            return new BaseResponse<NursingHomeAsstRecordData>()
            {
                Data = request
            };

        }
        /// <summary>
        /// 获取评估表结果数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<NursingHomeAsstRecordData> GetAdlRec(BaseRequest<NursingHomeAsstRecordDataFilter> request)
        {
            Mapper.CreateMap<NCIA_NURSINGHOMEASSTRECORD, NursingHomeAsstRecord>();

            BaseResponse<NursingHomeAsstRecordData> response = new BaseResponse<NursingHomeAsstRecordData>() { Data = new NursingHomeAsstRecordData() };

            var findItem = unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORD>().dbSet.FirstOrDefault(q => q.APPCERTID == request.Data.AppcertId && q.ISDELETE == false);
            if (findItem != null)
            {
                response.Data.NursingHomeAsstRecord = Mapper.Map<NursingHomeAsstRecord>(findItem);
                Mapper.CreateMap<NCIA_NURSINGHOMEASSTRECORDDETAIL, NursingHomeAsstRecordDetail>();
                var findItemDetail = unitOfWork.GetRepository<NCIA_NURSINGHOMEASSTRECORDDETAIL>().dbSet.Where(q => q.NSASSTRECORDID == findItem.NSASSTRECORDID).ToList();
                if (findItemDetail != null)
                {
                    response.Data.NursingHomeAsstRecordDetail = Mapper.Map<IList<NursingHomeAsstRecordDetail>>(findItemDetail);
                }
            }
            return response;
        }

        #region ADL数据来源
        /// <summary>
        /// 查询评估问题和答案显示数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public BaseResponse<EC_Question> GetQue(string code)
        {
            var ecQuestion = new BaseResponse<EC_Question>() { };
            ecQuestion = base.Get<LTC_QUESTION, EC_Question>((q) => q.CODE == code);
            ecQuestion.Data.MakerItem = QueryMakerItemList(new BaseRequest<MakerItemFilter>() { PageSize = 0, Data = new MakerItemFilter() { QuestionId = ecQuestion.Data.QuestionId } }).Data;
            ecQuestion.Data.QuestionResults = QueryQuestionResultsList(new BaseRequest<QuestionResultsFilter>() { PageSize = 0, Data = new QuestionResultsFilter() { QuestionId = ecQuestion.Data.QuestionId } }).Data;
            return ecQuestion;
        }
        /// <summary>
        /// 查询问题列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<EC_MakerItem>> QueryMakerItemList(BaseRequest<MakerItemFilter> request)
        {
            var response = base.Query<LTC_MAKERITEM, EC_MakerItem>(request, (q) =>
            {
                q = q.Where(m => m.QUESTIONID == request.Data.QuestionId);
                q = q.OrderBy(m => m.SHOWNUMBER);
                return q;
            });
            foreach (var item in response.Data)
            {
                item.MakerItemLimitedValue = base.Query<LTC_MAKERITEMLIMITEDVALUE, EC_MakerItemLimitedValue>(request, (q) =>
                {
                    q = q.Where(m => m.LIMITEDID == item.LimitedId);
                    return q;
                }).Data;
            }
            return response;
        }
        /// <summary>
        /// 查询评估结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<EC_QuestionResults>> QueryQuestionResultsList(BaseRequest<QuestionResultsFilter> request)
        {
            var response = base.Query<LTC_QUESTIONRESULTS, EC_QuestionResults>(request, (q) =>
            {
                q = q.Where(m => m.QUESTIONID == request.Data.QuestionId);
                return q;
            });

            return response;
        }
        /// <summary>
        /// 获取当前机构的护理形式
        /// </summary>
        /// <returns></returns>
        public BaseResponse<IList<CareTypeEntity>> QueryCareTypeList()
        {

            var response = new BaseResponse<IList<CareTypeEntity>>();
            Mapper.CreateMap<NCI_CARETYPE, CareTypeEntity>();
            var nursingHomeModel = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId);
            List<NCI_CARETYPE> careTypeList = nursingHomeModel.SelectMany(m => m.NCI_CARETYPE).OrderByDescending(m => m.CARETYPE).ToList();
            careTypeList = careTypeList.GroupBy(x => new
            {
                x.CARETYPE,
                x.DISPLAYNAME
            }).Select(g => g.First()).ToList();
            response.Data = Mapper.Map<IList<CareTypeEntity>>(careTypeList);
            return response;
        }
        #endregion
    }
}
