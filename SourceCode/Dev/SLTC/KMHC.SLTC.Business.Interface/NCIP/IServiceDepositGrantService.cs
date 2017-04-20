using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIP
{
    public interface IServiceDepositGrantService : IBaseService
    {

        /// <summary>
        /// 服务保证金拨付
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns> 
        BaseResponse<ServiceDepositGrant> SaveServiceDepositGrant(ServiceDepositGrant request);
    }
}
