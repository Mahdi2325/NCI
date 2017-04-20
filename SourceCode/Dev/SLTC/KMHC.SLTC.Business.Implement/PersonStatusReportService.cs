using System;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Business.Entity.Filter.NCIA;


namespace KMHC.SLTC.Business.Implement
{
    public class PersonStatusReportService : BaseService, IPersonStatusReportService
    {
        IAuditYearCertService service = IOCContainer.Instance.Resolve<IAuditYearCertService>();

        public BaseResponse<PersonStatusReportModel> QueryPersonStatusInfo(DateTime? startDate, DateTime? endDate)
        {
            var response = new BaseResponse<PersonStatusReportModel>();
            var lastYeardate = Convert.ToDateTime("2016-12-31 23:59:59");
            var yeardate = Convert.ToDateTime("2017-03-31 23:59:59");
            var xyyValue = ((int)OrgValue.Xyy).ToString();
            var qkyyValue = ((int)OrgValue.Qkyy).ToString();
            var jmyyValue = ((int)OrgValue.Jmyy).ToString();

            response.Data = new PersonStatusReportModel();
            if (startDate.Value != null && endDate.Value != null)
            {
                #region 资格证待遇申请统计
                response.Data.AppCertlastYearNum = new StatisticalNum();
                #region 取消查询
                var request = new BaseRequest<AuditYearCertFilter>()
                {
                    CurrentPage = 1,
                    PageSize = 10000,
                    Data = new AuditYearCertFilter()
                    {
                        Name = "",
                        IdNo = "",
                        NsID = "-1",
                        Status = "N",
                    }
                };
                var yearList = service.GetYearCertlist(request);
                #endregion
                #region 截止2016年年12月31日			
                #region 申请数量
                response.Data.AppCertlastYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                             where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                             select 0).Count();
                response.Data.AppCertlastYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                           where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                           select 0).Count();
                response.Data.AppCertlastYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                            where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                            select 0).Count();
                response.Data.AppCertlastYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                            where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                            select 0).Count();
                #endregion
                #region 通过数量
                response.Data.ByCertlastYearNum = new StatisticalNum();
                response.Data.ByCertlastYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                            where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                            select 0).Count();
                response.Data.ByCertlastYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                          where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                          select 0).Count();
                response.Data.ByCertlastYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                           where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                           select 0).Count();
                response.Data.ByCertlastYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                           where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                           select 0).Count();
                #endregion
                #region 未通过数量
                response.Data.UnAppCertlastYearNum = new StatisticalNum();
                response.Data.UnAppCertlastYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                               where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                               select 0).Count();
                response.Data.UnAppCertlastYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                             where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                             select 0).Count();
                response.Data.UnAppCertlastYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                              where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                              select 0).Count();
                response.Data.UnAppCertlastYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                              where o.CREATETIME.HasValue && o.CREATETIME <= lastYeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                              select 0).Count();
                #endregion
                #region 取消数量
                response.Data.CancelCertlastYearNum = new StatisticalNum();
                response.Data.CancelCertlastYearNum.TotalNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= lastYeardate).Count();
                response.Data.CancelCertlastYearNum.XyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= lastYeardate && m.NsID == xyyValue).Count();
                response.Data.CancelCertlastYearNum.QkyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= lastYeardate && m.NsID == qkyyValue).Count();
                response.Data.CancelCertlastYearNum.JmyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= lastYeardate && m.NsID == jmyyValue).Count();
                #endregion
                #endregion
                #region 截止2017年3月31日	
                response.Data.AppCertYearNum = new StatisticalNum();
                #region 申请数量
                response.Data.AppCertYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                         select 0).Count();
                response.Data.AppCertYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                       select 0).Count();
                response.Data.AppCertYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                        select 0).Count();
                response.Data.AppCertYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                        select 0).Count();
                #endregion
                #region 通过数量
                response.Data.ByCertYearNum = new StatisticalNum();
                response.Data.ByCertYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                        select 0).Count();
                response.Data.ByCertYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                      select 0).Count();
                response.Data.ByCertYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                       select 0).Count();
                response.Data.ByCertYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                       select 0).Count();
                #endregion
                #region 未通过数量
                response.Data.UnAppCertYearNum = new StatisticalNum();
                response.Data.UnAppCertYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                           select 0).Count();
                response.Data.UnAppCertYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                         select 0).Count();
                response.Data.UnAppCertYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                          select 0).Count();
                response.Data.UnAppCertYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME <= yeardate && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                          select 0).Count();
                #endregion
                #region 取消数量
                response.Data.CancelCertYearNum = new StatisticalNum();
                response.Data.CancelCertYearNum.TotalNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= yeardate).Count();
                response.Data.CancelCertYearNum.XyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= yeardate && m.NsID == xyyValue).Count();
                response.Data.CancelCertYearNum.QkyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= yeardate && m.NsID == qkyyValue).Count();
                response.Data.CancelCertYearNum.JmyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= yeardate && m.NsID == jmyyValue).Count();
                #endregion
                #endregion
                #region 查询期间	

                #region 申请数量
                response.Data.AppCertSearchYearNum = new StatisticalNum();
                response.Data.AppCertSearchYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                               select 0).Count();
                response.Data.AppCertSearchYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                             select 0).Count();
                response.Data.AppCertSearchYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                              select 0).Count();
                response.Data.AppCertSearchYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                              select 0).Count();
                #endregion
                #region 通过数量
                response.Data.ByCertSearchYearNum = new StatisticalNum();
                response.Data.ByCertSearchYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                              select 0).Count();
                response.Data.ByCertSearchYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                            select 0).Count();
                response.Data.ByCertSearchYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                             select 0).Count();
                response.Data.ByCertSearchYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                             select 0).Count();
                #endregion
                #region 未通过数量
                response.Data.UnAppCertSearchYearNum = new StatisticalNum();
                response.Data.UnAppCertSearchYearNum.TotalNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                         where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                                 select 0).Count();
                response.Data.UnAppCertSearchYearNum.XyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                       where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == xyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                               select 0).Count();
                response.Data.UnAppCertSearchYearNum.QkyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == qkyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                                select 0).Count();
                response.Data.UnAppCertSearchYearNum.JmyyNum = (from o in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet
                                                        where o.CREATETIME.HasValue && o.CREATETIME >= startDate.Value && o.CREATETIME <= endDate.Value && o.NSID == jmyyValue && o.ISDELETE != true && o.STATUS == (int)EnumAppCertStatus.审核不通过 && o.STATUS != (int)EnumAppCertStatus.重新审核
                                                                select 0).Count();
                #endregion
                #region 取消数量
                response.Data.CancelCertSearchYearNum = new StatisticalNum();
                response.Data.CancelCertSearchYearNum.TotalNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= endDate.Value && m.UpdateTime >= startDate.Value).Count();
                response.Data.CancelCertSearchYearNum.XyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= endDate.Value && m.UpdateTime >= startDate.Value && m.NsID == xyyValue).Count();
                response.Data.CancelCertSearchYearNum.QkyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= endDate.Value && m.UpdateTime >= startDate.Value && m.NsID == qkyyValue).Count();
                response.Data.CancelCertSearchYearNum.JmyyNum = yearList.Data.Where(m => m.UpdateTime.HasValue && m.UpdateTime.Value <= endDate.Value && m.UpdateTime >= startDate.Value && m.NsID == jmyyValue).Count();

                #endregion
                #endregion
                #endregion

            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "查询区间无效请重新选择";
            }

            return response;
        }



    }
}
