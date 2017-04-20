using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class MonFeeService : BaseService, IMonFeeService
    {
        IPayGrantService service = IOCContainer.Instance.Resolve<IPayGrantService>();
        IResidentMonfeeService resService = IOCContainer.Instance.Resolve<IResidentMonfeeService>();

        /// <summary>
        /// 查询定点服务机构月度费用
        /// </summary>
        /// <param name="resquest"></param>
        /// <returns></returns>
        public BaseResponse<IList<MonFeeModel>> QueryMonFeeModel(BaseRequest<MonFeeFilter> request)
        {
            try
            {
                var response = base.Query<NCIP_NSMONFEE, MonFeeModel>(request, (q) =>
                {
                    q = q.Where(o => o.ISDELETE != true);
                    //remove By Duke
                    //if (!string.IsNullOrEmpty(request.Data.OrgID) && request.Data.OrgID == "-1")
                    //{
                    //    Mapper.CreateMap<NCI_NURSINGHOME, NursingHome>();
                    //    var model = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(m => m.GOVID == SecurityHelper.CurrentPrincipal.OrgId && m.ISDELETE != true);
                    //    var nursingHome = Mapper.Map<NursingHome>(model);
                    //    if (nursingHome != null)
                    //    {
                    //        q = q.Where(o => o.NSID == nursingHome.NsId);
                    //    }
                    //    else
                    //    {
                    //        q = q.Where(o => o.NSID == request.Data.OrgID);
                    //    }
                    //}
                    //else
                    //{
                    //    q = q.Where(o => o.NSID == request.Data.OrgID);
                    //}

                    if (request.Data.OrgID != "-1")
                    {
                        q = q.Where(o => o.NSID == request.Data.OrgID);
                    }
                    if (request.Data.StartTime != null)
                    {
                        q = q.Where(o => o.YEARMONTH.CompareTo(request.Data.StartTime) >= 0);
                    }
                    if (request.Data.EndTime != null)
                    {
                        q = q.Where(o => o.YEARMONTH.CompareTo(request.Data.EndTime) <= 0);
                    }
                    q = q.OrderByDescending(m => m.CREATETIME);
                    return q;
                });
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据机构id 查询机构信息
        /// </summary>
        /// <param name="nsId"></param>
        /// <returns></returns>
        public NursingHome QueryOrgNsHomeByID(string nsId)
        {
            var response = new NursingHome();
            Mapper.CreateMap<NCI_NURSINGHOME, NursingHome>();
            var model = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(m => m.NSID == nsId && m.ISDELETE != true);
            response = Mapper.Map<NursingHome>(model);
            return response;
        }

        /// <summary>
        /// 查询单笔数据信息
        /// </summary>
        /// <param name="monFeeID">定点机构月费用ID</param>
        /// <returns>集合</returns>
        public BaseResponse<IList<MonFeeModel>> QueryMonFeeDetial(string monFeeID)
        {
            var monFeelist = monFeeID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var response = base.Query<NCIP_NSMONFEE, MonFeeModel>(null, (q) =>
                {
                    q = q.Where(m => m.ISDELETE != true);
                    q = q.Where(m => monFeelist.Contains(m.NSMONFEEID));
                    return q;
                });
            return response;
        }

        public BaseResponse<MonFeeModel> SaveMonFee(MonFeeGrantRequestEntity request)
        {
            BaseResponse<MonFeeModel> response = new BaseResponse<MonFeeModel>();
            try
            {
                if (request.payGrantModel.AgencyResult == Convert.ToInt32(NCIPStatusEnum.NotPassed))
                {
                    if (request.monfeeList != null && request.monfeeList.Count > 0)
                    {
                        foreach (var item in request.monfeeList)
                        {
                            item.Status = (int)request.payGrantModel.AgencyResult;
                            base.Save<NCIP_NSMONFEE, MonFeeModel>(item, (q) => q.NSMONFEEID == item.NSMonFeeID);
                            resService.SaveResidentMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(request.payGrantModel.AgencyResult));
                            resService.SaveDeductionMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(request.payGrantModel.AgencyResult));
                        }
                    }
                }

                else if (request.payGrantModel.AgencyResult == Convert.ToInt32(NCIPStatusEnum.Passed))
                {
                    var grantID = service.SavePayGrant(request);

                    if (request.monfeeList != null && request.monfeeList.Count > 0)
                    {
                        foreach (var item in request.monfeeList)
                        {
                            item.Status = Convert.ToInt32(request.payGrantModel.AgencyResult);
                            item.NCIPayGrantID = Convert.ToInt32(grantID);
                            base.Save<NCIP_NSMONFEE, MonFeeModel>(item, (q) => q.NSMONFEEID == item.NSMonFeeID);
                            resService.SaveResidentMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(request.payGrantModel.AgencyResult));
                            resService.SaveDeductionMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(request.payGrantModel.AgencyResult));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public object QueryYearMonthMonFeeInfo(BillV2 request)
        {
            var response = new MonFeeModel();
            Mapper.CreateMap<NCIP_NSMONFEE, MonFeeModel>();
            var model = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.FirstOrDefault(m => m.NSNO == request.OrgId && m.ISDELETE != true && m.YEARMONTH == request.BillMonth);
            response = Mapper.Map<MonFeeModel>(model);
            return response;
        }


        public BaseResponse<MonFeeInfoModel> QueryMonFeeInfo(int monFeeId)
        {
            var response = new BaseResponse<MonFeeInfoModel>();
            response.Data = new MonFeeInfoModel();
            response.Data.MonFeeEntity = new MonFeeEntity();
            response.Data.ResidentMonFeeEntityList = new List<ResidentMonFeeEntity>();
            Mapper.CreateMap<NCIP_NSMONFEE, MonFeeEntity>();
            var model = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.FirstOrDefault(m => m.NSMONFEEID == monFeeId && m.ISDELETE != true);
            response.Data.MonFeeEntity = Mapper.Map<MonFeeEntity>(model);
            response.Data.deAmount = unitOfWork.GetRepository<NCIA_DEDUCTION>().dbSet.Where(w => w.NSMONFEEID == monFeeId && w.ISDELETE == false).Sum(
                s => (double?)s.AMOUNT);
            var q = from monFee in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet
                    where monFee.NSMONFEEID == monFeeId && monFee.ISDELETE != true
                    join app in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE != true) on monFee.RESIDENTSSID equals app.IDNO into bs
                    from b in bs.DefaultIfEmpty()
                    select new ResidentMonFeeEntity
                    {
                        Hospentrydate = monFee.HOSPENTRYDATE,
                        Name = b.NAME,
                        Rsmonfeeid = monFee.RSMONFEEID,
                        Nsmonfeeid = monFee.NSMONFEEID,
                        Ncipaylevel = monFee.NCIPAYLEVEL,
                        Ncipayscale = monFee.NCIPAYSCALE,
                        Totalamount = monFee.TOTALAMOUNT,
                        Ncipay = monFee.NCIPAY,
                        Nsid = monFee.NSID,
                    };
            response.Data.ResidentMonFeeEntityList = q.ToList();
            return response;
        }

        public object GetDeductionList(BaseRequest request, long NSMONFEEID)
        {
            var response = base.Query<NCIA_DEDUCTION, Deduction>(request, (q) =>
            {
                q = q.Where(m => m.NSMONFEEID == NSMONFEEID);
                q = q.OrderByDescending(o => o.DEBITMONTH);
                return q;
            });
            return response;
        }
    }
}
