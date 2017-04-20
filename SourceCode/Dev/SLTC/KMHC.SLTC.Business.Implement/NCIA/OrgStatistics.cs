using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIA
{
    public class OrgStatistics : BaseService, IOrgStatistics
    {
        public object GetAppcertRate(string starttime, string endtime)
        {
            var q = from c in unitOfWork.GetRepository<NCIA_NSAPPCERTRATE>().dbSet
                    join h in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on c.NSID equals h.NSID
                    select new
                    {
                        NSNAME = h.NSNAME,
                        PASSRATE = c.PASSRATE,
                        c.PASSPEOPLE,
                        c.TOTALPEOPLE,
                        c.YEAR
                    };
            if (!string.IsNullOrEmpty(starttime))
            {
                q = q.Where(m => string.Compare(m.YEAR, starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                q = q.Where(m => string.Compare(m.YEAR, endtime) <= 0);
            }
            int Year = DateTime.Now.Year;
            List<string> listY = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                listY.Add((Year--).ToString());
            }
            return unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Select(s => s.NSNAME).ToList().Select(s => new
            {
                NSNAME = s,
                DATA = listY.OrderBy(o => o).Select(y => new
                {
                    PASSRATE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).PASSRATE,
                    PASSPEOPLE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).PASSPEOPLE,
                    TOTALPEOPLE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).TOTALPEOPLE,
                    YEAR = y
                }),
            }).ToList();
        }
        public object GetcertRate(string NSID)
        {
            var q = (from c in unitOfWork.GetRepository<NCIA_NSAPPCERTRATE>().dbSet
                     join h in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on c.NSID equals h.NSID
                     select new
                     {
                         NSNAME = h.NSNAME,
                         PASSRATE = c.PASSRATE,
                         c.PASSPEOPLE,
                         c.TOTALPEOPLE,
                         c.YEAR,
                         c.NSID
                     }).ToList();
            int Year = DateTime.Now.Year;
            List<string> listY = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                listY.Add((Year--).ToString());
            }
            return listY.OrderBy(o => o).Select(y => new
            {
                PASSRATEAVG = q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE) == 0 ? 0 : decimal.Round(decimal.Parse((q.Where(w => w.YEAR == y).Sum(s => s.PASSPEOPLE) * 1.0 * 100 / q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE)).ToString()), 2),
                PASSPEOPLEAVG = q.Where(w => w.YEAR == y).Sum(s => s.PASSPEOPLE),
                TOTALPEOPLEAVG = q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE),
                PASSRATE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).PASSRATE,
                PASSPEOPLE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).PASSPEOPLE,
                TOTALPEOPLE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).TOTALPEOPLE,
                YEAR = y
            });
        }
        public object GetApphospRate(string starttime, string endtime)
        {
            var q = from c in unitOfWork.GetRepository<NCIA_NSAPPHOSPRATE>().dbSet
                    join h in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on c.NSID equals h.NSID
                    select new
                    {
                        NSNAME = h.NSNAME,
                        PASSRATE = c.PASSRATE,
                        c.PASSPEOPLE,
                        c.TOTALPEOPLE,
                        c.YEAR
                    };
            if (!string.IsNullOrEmpty(starttime))
            {
                q = q.Where(m => string.Compare(m.YEAR, starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                q = q.Where(m => string.Compare(m.YEAR, endtime) <= 0);
            }
            int Year = DateTime.Now.Year;
            List<string> listY = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                listY.Add((Year--).ToString());
            }
            return unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet.Select(s => s.NSNAME).ToList().Select(s => new
            {
                NSNAME = s,
                DATA = listY.OrderBy(o => o).Select(y => new
                {
                    PASSRATE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).PASSRATE,
                    PASSPEOPLE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).PASSPEOPLE,
                    TOTALPEOPLE = q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y) == null ? 0 : q.FirstOrDefault(f => f.NSNAME == s && f.YEAR == y).TOTALPEOPLE,
                    YEAR = y
                }),
            }).ToList();
        }
        public object GethospRate(string NSID)
        {
            var q = (from c in unitOfWork.GetRepository<NCIA_NSAPPHOSPRATE>().dbSet
                     join h in unitOfWork.GetRepository<NCI_NURSINGHOME>().dbSet on c.NSID equals h.NSID
                     select new
                     {
                         NSNAME = h.NSNAME,
                         PASSRATE = c.PASSRATE,
                         c.PASSPEOPLE,
                         c.TOTALPEOPLE,
                         c.YEAR,
                         c.NSID
                     }).ToList();
            int Year = DateTime.Now.Year;
            List<string> listY = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                listY.Add((Year--).ToString());
            }
            return listY.OrderBy(o => o).Select(y => new
            {
                PASSRATEAVG = q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE) == 0 ? 0 : decimal.Round(decimal.Parse((q.Where(w => w.YEAR == y).Sum(s => s.PASSPEOPLE) * 1.0 * 100 / q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE)).ToString()), 2),
                PASSPEOPLEAVG = q.Where(w => w.YEAR == y).Sum(s => s.PASSPEOPLE),
                TOTALPEOPLEAVG = q.Where(w => w.YEAR == y).Sum(s => s.TOTALPEOPLE),
                PASSRATE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).PASSRATE,
                PASSPEOPLE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).PASSPEOPLE,
                TOTALPEOPLE = q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID) == null ? 0 : q.FirstOrDefault(f => f.YEAR == y && f.NSID == NSID).TOTALPEOPLE,
                YEAR = y
            });
        }
        public object PutAppcertRate(string NSID, int? YEAR = null)
        {
            YEAR = YEAR ?? DateTime.Now.Year;
            var list = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == NSID && w.NSOPERATETIME.Year == YEAR && w.ISDELETE == false).Select(s => new
            {
                s.STATUS,
                s.IDNO
            }).ToList();
            int pCount = list.Where(w => w.STATUS == (int)EnumAppCertStatus.审核通过).Count();
            int tCount = list.Select(s => s.IDNO).Distinct().Count();
            BaseResponse<AppcertRate> res = new BaseResponse<AppcertRate>();
            int retry = 10;//如果出现更新的并发冲突，尝试一定次数
            do
            {
                try
                {
                    BaseResponse<AppcertRate> appcertrate = base.Get<NCIA_NSAPPCERTRATE, AppcertRate>((q) => q.NSID == NSID && q.YEAR == YEAR.ToString());
                    if (appcertrate.Data != null)
                    {
                        appcertrate.Data.PASSPEOPLE = pCount;
                        appcertrate.Data.TOTALPEOPLE = tCount;
                        appcertrate.Data.PASSRATE = tCount == 0
                            ? 0
                            : decimal.Parse((pCount * 1.0 * 100 / tCount).ToString());
                        appcertrate.Data.UPDATETIME = DateTime.Now;
                        return base.Save<NCIA_NSAPPCERTRATE, AppcertRate>(appcertrate.Data, (q) =>
                            q.ID == appcertrate.Data.ID);

                    }
                    else
                    {
                        return base.Save<NCIA_NSAPPCERTRATE, AppcertRate>(new AppcertRate
                        {
                            NSID = NSID,
                            YEAR = YEAR.ToString(),
                            PASSRATE = tCount == 0 ? 0 : decimal.Parse((pCount * 1.0 * 100 / tCount).ToString()),
                            PASSPEOPLE = pCount,
                            TOTALPEOPLE = tCount,
                            UPDATETIME = DateTime.Now
                        }, (q) =>
                            false);
                    }
                }
                catch (Exception)
                {

                }
            }
            while (--retry > 0);
            return res;
        }
        public object PutApphospRate(string NSID, int? YEAR = null)
        {
            YEAR = YEAR ?? DateTime.Now.Year;
            var list = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(w => w.NSID == NSID && w.NSOPERATETIME.Value.Year == YEAR && w.ISDELETE == false).Select(s => new
            {
                s.AGENCYRESULT,
                s.IDNO
            }).ToList();
            int pCount = list.Where(w => w.AGENCYRESULT == (int)EnumAppCertStatus.审核通过).Count();
            int tCount = list.Select(s => s.IDNO).Distinct().Count();
            BaseResponse<AppcertRate> res = new BaseResponse<AppcertRate>();
            int retry = 10;//如果出现更新的并发冲突，尝试一定次数
            do
            {
                try
                {
                    BaseResponse<ApphospRate> appcertrate = base.Get<NCIA_NSAPPHOSPRATE, ApphospRate>((q) => q.NSID == NSID && q.YEAR == YEAR.ToString());
                    if (appcertrate.Data != null)
                    {
                        appcertrate.Data.PASSPEOPLE = pCount;
                        appcertrate.Data.TOTALPEOPLE = tCount;
                        appcertrate.Data.PASSRATE = tCount == 0
                            ? 0
                            : decimal.Parse((pCount * 1.0 * 100 / tCount).ToString());
                        appcertrate.Data.UPDATETIME = DateTime.Now;
                        return base.Save<NCIA_NSAPPHOSPRATE, ApphospRate>(appcertrate.Data, (q) =>
                            q.ID == appcertrate.Data.ID);

                    }
                    else
                    {
                        return base.Save<NCIA_NSAPPHOSPRATE, ApphospRate>(new ApphospRate
                        {
                            NSID = NSID,
                            YEAR = YEAR.ToString(),
                            PASSRATE = tCount == 0 ? 0 : decimal.Parse((pCount * 1.0 * 100 / tCount).ToString()),
                            PASSPEOPLE = pCount,
                            TOTALPEOPLE = tCount,
                            UPDATETIME = DateTime.Now
                        }, (q) =>
                            false);
                    }
                }
                catch (Exception)
                {

                }
            }
            while (--retry > 0);
            return res;
        }
        public object GetAppcert(string NSID, int YEAR)
        {
            var list = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(w => w.NSID == NSID && w.NSOPERATETIME.Year == YEAR && w.ISDELETE == false).Select(s => new
            {
                s.STATUS,
                s.IDNO
            }).ToList();
            var dic = EnumHelper.getEnumDic<EnumAppCertStatus>();
            return dic.Select(s => new
            {
                name = s.Key,
                value = list.Where(w => w.STATUS == s.Value).Select(l => l.IDNO).Distinct().Count()
            });
        }
        public object GetApphosp(string NSID, int YEAR)
        {
            var list = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(w => w.NSID == NSID && w.NSOPERATETIME.Value.Year == YEAR && w.ISDELETE == false).Select(s => new
            {
                s.AGENCYRESULT,
                s.IDNO
            }).ToList();
            var dic = EnumHelper.getEnumDic<EnumAppHospStatus>();
            return dic.Select(s => new
            {
                name = s.Key,
                value = list.Where(w => w.AGENCYRESULT == s.Value).Select(l => l.IDNO).Distinct().Count()
            });
        }
        public object GetMonthFeeStatistics(string sDate, string eDate, string nsId)
        {
            var q = unitOfWork.GetRepository<NCIP_NSMONFEE>().dbSet.Where(w => w.ISDELETE == false).Select(s => s);
            if (!string.IsNullOrEmpty(sDate))
            {
                q = q.Where(m => string.Compare(m.YEARMONTH, sDate) >= 0);
            }
            if (!string.IsNullOrEmpty(eDate))
            {
                q = q.Where(m => string.Compare(m.YEARMONTH, eDate) <= 0);
            }
            if (!string.IsNullOrEmpty(nsId))
            {
                q = q.Where(m =>m.NSID==nsId);
            }
            DateTime dt1 = DateTime.Parse(sDate + "-01");
            DateTime dt2 = DateTime.Parse(eDate + "-01");
            List<string> yMonList = new List<string>();
            for (; dt1 <= dt2; dt1 = dt1.AddMonths(1))
            {
                yMonList.Add(dt1.ToString("yyyy-MM"));
            }
            var list= q.GroupBy(g => g.YEARMONTH).Select(s => new { 
               tAmount=s.Sum(u=>u.TOTALAMOUNT),
               tNciPay=s.Sum(u=>u.TOTALNCIPAY),
               tRes=s.Sum(u=>u.TOTALRESIDENT),
               yearMonth=s.Key
            }).ToList();
            List<MonthFeeStatistics> objList = new List<MonthFeeStatistics>();
            yMonList.ForEach(f => {
                objList.Add(new MonthFeeStatistics
                {
                    yearMonth=f,
                    tAmount = list.FirstOrDefault(o => o.yearMonth == f) == null ? null : (decimal?)list.FirstOrDefault(o => o.yearMonth == f).tAmount,
                    tNciPay = list.FirstOrDefault(o => o.yearMonth == f) == null ? null : (decimal?)list.FirstOrDefault(o => o.yearMonth == f).tNciPay,
                    tRes = list.FirstOrDefault(o => o.yearMonth == f) == null ? null : (int?)list.FirstOrDefault(o => o.yearMonth == f).tRes,
                });
            });
            return objList;
        }
    }
}
