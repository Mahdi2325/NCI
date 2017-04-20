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
    public class ServiceDepositGrantListService : BaseService, IServiceDepositGrantListService
    {

        #region 查询服务保证金拨付记录
        /// <summary>
        ///查询服务保证金拨付记录
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        public BaseResponse<IList<ServiceDepositGrant>> QueryServiceDepositGrantList(BaseRequest<ServiceDepositGrantFilter> request)
        {
            string  months = "";
            BaseResponse<IList<ServiceDepositGrant>> response = new BaseResponse<IList<ServiceDepositGrant>>();
            Mapper.CreateMap<NCIP_SERVICEDEPOSITGRANT, ServiceDepositGrant>();
            var q = from m in unitOfWork.GetRepository<NCIP_SERVICEDEPOSITGRANT>().dbSet
                    select m;
            q = q.Where(m => m.ISDELETE != true);
            if (Convert.ToInt32(request.Data.NSID) != -1)
            {
                q = q.Where(m => m.NSID == request.Data.NSID);
            }
            q = q.OrderByDescending(m => new { m.YEAR, m.CREATETIME });
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ServiceDepositGrant>();
                foreach (dynamic item in list)
                {
                    months = "";
                    ServiceDepositGrant newItem = Mapper.DynamicMap<ServiceDepositGrant>(item);
                    var month = from m in unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet
                                select m;
                    month = month.Where(m => m.SDGRANTID == newItem.SdGrantid);
                    foreach (var mon in month.ToList())
                    {
                       var monthList= mon.YEARMONTH.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                       months += monthList[1] + ",";
                    }
                    newItem.Months = months.TrimEnd(',');
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


        public object GetServiceDepositGrantByNsNo(string nsNo, int CurrentPage, int PageSize)
        {
            string months = "";
            var org = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Where(w => w.NSNO == nsNo).FirstOrDefault();
            if (org == null)
            {
                return null;
            }
            BaseResponse<IList<ServiceDepositGrant>> response = new BaseResponse<IList<ServiceDepositGrant>>();
            Mapper.CreateMap<NCIP_SERVICEDEPOSITGRANT, ServiceDepositGrant>();
            var q = from m in unitOfWork.GetRepository<NCIP_SERVICEDEPOSITGRANT>().dbSet
                    select m;

            q = q.Where(m => m.ISDELETE != true);
            if (Convert.ToInt32(org.NSID) != -1)
            {
                q = q.Where(m => m.NSID == org.NSID);
            }
            q = q.OrderByDescending(m => new { m.YEAR, m.CREATETIME });
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ServiceDepositGrant>();
                foreach (dynamic item in list)
                {
                    months = "";
                    ServiceDepositGrant newItem = Mapper.DynamicMap<ServiceDepositGrant>(item);
                    var month = from m in unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet
                                select m;
                    month = month.Where(m => m.SDGRANTID == newItem.SdGrantid);
                    foreach (var mon in month.ToList())
                    {
                        var monthList = mon.YEARMONTH.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        months += monthList[1] + ",";
                    }
                    newItem.Months = months.TrimEnd(',');
                    response.Data.Add(newItem);
                }
            };
            if (PageSize > 0)
            {
                var list = q.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<ServiceDepositGrant> DeleteServiceDepositGrant(ServiceDepositGrant request)
        {
            var response = new BaseResponse<ServiceDepositGrant>(); 
            var now = DateTime.Now;

            //unitOfWork.BeginTransaction();
            #region 将保证金拨付表中的一条记录的状态置为删除状态

            request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
            request.UpdateTime = DateTime.Now;
            request.IsDelete = true;
            response=base.Save<NCIP_SERVICEDEPOSITGRANT, ServiceDepositGrant>(request, (q) => q.SDGRANTID == request.SdGrantid);

            #region 更新保证金表中的信息状态
            var SerDepo = unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet.Where(m => m.NSID == request.NsId && m.SDGRANTID == request.SdGrantid && m.ISDELETE == false);
            if (SerDepo != null)
            {
                foreach (var sd in SerDepo)
                {
                    sd.SDGRANTID = null;
                    sd.STATUS = 0;
                    sd.UPDATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    sd.UPDATETIME = DateTime.Now;
                }
            }
            unitOfWork.Save();
            #endregion


            //unitOfWork.Commit();
            #endregion
            return response;
        }
        

    }
}
