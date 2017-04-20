using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIP;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIP
{
    public class ServiceDepositGrantService : BaseService, IServiceDepositGrantService
    {
        #region 服务保证金拨付
        /// <summary>
        /// 服务保证金拨付
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns>
        public BaseResponse<ServiceDepositGrant> SaveServiceDepositGrant(ServiceDepositGrant request)
        {
            var response = new BaseResponse<ServiceDepositGrant>(); 
            var now = DateTime.Now;

            //unitOfWork.BeginTransaction();
            #region 向保证金拨付表中增加一条记录

            request.CreateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
            request.CreateTime = DateTime.Now;
            request.IsDelete = false;
            response=base.Save<NCIP_SERVICEDEPOSITGRANT, ServiceDepositGrant>(request, (q) => q.SDGRANTID == request.SdGrantid);

            #region 更新保证金表中的信息状态
            var SerDepo = unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet.Where(m => m.NSID == request.NsId && m.YEARMONTH.Contains(request.Year) && m.STATUS==0 && m.ISDELETE == false);
            if (SerDepo != null)
            {
                foreach (var sd in SerDepo)
                {
                    sd.SDGRANTID = request.SdGrantid;
                    sd.STATUS = 1;
                    sd.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    sd.UPDATETIME = DateTime.Now;
                    //unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().Update(SerDepo);
                }
            }
            unitOfWork.Save();
            #endregion


            //unitOfWork.Commit();
            #endregion
            return response;
        }
        #endregion
    }
}
