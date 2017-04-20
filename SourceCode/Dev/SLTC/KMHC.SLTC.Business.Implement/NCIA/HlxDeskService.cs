using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIA
{
    public class HlxDeskService : BaseService, IHlxDeskService
    {
        public object GetHeadMsg(dynamic d)
        {
            return new
            {
                appCertedResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)EnumAppCertStatus.审核通过),
                settleAmount = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(w => w.ISDELETE == false && w.STATUS == (int)NCIPStatusEnum.Passed
                ).Sum(s => (decimal?)s.TOTALNCIPAY)??0,
                dispatchAmount = unitOfWork.GetRepository<NCIP_SERVICEDEPOSIT>().dbSet.Where(w => w.ISDELETE == false && w.STATUS == 0).Sum(s => (decimal?)s.AMOUNT),
                totalResNum = d.totalResNum - d.leaveResNum,
                leaveResNum = d.leaveResNum
            };
        }
        public object GetAppcertStatistics()
        {
            return unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Select(s => new { s.NSNAME, s.NSID }).ToList().Select(s => new
            {
                orgName = s.NSNAME,
                applyResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == s.NSID &&
                    w.ISDELETE == false).Select(s1 => s1.IDNO).Distinct().Count(),
                passResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == s.NSID && w.ISDELETE == false &&
                    w.STATUS == (int)EnumAppCertStatus.审核通过).Count(),
            }).ToList();
        }
        public object GetDeclareState()
        {
            var date = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
            return unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Select(s => new { s.NSNAME, s.NSID }).ToList().Select(s => new
            {
                orgName = s.NSNAME,
                declareTime = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(w => w.NSID == s.NSID &&
                    w.ISDELETE == false && w.YEARMONTH == date).Select(s1 => s1.YEARMONTH).FirstOrDefault()
            }).ToList();
        }
        public object GetRequireAppItem()
        {
            return new
            {
                appcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)EnumAppCertStatus.待审核),
                apphosp = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Count(c => c.ISDELETE == false && c.AGENCYRESULT == (int)EnumAppHospStatus.待审核),
                monthFee = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)NCIPStatusEnum.Pending),
                appropriation = unitOfWork.GetRepository<NCIP_NCIPAYGRANT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)NCIPStatusEnum.Passed),
            };
        }
        public object GetHlxSbaoHeadMsg()
        {
            return new
            {
                appCertedResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)EnumAppCertStatus.审核通过 &&
                c.NSID == SecurityHelper.CurrentPrincipal.OrgId),
                appCertingResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)EnumAppCertStatus.待审核 &&
                c.NSID == SecurityHelper.CurrentPrincipal.OrgId),
                appHospedResNum = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Count(c => c.ISDELETE == false && c.AGENCYRESULT == (int)EnumAppHospStatus.审核通过 &&
                c.NSID == SecurityHelper.CurrentPrincipal.OrgId),
                appCertNoResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Count(c => c.ISDELETE == false && c.STATUS == (int)EnumAppCertStatus.审核不通过 &&
                c.NSID == SecurityHelper.CurrentPrincipal.OrgId),
            };
        }
        public object GetHlxSbaoAppcertStatistics()
        {
            int Year = DateTime.Now.Year;
            List<object> listY = new List<object>();
            for (int i = 0; i < 5; i++)
            {
                var year = Year--;
                listY.Add(new
                {
                    year = year,
                    sDate = DateTime.Parse(year + "-01-01 00:00:00"),
                    eDate = DateTime.Parse(year + "-01-01 00:00:00").AddYears(1).AddSeconds(-1)
                });
            }
            var orgId = SecurityHelper.CurrentPrincipal.OrgId;
            return listY.OrderBy(o => Util.AnonymousTypeCast(o, new { year = int.MinValue, sDate = DateTime.MinValue, eDate = DateTime.MinValue }).year).Select(s =>
            {
                var at = Util.AnonymousTypeCast(s, new { year = int.MinValue, sDate = DateTime.MinValue, eDate = DateTime.MinValue });
                var year = at.year;
                var sDate = at.sDate;
                var eDate = at.eDate;
                return new
                {
                    year = year,
                    applyResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == orgId &&
                        w.ISDELETE == false && w.NSOPERATETIME >= sDate
                        && w.NSOPERATETIME <= eDate)
                        .Select(s1 => s1.IDNO).Distinct().Count(),
                    passResNum = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == orgId && w.ISDELETE == false &&
                        w.STATUS == (int)EnumAppCertStatus.审核通过 && w.CERTSTARTTIME >= sDate
                        && w.CERTSTARTTIME <= eDate)
                        .Count(),
                };
            }).ToList();
        }
    }
}
