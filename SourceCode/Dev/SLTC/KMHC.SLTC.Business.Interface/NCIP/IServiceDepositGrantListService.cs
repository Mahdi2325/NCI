using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIP
{
    public interface IServiceDepositGrantListService : IBaseService
    {
        /// <summary>
        ///查询服务保证金拨付列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        BaseResponse<IList<ServiceDepositGrant>> QueryServiceDepositGrantList(BaseRequest<ServiceDepositGrantFilter> request);

        object GetServiceDepositGrantByNsNo(string nsNo, int CurrentPage, int PageSize);

        BaseResponse<ServiceDepositGrant> DeleteServiceDepositGrant(ServiceDepositGrant request);
    }
}
