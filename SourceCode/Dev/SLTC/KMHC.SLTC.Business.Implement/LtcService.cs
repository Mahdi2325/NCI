using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class LtcService : BaseService, ILtcService
    {
        public void SaveData(MonthFeeModel model)
        {
            //机构月费用信息
            var nsMonFee = model.nsMonthFee;
            var org = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(w => w.NSNO == model.nsNo);
            if (org != null)
                nsMonFee.NSID = org.NSID;

            nsMonFee.NSNO = model.nsNo;
            nsMonFee.STATUS = (int)NCIPStatusEnum.Pending;
            nsMonFee.TrackingPropertyCreate(nsMonFee.CreateBy);

            //机构内参保人月费用信息
            nsMonFee.NCIP_RESIDENTMONFEE = model.rsMonthFee;
            foreach (var reMonFee in nsMonFee.NCIP_RESIDENTMONFEE)
            {
                reMonFee.NCIP_RESIDENTMONFEEDTL = new List<RESIDENTMONFEEDTL>();
                reMonFee.NSID = nsMonFee.NSID;
                reMonFee.STATUS = (int)NCIPStatusEnum.Pending;
                reMonFee.TrackingPropertyCreate(reMonFee.CreateBy);
                //参保人费用明细
                foreach (var reMonDtl in model.rsMonthFeeDtl.Where(m => m.FEENO == reMonFee.FEENO))
                {
                    reMonDtl.TrackingPropertyCreate(reMonDtl.CreateBy);
                    reMonFee.NCIP_RESIDENTMONFEEDTL.Add(reMonDtl);
                }
            }
            //nsMonFee.NCIA_DEDUCTION = model.nciDeduction;
            //foreach (var deduction in nsMonFee.NCIA_DEDUCTION)
            //{
            //    deduction.STATUS = (int)NCIPStatusEnum.Pending;
            //    deduction.TrackingPropertyCreate(deduction.CreateBy);
            //}

            var response = new BaseResponse<NSMONFEE>();
            Mapper.CreateMap<NSMONFEE, NCIP_NSMONFEE>();

            //Mapper.CreateMap<Deduction, NCIA_DEDUCTION>();

            Mapper.CreateMap<RESIDENTMONFEE, NCIP_RESIDENTMONFEE>();

            Mapper.CreateMap<RESIDENTMONFEEDTL, NCIP_RESIDENTMONFEEDTL>();
            NCIP_NSMONFEE nsmonfee = Mapper.Map<NSMONFEE, NCIP_NSMONFEE>(nsMonFee);
            unitOfWork.GetRepository<NCIP_NSMONFEE>().Insert(nsmonfee);
            unitOfWork.Save();
        }
        public object CancelData(string date, string nsno)
        {
            var list = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(w => (w.STATUS == (int)NCIPStatusEnum.Pending || w.STATUS == (int)NCIPStatusEnum.NotPassed) && w.NSNO == nsno && w.YEARMONTH == date && w.ISDELETE == false).ToList();
            unitOfWork.BeginTransaction();
            list.ForEach((p) =>
            {
                var rslist = unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet.Where(w => w.NSMONFEEID == p.NSMONFEEID && w.ISDELETE == false).ToList();
                rslist.ForEach((q) =>
                {
                    var rslistdtl = unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().dbSet.Where(w => w.RSMONFEEID == q.RSMONFEEID && w.ISDELETE == false).ToList();
                    rslistdtl.ForEach((m) =>
                    {
                        m.ISDELETE = true;
                        unitOfWork.GetRepository<NCIP_RESIDENTMONFEEDTL>().Update(m);
                    });
                    q.ISDELETE = true;
                    unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().Update(q);
                });
                var delist = unitOfWork.GetRepository<NCIA_DEDUCTION>().dbSet.Where(w => w.NSMONFEEID == p.NSMONFEEID && w.ISDELETE == false).ToList();
                delist.ForEach((q) =>
                {

                    q.ISDELETE = true;
                    unitOfWork.GetRepository<NCIA_DEDUCTION>().Update(q);

                });
                p.ISDELETE = true;
                unitOfWork.GetRepository<NCIP_NSMONFEE>().Update(p);
            });
            unitOfWork.Commit();
            return "撤回成功";
        }
        public object GetOrgMonthDataList(string beginTime, string endTime, string nsno)
        {
            var org = unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.FirstOrDefault(w => w.NSNO == nsno);
            if (org == null)
            {
                return null;
            }
            BaseRequest<MonFeeFilter> request = new BaseRequest<MonFeeFilter>() { Data = { OrgID = org.NSID, StartTime = beginTime, EndTime = endTime } };
            return base.Query<NCIP_NSMONFEE, MonFeeModel>(request, (q) =>
            {
                q = q.Where(o => o.NSID == request.Data.OrgID && o.ISDELETE == false);
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
        }
        public object GetOrgMonthData(long NSMonFeeID)
        {
            return new
            {
                OrgMonthData = base.Get<NCIP_NSMONFEE, MonFeeModel>((q) => q.NSMONFEEID == NSMonFeeID && q.ISDELETE == false),
                deDuction = unitOfWork.GetRepository<NCIA_DEDUCTION>().dbSet.Where(w => w.NSMONFEEID == NSMonFeeID).
                Sum(s => (double?)s.AMOUNT) ?? 0
            };
        }
        public object GetResMonthData(BaseRequest<MonthFeeFilter> request)
        {
            var orgData = base.Get<NCIP_NSMONFEE, MonFeeModel>((q1) => q1.NSMONFEEID == request.Data.NSMonFeeID && q1.ISDELETE == false);
            if (orgData == null)
            {
                return null;
            }
            var response = new BaseResponse<IList<ResidentMonFeeModel>>();
            var q = from r1 in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet
                    join r2 in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on r1.RESIDENTSSID equals r2.IDNO
                    where r2.ISDELETE==false
                    select new
                    {
                        r = r1,
                        Name = r2.NAME

                    };
            q = q.Where(m => m.r.NSMONFEEID == orgData.Data.NSMonFeeID && m.r.ISDELETE == false);
            q = q.OrderByDescending(m => m.r.RSMONFEEID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ResidentMonFeeModel>();
                foreach (dynamic item in list)
                {
                    ResidentMonFeeModel newItem = Mapper.DynamicMap<ResidentMonFeeModel>(item.r);
                    newItem.Name = item.Name;
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
        public object GetResMonthData(BaseRequest request, string sDate, string eDate, string nsno)
        {
            var response = new BaseResponse<IList<TreatmentAccount>>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A002", "A003" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;
            var nsMonFeeIdList = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(w => w.NSNO == nsno && w.ISDELETE == false
                  && string.Compare(w.YEARMONTH, sDate) >= 0 && string.Compare(w.YEARMONTH, eDate) <= 0).
                  Select(s => new  { nsmonfeeid = (long?)s.NSMONFEEID, yearMonth = s.YEARMONTH }).ToList();
            var nsMonFeeIdarr = nsMonFeeIdList.Select(s => s.nsmonfeeid);
            if (nsMonFeeIdList.Count <= 0)
            {
                return response;
            }
            var q = from r1 in unitOfWork.GetRepository<NCIP_RESIDENTMONFEE>().dbSet
                    where nsMonFeeIdarr.Contains(r1.NSMONFEEID) && r1.ISDELETE == false
                    join r2 in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet on r1.RESIDENTSSID equals r2.IDNO
                    where r2.ISDELETE==false
                    join r3 in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet on r1.RESIDENTSSID equals r3.IDNO
                    where r3.STATUS == (int)EnumAppCertStatus.审核通过
                    group new
                    {
                        r1.NCIPAY,
                        r1.TOTALAMOUNT,
                        r1.HOSPDAY,
                        r1.NCIPAYLEVEL,
                        r1.FEENO,
                        r1.RESIDENTSSID,
                        r2.NAME,
                        r2.GENDER,
                        r2.MCTYPE,
                        r2.RESIDENCE,
                        r2.DISEASE,
                        r3.AGENCYAPPROVEDCARETYPE,
                        r3.EVALUATIONTIME,
                        r1.NSMONFEEID,
                    } by new
                    {
                        r1.NCIPAYLEVEL,
                        r1.FEENO,
                        r1.RESIDENTSSID,
                        r2.NAME,
                        r2.GENDER,
                        r2.MCTYPE,
                        r2.RESIDENCE,
                        r2.DISEASE,
                        r3.AGENCYAPPROVEDCARETYPE,
                        r3.EVALUATIONTIME,
                        r1.NSMONFEEID,
                    } into g
                    select new TreatmentAccount
                    {
                        Ncipay = g.Sum(m => m.NCIPAY),
                        Totalamount = g.Sum(m => m.TOTALAMOUNT),
                        Hospday = g.Sum(m => m.HOSPDAY),
                        Ncipaylevel = g.Key.NCIPAYLEVEL,
                        NSMONFEEID = g.Key.NSMONFEEID,
                        Name = g.Key.NAME,
                        FeeNo=g.Key.FEENO,
                        Residentssid=g.Key.RESIDENTSSID,
                        McType = g.Key.MCTYPE,
                        Gender = g.Key.GENDER,
                        Residence = g.Key.RESIDENCE,
                        Disease = g.Key.DISEASE,
                        AgencyApprovedCareType = g.Key.AGENCYAPPROVEDCARETYPE,
                        EvaluationTime = g.Key.EVALUATIONTIME,
                    };
            var list = q.ToList().Select(s => new TreatmentAccount
            {
                Ncipay = s.Ncipay,
                Totalamount = s.Totalamount,
                Hospday = s.Hospday,
                Ncipaylevel = s.Ncipaylevel,
                yearMonthArr = nsMonFeeIdList.Where(w => w.nsmonfeeid == s.NSMONFEEID).Select(c => c.yearMonth).FirstOrDefault(),
                Name = s.Name,
                FeeNo=s.FeeNo,
                Residentssid=s.Residentssid,
                McType = s.McType,
                Gender = s.Gender,
                Residence = s.Residence,
                Disease = s.Disease,
                AgencyApprovedCareType = s.AgencyApprovedCareType,
                EvaluationTime = s.EvaluationTime,

            }).GroupBy(g => new {
                g.Ncipaylevel,
                g.Name,
                g.FeeNo,
                g.Residentssid,
                g.Gender,
                g.McType,
                g.Residence,
                g.Disease,
                g.AgencyApprovedCareType,
                g.EvaluationTime,
            }).Select(k => new TreatmentAccount {
                Ncipay = k.Sum(u=>u.Ncipay),
                Totalamount = k.Sum(u => u.Totalamount),
                Hospday = k.Sum(u => u.Hospday),
                Ncipaylevel = k.Key.Ncipaylevel,
                yearMonthArr =string.Join(",",k.Select(p => p.yearMonthArr)),
                Name = k.Key.Name,
                FeeNo=k.Key.FeeNo,
                Residentssid=k.Key.Residentssid,
                McTypeName = dict.Find(it => it.ItemType == "A002" && it.ItemCode == k.Key.McType.ToString()) != null ?
                dict.Find(it => it.ItemType == "A002" && it.ItemCode == k.Key.McType.ToString()).ItemName : "",
                Gender = k.Key.Gender,
                Residence = k.Key.Residence,
                Disease = dict.Find(it => it.ItemType == "A003" && it.ItemCode == k.Key.Disease.Split(',')[0]) != null ?
                dict.Find(it => it.ItemType == "A003" && it.ItemCode == k.Key.Disease.Split(',')[0]).ItemName : "",
                NsappcareTypeName = k.Key.AgencyApprovedCareType.ToString().Replace("1", "专护").Replace("2", "机构护理"),
                EvaluationTime = k.Key.EvaluationTime,
            }).OrderByDescending(o => o.Name);
            response.RecordsCount = list.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = list.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = list.ToList();
            }
            return response;
        }
    }
}
