using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIP;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIP
{
    public class ServiceDepositService : BaseService, IServiceDepositService
    {

        #region 查询未发放的服务保证金
        /// <summary>
        ///查询服务保证金列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        public BaseResponse<IList<ServiceDeposit>> QueryServiceDepositList(BaseRequest<ServiceDepositFilter> request)
        {
            BaseResponse<IList<ServiceDeposit>> response = new BaseResponse<IList<ServiceDeposit>>();
            Mapper.CreateMap<NCIP_SERVICEDEPOSIT, ServiceDeposit>();
            var q = from m in unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet
                    select m;
            
            q = q.Where(m => m.ISDELETE == false);

            if (!string.IsNullOrEmpty(request.Data.YearMonth))
            {
                q = q.Where(m => m.YEARMONTH.Contains(request.Data.YearMonth));
            }
            if (Convert.ToInt32(request.Data.NSID) != -1)
            {
                q = q.Where(m => m.NSID == request.Data.NSID);
            }
            q = q.OrderBy(m => m.YEARMONTH);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ServiceDeposit>();
                foreach (dynamic item in list)
                {
                    ServiceDeposit newItem = Mapper.DynamicMap<ServiceDeposit>(item);
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

    }
}
