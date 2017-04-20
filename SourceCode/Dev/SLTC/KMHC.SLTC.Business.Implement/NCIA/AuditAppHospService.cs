using AutoMapper;
using KMHC.Infrastructure;
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
    public class AuditAppHospService : BaseService, IAuditAppHospService
    {
        #region 查询需要审核的数据集合
        /// <summary>
        ///查询审核列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        public BaseResponse<IList<AppHospEntity>> QueryAppHospList(BaseRequest<AuditAppHospEntityFilter> request)
        {
            //var response = base.Query<NCIA_APPHOSP, AppHospEntity>(request, (q) =>
            //{
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
                q = q.Where(m => m.appHosp.IDNO.ToUpper().Trim().Contains(request.Data.IdNo.ToUpper().Trim()) || m.appHosp.SSNO.ToUpper().Trim().Contains(request.Data.SsNo.ToUpper().Trim()));
            }
            if (request.Data.Status != -1)
            {
                q = q.Where(m => m.appHosp.AGENCYRESULT == request.Data.Status);
            }
            else
            {
                q = q.Where(m => m.appHosp.AGENCYRESULT == 3 || m.appHosp.AGENCYRESULT == 6 || m.appHosp.AGENCYRESULT == 9);
            }

            if (Convert.ToInt32(request.Data.Nsid) != -1)
            {
                q = q.Where(m => m.appHosp.NSID.ToUpper().Trim() == request.Data.Nsid.ToUpper().Trim());
            }

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

        }

        #endregion

        #region 入院申请审核
        /// <summary>
        /// 入院审核
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns>
        public BaseResponse<AppHospEntity> AuditAppHosp(AppHospEntity request)
        {
            var response = new BaseResponse<AppHospEntity>();
            var now = DateTime.Now;
            var apphosp = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.OrderByDescending(m => m.CREATETIME).FirstOrDefault(m => m.IDNO.ToUpper().Trim() == request.IdNO.ToUpper().Trim() && m.ISDELETE == false && m.NSID.ToUpper().Trim() == request.NsID.ToUpper().Trim() && m.APPHOSPID != request.AppHospid);
            if (apphosp != null)
            {
                var currentStatus = apphosp.AGENCYRESULT;
                var nursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO.ToUpper().Trim() == request.IdNO.ToUpper().Trim() && m.ISDELETE == false);
                var lastHospdate = nursingHomeAsstRecord.LASTHOSPDATE;
                var lastHospResult = nursingHomeAsstRecord.LASTHOSPRESULT;

                if (nursingHomeAsstRecord != null)
                {
                    //3:check 申请通过的半年之内不能做第二次申请
                    if (lastHospResult == 6)
                    {
                        if (lastHospdate >= now)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "已存在有效的入院审核记录信息！";
                        }
                    }
                }
            }
            if (response.ResultCode != -1)
            {
                #region 改变基础数据 信息状态
                var Applicant = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO.ToUpper().Trim() == request.IdNO.ToUpper().Trim() && m.ISDELETE == false);
                if (Applicant != null)
                {
                    if (request.ActionStatus == 6)
                    {
                        Applicant.LASTHOSPRESULT = 6;
                        Applicant.LASTHOSPDATE = Convert.ToDateTime(DateTime.Now.Year.ToString() + "/12/31 23:59:59");
                    }
                    if (request.ActionStatus == 9)
                    {
                        Applicant.LASTHOSPRESULT = 9;
                    }
                    unitOfWork.GetRepository<NCIA_APPLICANT>().Update(Applicant);
                    unitOfWork.Save();
                }
                #endregion

                #region 改变申请的数据的状态
                request.AgencyResult = request.ActionStatus;
                request.AgencyoperateTime = DateTime.Now;
                request.AgencystafFName = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.UpdateTime = DateTime.Now;
                response.Data = base.Save<NCIA_APPHOSP, AppHospEntity>(request, (q) => q.APPHOSPID == request.AppHospid).Data;
                #endregion
            }
            return response;
        }
        #endregion
    }
}
