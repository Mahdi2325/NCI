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
    public class AuditAppcertService : BaseService, IAuditAppcertService
    {

        #region 查询定点服务机构数据
        /// <summary>
        /// 根据政府机构码查询定点服务机构数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NursingHome>> QueryOrgNsHome(BaseRequest<AgencyORGFilter> request)
        {
            var response = base.Query<NCI_NURSINGHOME, NursingHome>(request, (q) =>
            {
                if (request != null && !string.IsNullOrEmpty(request.Data.Govid))
                {
                    q = q.Where(m => m.GOVID == request.Data.Govid);
                }
                q = q.OrderBy(m => m.NSID);
                return q;
            });
            return response;
        }

        #endregion

        #region 查询需要审核的数据集合
        /// <summary>
        ///查询审核列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        public BaseResponse<IList<AppcertEntity>> QueryAppcertList(BaseRequest<AuditAppcretEntityFilter> request)
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
            q = q.Where(m => m.appcert.ISDELETE == false);

            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.appcert.NAME.Contains(request.Data.Name));
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.appcert.IDNO.Contains(request.Data.IdNo) || m.appcert.SSNO.Contains(request.Data.SsNo));
            }
            if (request.Data.Status != -1)
            {
                q = q.Where(m => m.appcert.STATUS == request.Data.Status);
            }
            else
            {
                q = q.Where(m => m.appcert.STATUS == 3 || m.appcert.STATUS == 6 || m.appcert.STATUS == 9);
            }
            if (Convert.ToInt32(request.Data.Nsid) != -1)
            {
                q = q.Where(m => m.appcert.NSID.ToUpper().Trim() == request.Data.Nsid.ToUpper().Trim());
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
            //var nursingHomeModel = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId);
            //List<NCI_CARETYPE> careTypeList = nursingHomeModel.SelectMany(m => m.NCI_CARETYPE).OrderByDescending(m => m.CARETYPE).ToList();
            //foreach (var item in response.Data)
            //{
            //    var careTypeModel=careTypeList.Where(m=>m.CARETYPEID==item.NsappcareType).FirstOrDefault();
            //    if(careTypeModel!=null)
            //    {
            //        item.AgencyapprovedcareType = careTypeModel.CARETYPEID;
            //    }
            //    else
            //    {
            //        if (careTypeList.Count > 0) item.AgencyapprovedcareType = careTypeList[0].CARETYPEID;
            //    }
            //}
            return response;
        }

        #endregion
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
                var nsInfo = base.Get<NCI_NURSINGHOME, NCI_NursingHome>((q) => q.NSID == response.Data.NsId).Data;
                if(nsInfo!=null)
                {
                    response.Data.NsName = nsInfo.NsName;
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
            var response = new BaseResponse<AppcertEntity>();
            if (request.ActionStatus != 0)
            {
                //1:check a. 是否做过ADL评估  b. ADL总分是否小于60分
                var NursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_AGENCYASSTRECORD>().dbSet.FirstOrDefault(m => m.AGENCYASSTRECORDID == request.AgencyAsstRecordId);
                if (NursingHomeAsstRecord == null)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "未做日常生活能力评定量表";
                }
                else
                {
                    var adlTotalScore = NursingHomeAsstRecord.TOTALSCORE;
                    if (adlTotalScore >= 60)
                    {
                        if (request.ActionStatus != 9)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "日常生活能力评定量表总分不能大于或等于60分";
                        }

                    }
                }
            }

            if (response.ResultCode != -1)
            {

                //if (request.ActionStatus == 6 || request.ActionStatus == 9)
                //{
                //    var Applicant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO == request.IdNo && m.ISDELETE == false);
                //    if (Applicant != null)
                //    {
                //        if (request.ActionStatus == 6)
                //        {
                //            Applicant.LASTCERTRESULT = 6;
                //            Applicant.LASTCERTDATE = Convert.ToDateTime(DateTime.Now.Year.ToString() + "/12/31");
                //        }
                //        if (request.ActionStatus == 9)
                //        {
                //            Applicant.LASTCERTRESULT = 9;
                //        }
                //        unitOfWork.GetRepository<NCIA_APPLICANT>().Update(Applicant);
                //        unitOfWork.Save();
                //    }
                //}
                #region 资格申请数据
                if (request.ActionStatus == 6)
                {
                    request.CertNo = GenerateCode("CertNo", EnumCodeKey.CertNo);
                    request.CertStartTime = DateTime.Now;
                    request.CertExpiredTime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "/12/31");
                }
                #endregion
                request.AgencyOperateTime = DateTime.Now;
                request.AgencyUserId = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.AgencyId = SecurityHelper.CurrentPrincipal.OrgId;
                request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.UpdateTime = DateTime.Now;
                response.Data = base.Save<NCIA_APPCERT, AppcertEntity>(request, (q) => q.APPCERTID == request.AppcertId).Data;
            }
            return response;
        }
        /// <summary>
        /// 保存ADL评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AgencyAsstRecordData> SaveAdlRec(AgencyAsstRecordData request)
        {

            Mapper.Reset();
            Mapper.CreateMap<AgencyAsstRecord, NCIA_AGENCYASSTRECORD>();
            Mapper.CreateMap<NCIA_AGENCYASSTRECORD, AgencyAsstRecord>();
            var model = unitOfWork.GetRepository<NCIA_AGENCYASSTRECORD>().dbSet.FirstOrDefault(m => m.AGENCYASSTRECORDID == request.AgencyAsstRecord.AgencyAsstRecordId);
            if (model == null)
            {
                model = Mapper.Map<NCIA_AGENCYASSTRECORD>(request.AgencyAsstRecord);
                model.ISDELETE = false;
                model.CREATETIME = DateTime.Now;
                model.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.UPDATETIME = DateTime.Now;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.ASSTDATE = DateTime.Now;
                unitOfWork.GetRepository<NCIA_AGENCYASSTRECORD>().Insert(model);
            }
            else
            {
                Mapper.Map(request.AgencyAsstRecord, model);
                model.ISDELETE = false;
                model.UPDATETIME = DateTime.Now;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                model.ASSTDATE = DateTime.Now;
                unitOfWork.GetRepository<NCIA_AGENCYASSTRECORD>().Update(model);
            }
            unitOfWork.Save();
            Mapper.Map(model, request.AgencyAsstRecord);
            if (request.AgencyAsstRecordDetail != null && request.AgencyAsstRecordDetail.Count > 0)
            {
                foreach (var item in request.AgencyAsstRecordDetail)
                {
                    Mapper.Reset();
                    Mapper.CreateMap<AgencyAsstRecordDetail, NCIA_AGENCYASSTRECORDDETAIL>();
                    Mapper.CreateMap<NCIA_AGENCYASSTRECORDDETAIL, AgencyAsstRecordDetail>();
                    var subModel = unitOfWork.GetRepository<NCIA_AGENCYASSTRECORDDETAIL>().dbSet.FirstOrDefault(m => m.AGENCYASSTRECORDDETAILID == item.AgencyAsstRecordDetailId);
                    if (subModel == null)
                    {
                        subModel = Mapper.Map<NCIA_AGENCYASSTRECORDDETAIL>(item);
                        subModel.AGENCYASSTRECORDID = request.AgencyAsstRecord.AgencyAsstRecordId;
                        unitOfWork.GetRepository<NCIA_AGENCYASSTRECORDDETAIL>().Insert(subModel);
                    }
                    else
                    {
                        Mapper.Map(item, subModel);
                        subModel.AGENCYASSTRECORDID = request.AgencyAsstRecord.AgencyAsstRecordId;
                        unitOfWork.GetRepository<NCIA_AGENCYASSTRECORDDETAIL>().Update(subModel);
                    }
                    unitOfWork.Save();
                    Mapper.Map(subModel, item);
                }
            }

            return new BaseResponse<AgencyAsstRecordData>()
            {
                Data = request
            };

        }
        /// <summary>
        /// 获取评估表结果数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AgencyAsstRecordData> GetAdlRec(BaseRequest<AgencyAsstRecordDataFilter> request)
        {
            Mapper.CreateMap<NCIA_AGENCYASSTRECORD, AgencyAsstRecord>();

            BaseResponse<AgencyAsstRecordData> response = new BaseResponse<AgencyAsstRecordData>() { Data = new AgencyAsstRecordData() };

            var findItem = unitOfWork.GetRepository<NCIA_AGENCYASSTRECORD>().dbSet.FirstOrDefault(q => q.APPCERTID == request.Data.AppcertId && q.ISDELETE == false);
            if (findItem != null)
            {
                response.Data.AgencyAsstRecord = Mapper.Map<AgencyAsstRecord>(findItem);
                Mapper.CreateMap<NCIA_AGENCYASSTRECORDDETAIL, AgencyAsstRecordDetail>();
                var findItemDetail = unitOfWork.GetRepository<NCIA_AGENCYASSTRECORDDETAIL>().dbSet.Where(q => q.AGENCYASSTRECORDID == findItem.AGENCYASSTRECORDID).ToList();
                if (findItemDetail != null)
                {
                    response.Data.AgencyAsstRecordDetail = Mapper.Map<IList<AgencyAsstRecordDetail>>(findItemDetail);
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
        #endregion


        /// <summary>
        /// 获取定点机构的护理形式
        /// </summary>
        /// <returns></returns>
        public BaseResponse<IList<CareTypeEntity>> QueryCareTypeList(string nsId)
        {
            var response = new BaseResponse<IList<CareTypeEntity>>();
            Mapper.CreateMap<NCI_CARETYPE, CareTypeEntity>();
            var nursingHomeModel = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(m => m.NSID == nsId);
            List<NCI_CARETYPE> careTypeList = nursingHomeModel.SelectMany(m => m.NCI_CARETYPE).OrderByDescending(m => m.CARETYPE).ToList();
            response.Data = Mapper.Map<IList<CareTypeEntity>>(careTypeList);
            return response;
        }
    }
}
