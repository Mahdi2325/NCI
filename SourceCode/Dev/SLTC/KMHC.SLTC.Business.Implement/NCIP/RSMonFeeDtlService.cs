using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
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
    public class RSMonFeeDtlService : BaseService, IRSMonFeeDtlService
    {
        public BaseResponse<IList<ResidentMonFeeModel>> QueryRSMonFee(BaseRequest<MonFeeFilter> request)
        {
            BaseResponse<IList<ResidentMonFeeModel>> response = new BaseResponse<IList<ResidentMonFeeModel>>();
            var rsMonFee = (from a in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet
                            join b in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on a.RESIDENTSSID equals b.IDNO
                            join c in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on a.NSID equals c.NSID
                            where a.RSMONFEEID == request.Data.OrgMonFeeId && a.ISDELETE==false
                            select new ResidentMonFeeModel()
                            {
                                Rsmonfeeid = a.RSMONFEEID,
                                Nsmonfeeid = a.NSMONFEEID,
                                Nsid = a.NSID,
                                NsName=c.NSNAME,
                                Residentssid = a.RESIDENTSSID,
                                Name=b.NAME,
                                Certno = a.CERTNO,
                                Hospentrydate= a.HOSPENTRYDATE,
                                Hospdischargedate=a.HOSPDISCHARGEDATE,
                                Hospday = a.HOSPDAY,
                                Ncipaylevel = a.NCIPAYLEVEL,
                                Ncipayscale = a.NCIPAYSCALE,
                                Totalamount = a.TOTALAMOUNT,
                                Ncipay = a.NCIPAY,
                                Status = a.STATUS,
                                Createby = a.CREATEBY,
                                Createtime = a.CREATETIME,
                                Updateby = a.UPDATEBY,
                                Updatetime = a.UPDATETIME,
                                Isdelete = a.ISDELETE,
                            }).ToList();


            response.Data = rsMonFee;
            return response;
        }

        BaseResponse<IList<ResidentMonFeeDtlModel>> IRSMonFeeDtlService.QueryRSMonFeeDtl(BaseRequest<MonFeeFilter> request)
        {
            BaseResponse<IList<ResidentMonFeeDtlModel>> response = new BaseResponse<IList<ResidentMonFeeDtlModel>>();
            if (request.Data.FeeType == "NCI")
            {
                var rsNciMonFeeDtl = (from a in unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().dbSet
                                   join b in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet on a.RSMONFEEID equals b.RSMONFEEID
                                   where a.RSMONFEEID == request.Data.OrgMonFeeId && a.ISNCIITEM == true && a.ISDELETE != true
                                   orderby a.FEETYPE ascending, a.CREATETIME descending
                                   select new ResidentMonFeeDtlModel()
                                   {
                                       RsMonFeeDtlId = a.RSMONFEEDTLID,
                                       RsMonFeeId = a.RSMONFEEID,
                                       FeeName = a.FEENAME,
                                       FeeType = a.FEETYPE,
                                       MCCode = a.MCCODE,
                                       UnitPrice = a.UNITPRICE,
                                       Qty = a.QTY,
                                       IsNCIItem=a.ISNCIITEM,
                                       Amount = a.AMOUNT,
                                       TakeTime = a.TAKETIME,
                                       OperatorName = a.OPERATORNAME,
                                       CreateBy = a.CREATEBY,
                                       CreateTime = a.CREATETIME,
                                       UpdateBy = a.UPDATEBY,
                                       UpdateTime = a.UPDATETIME,
                                       IsDelete = a.ISDELETE,
                                   }).ToList();
                response.RecordsCount = rsNciMonFeeDtl.Count;
                List<ResidentMonFeeDtlModel> list = null;
                if (request != null && request.PageSize > 0)
                {
                    list = rsNciMonFeeDtl.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = rsNciMonFeeDtl.ToList();
                }

                response.Data = list;
            }
            else if (request.Data.FeeType == "SELF")
            {
                var rsSelfMonFeeDtl = (from a in unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().dbSet
                                   join b in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet on a.RSMONFEEID equals b.RSMONFEEID
                                   where a.RSMONFEEID == request.Data.OrgMonFeeId && a.ISNCIITEM != true && a.ISDELETE != true 
                                       orderby a.FEETYPE ascending, a.CREATETIME descending
                                   select new ResidentMonFeeDtlModel()
                                   {
                                       RsMonFeeDtlId = a.RSMONFEEDTLID,
                                       RsMonFeeId = a.RSMONFEEID,
                                       FeeName = a.FEENAME,
                                       FeeType = a.FEETYPE,
                                       MCCode = a.MCCODE,
                                       UnitPrice = a.UNITPRICE,
                                       Qty = a.QTY,
                                       IsNCIItem = a.ISNCIITEM,
                                       Amount = a.AMOUNT,
                                       TakeTime = a.TAKETIME,
                                       OperatorName = a.OPERATORNAME,
                                       CreateBy = a.CREATEBY,
                                       CreateTime = a.CREATETIME,
                                       UpdateBy = a.UPDATEBY,
                                       UpdateTime = a.UPDATETIME,
                                       IsDelete = a.ISDELETE,
                                   }).ToList();
                response.RecordsCount = rsSelfMonFeeDtl.Count;
                List<ResidentMonFeeDtlModel> list = null;
                if (request != null && request.PageSize > 0)
                {
                    list = rsSelfMonFeeDtl.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = rsSelfMonFeeDtl.ToList();
                }

                response.Data = list;
            }
            
            return response;
        }
    }
}
