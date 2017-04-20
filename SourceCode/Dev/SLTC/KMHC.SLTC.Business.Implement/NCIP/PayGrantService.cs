using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class PayGrantService : BaseService, IPayGrantService
    {

        IResidentMonfeeService resService = IOCContainer.Instance.Resolve<IResidentMonfeeService>();
        public long SavePayGrant(MonFeeGrantRequestEntity request)
        {
            var response = new BaseResponse<NCIP_NCIPAYGRANT>();
            response.Data = new NCIP_NCIPAYGRANT();

            NCIP_NCIPAYGRANT newItem = new NCIP_NCIPAYGRANT();
            newItem.GRANTID = Convert.ToInt32(base.GenerateCode("GrantId", EnumCodeKey.GrantId));
            newItem.NSID = request.monfeeList[0].NSID;
            newItem.GRANTYEAR = Convert.ToDateTime(request.monfeeList[0].YearMonth).Year.ToString();
            newItem.TOTALRESIDENT = request.payGrantModel.TotalResident;
            newItem.TOTALHOSPDAY = request.payGrantModel.TotalHospDay;
            newItem.TOTALAMOUNT = request.payGrantModel.TotalAmount;
            newItem.TOTALNCIPAY = request.payGrantModel.TotalNCIpay;
            newItem.STATUS = Convert.ToInt32(NCIPStatusEnum.Passed);
            newItem.CREATORNAME = SecurityHelper.CurrentPrincipal.UserName;
            newItem.AGENCYRESULT = Convert.ToInt32(NCIPStatusEnum.Passed);
            newItem.AGENCYCOMMENT = request.payGrantModel.AgencyComment;
            newItem.AGENCYOPERATETIME = DateTime.Now;
            newItem.AGENCYUSERID = SecurityHelper.CurrentPrincipal.EmpNo;
            newItem.AGENCYID = SecurityHelper.CurrentPrincipal.OrgId;
            newItem.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
            newItem.CREATETIME = DateTime.Now;
            newItem.ISDELETE = false;
            unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().Insert(newItem);
            unitOfWork.Save();
            return newItem.GRANTID;
        }

        public BaseResponse<NCIPayGrantEntity> QueryPayGrantinfo(int id)
        {
            var response = new BaseResponse<NCIPayGrantEntity>();

            response.Data = new NCIPayGrantEntity();
            Mapper.CreateMap<NCIP_NCIPAYGRANT, NCIPayGrantModel>();
            var model = unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().dbSet.FirstOrDefault(m => m.ISDELETE != true && m.GRANTID == id);
            var payGrant = Mapper.Map<NCIPayGrantModel>(model);
            if (payGrant != null)
            {
                response.Data.GrantID = payGrant.GrantID;
                response.Data.ServiceDepositID = payGrant.ServiceDepositID;
                response.Data.NSID = payGrant.NSID;
                response.Data.GrantYear = payGrant.GrantYear;
                response.Data.TotalResident = payGrant.TotalResident;
                response.Data.TotalHospDay = payGrant.TotalHospDay;
                response.Data.TotalAmount = payGrant.TotalAmount;
                response.Data.TotalNCIpay = payGrant.TotalNCIpay;
                response.Data.Status = payGrant.Status;
                response.Data.AdjustAmount = payGrant.AdjustAmount;
                var ActualPayment = payGrant.TotalNCIpay + Convert.ToDecimal(payGrant.AdjustAmount);
                response.Data.ServiceSecurity = Convert.ToDecimal(Convert.ToDouble(ActualPayment) * 0.05);
                response.Data.ActualPayment = ActualPayment - response.Data.ServiceSecurity; 
                response.Data.AdjustReason = payGrant.AdjustReason;
                response.Data.AgencyComment = payGrant.AgencyComment;

                Mapper.CreateMap<NCIP_NSMONFEE, MonFeeModel>();
                var modelpay = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE == false && m.NCIPAYGRANTID == payGrant.GrantID).OrderByDescending(m => m.CREATETIME).ToList();
                var monFeeList = Mapper.Map<List<MonFeeModel>>(modelpay);
                if (monFeeList != null)
                {
                    foreach (var mon in monFeeList)
                    {
                        response.Data.GrantMonTh += (Convert.ToDateTime(mon.YearMonth).Month + ",");
                    }
                }
                response.Data.GrantMonTh = response.Data.GrantMonTh.Substring(0, response.Data.GrantMonTh.Length - 1);

                Mapper.CreateMap<NCIP_NSMONFEE, MonFeeEntity>();
                var monfee = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.NCIPAYGRANTID == payGrant.GrantID && m.ISDELETE != true).ToList();
                var monfeeList = Mapper.Map<List<MonFeeEntity>>(monfee);
                if (monfeeList != null & monfeeList.Count > 0)
                {
                    response.Data.monfeeList = monfeeList;
                }
            }
            return response;
        }



        public BaseResponse<List<NCIPayGrantEntity>> QueryPayGrantList(string year, string nsid)
        {
            var response = new BaseResponse<List<NCIPayGrantEntity>>();
            response.Data = new List<NCIPayGrantEntity>();

            int passed = Convert.ToInt32(NCIPStatusEnum.Passed);
            int appropriated = Convert.ToInt32(NCIPStatusEnum.Appropriated);

            Mapper.CreateMap<NCIP_NCIPAYGRANT, NCIPayGrantModel>();
            var model = unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().dbSet.Where(m => m.ISDELETE == false && m.GRANTYEAR == year && (m.STATUS == passed || m.STATUS == appropriated)).ToList();
            //Mod By Duke
            if(nsid!="-1")
            {
                model = model.Where(m => m.NSID == nsid).ToList();
            }
            var payGrantList = Mapper.Map<List<NCIPayGrantModel>>(model);
            payGrantList = payGrantList.OrderByDescending(m => m.CreateTime).ToList();
            if (payGrantList != null && payGrantList.Count > 0)
            {
                foreach (var item in payGrantList)
                {
                    NCIPayGrantEntity entity = new NCIPayGrantEntity();
                    entity.GrantID = item.GrantID;
                    entity.ServiceDepositID = item.ServiceDepositID;
                    entity.NSID = item.NSID;
                    entity.GrantYear = item.GrantYear;
                    entity.TotalResident = item.TotalResident;
                    entity.TotalHospDay = item.TotalHospDay;
                    entity.TotalAmount = item.TotalAmount;
                    entity.TotalNCIpay = item.TotalNCIpay;
                    entity.Status = item.Status;

                    var adAmount = item.AdjustAmount == null ? 0 : item.AdjustAmount;
                    var ActualPayment = Convert.ToDecimal(adAmount + item.TotalNCIpay);

                    entity.ServiceSecurity = Convert.ToDecimal(Convert.ToDouble(ActualPayment) * 0.05);
                    Mapper.CreateMap<NCIP_NSMONFEE, MonFeeModel>();
                    var modelpay = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE == false && m.NCIPAYGRANTID == item.GrantID).OrderByDescending(m => m.CREATETIME).ToList();
                    var monFeeList = Mapper.Map<List<MonFeeModel>>(modelpay);
                    if (monFeeList != null && monFeeList.Count > 0)
                    {
                        foreach (var mon in monFeeList)
                        {
                            entity.GrantMonTh += (Convert.ToDateTime(mon.YearMonth).Month + ",");
                        }
                        entity.GrantMonTh = entity.GrantMonTh.Substring(0, entity.GrantMonTh.Length - 1);
                        response.Data.Add(entity);
                    }
                    else
                    {
                        response.ResultCode = 101;
                        response.ResultMessage = "查无数据";
                    }
                }
            }
            else
            {
                response.ResultCode = 101;
                response.ResultMessage = "查无数据";
            }
            return response;
        }

        public BaseResponse<List<NCIPayGrantEntity>> QueryLTCPayGrant(string year, string nsno)
        {
            var response = new BaseResponse<List<NCIPayGrantEntity>>();
            response.Data = new List<NCIPayGrantEntity>();
            var org = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(w => w.NSNO == nsno).FirstOrDefault();

            if (org != null)
            {
                int passed = Convert.ToInt32(NCIPStatusEnum.Passed);
                int appropriated = Convert.ToInt32(NCIPStatusEnum.Appropriated);

                Mapper.CreateMap<NCIP_NCIPAYGRANT, NCIPayGrantModel>();
                var model = unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().dbSet.Where(m => m.ISDELETE == false && m.GRANTYEAR == year && m.NSID == org.NSID && (m.STATUS == passed || m.STATUS == appropriated)).ToList();
                var payGrantList = Mapper.Map<List<NCIPayGrantModel>>(model);
                payGrantList = payGrantList.OrderByDescending(m => m.CreateTime).ToList();
                if (payGrantList != null && payGrantList.Count > 0)
                {
                    foreach (var item in payGrantList)
                    {
                        NCIPayGrantEntity entity = new NCIPayGrantEntity();
                        entity.GrantID = item.GrantID;
                        entity.ServiceDepositID = item.ServiceDepositID;
                        entity.NSID = item.NSID;
                        entity.GrantYear = item.GrantYear;
                        entity.TotalResident = item.TotalResident;
                        entity.TotalHospDay = item.TotalHospDay;
                        entity.TotalNCIpay = item.TotalNCIpay;
                        entity.Status = item.Status;
                      
                        var adAmount = item.AdjustAmount == null ? 0 : item.AdjustAmount;
                        var ActualPayment = Convert.ToDecimal(adAmount + item.TotalNCIpay);
                        entity.ServiceSecurity = Convert.ToDecimal(Convert.ToDouble(ActualPayment) * 0.05);
                        entity.ActualPayment = ActualPayment - entity.ServiceSecurity;
                        entity.TotalAmount = item.TotalAmount;
                        Mapper.CreateMap<NCIP_NSMONFEE, MonFeeModel>();
                        var modelpay = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE == false && m.NCIPAYGRANTID == item.GrantID).OrderByDescending(m => m.CREATETIME).ToList();
                        var monFeeList = Mapper.Map<List<MonFeeModel>>(modelpay);
                        if (monFeeList != null && monFeeList.Count > 0)
                        {
                            foreach (var mon in monFeeList)
                            {
                                entity.GrantMonTh += (Convert.ToDateTime(mon.YearMonth).Month + ",");
                            }
                            entity.GrantMonTh = entity.GrantMonTh.Substring(0, entity.GrantMonTh.Length - 1);
                        }
                        response.Data.Add(entity);
                    }
                }
                else
                {
                    response.ResultCode = 101;
                    response.ResultMessage = "查无数据";
                }
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "当前机构注册信息有误，请联系管理员核实！";
            }
            return response;
        }
        public BaseResponse<NCIPayGrantEntity> SavePayGrant(NCIPayGrantEntity request)
        {

            var response = new BaseResponse<NCIPayGrantEntity>();

            try
            {
                NCIP_SERVICEDEPOSIT servicedeposit = new NCIP_SERVICEDEPOSIT();
                servicedeposit.SERVICEDEPOSITID = Convert.ToInt32(base.GenerateCode("serviceId", EnumCodeKey.serviceId));
                servicedeposit.NSID = request.NSID;
                servicedeposit.YEARMONTH = request.GrantYear + "-" + request.GrantMonTh;
                servicedeposit.AMOUNT = Convert.ToDecimal(Convert.ToDouble(request.TotalNCIpay + request.AdjustAmount) * 0.05);
                servicedeposit.SERVICEDEPOSITDATE = DateTime.Now;
                servicedeposit.STATUS = 0;
                servicedeposit.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                servicedeposit.CREATETIME = DateTime.Now;
                servicedeposit.ISDELETE = false;
                long servicedepositid = insertServicedeposit(servicedeposit);

                var model = unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().dbSet.FirstOrDefault(m => m.GRANTID == request.GrantID && m.ISDELETE == false);
                model.STATUS = Convert.ToInt32(NCIPStatusEnum.Appropriated);
                model.SERVICEDEPOSITID = Convert.ToInt32(servicedepositid);
                model.ADJUSTAMOUNT = request.AdjustAmount;
                model.ADJUSTREASON = request.AdjustReason;
                model.CREATETIME = DateTime.Now;
                model.ISDELETE = false;
                model.CREATORNAME = request.CreatorName == null ? SecurityHelper.CurrentPrincipal.UserName : request.CreatorName;
                unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().Update(model);
                unitOfWork.Save();

                Mapper.CreateMap<NCIP_NSMONFEE, MonFeeModel>();
                var modelpay = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(m => m.ISDELETE == false && m.NCIPAYGRANTID == request.GrantID).ToList();
                var monFeeList = Mapper.Map<List<MonFeeModel>>(modelpay);
                if (monFeeList != null)
                {
                    foreach (var item in monFeeList)
                    {
                        item.Status = Convert.ToInt32(NCIPStatusEnum.Appropriated);
                        item.CreatorName = request.CreatorName == null ? SecurityHelper.CurrentPrincipal.UserName : item.CreatorName;
                        base.Save<NCIP_NSMONFEE, MonFeeModel>(item, (q) => q.NSMONFEEID == item.NSMonFeeID);
                        resService.SaveResidentMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(NCIPStatusEnum.Appropriated));
                        resService.SaveDeductionMonfeeStatus(item.NSMonFeeID, Convert.ToInt32(NCIPStatusEnum.Appropriated));
                    }
                }
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// 保存服务记录保证金数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long insertServicedeposit(NCIP_SERVICEDEPOSIT request)
        {
            unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().Insert(request);
            unitOfWork.Save();
            return request.SERVICEDEPOSITID;
        }
    }
}
