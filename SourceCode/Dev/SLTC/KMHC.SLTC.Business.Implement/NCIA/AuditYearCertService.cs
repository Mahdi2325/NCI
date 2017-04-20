using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.NCIA
{
    public class AuditYearCertService : BaseService, IAuditYearCertService
    {
        IMonFeeService monfeeService = IOCContainer.Instance.Resolve<IMonFeeService>();

        #region 资格年审old数据
        public BaseResponse<IList<AuditYearCertModel>> GetYearCertlist(BaseRequest<AuditYearCertFilter> request)
        {
            var response = new BaseResponse<IList<AuditYearCertModel>>();
            var odt = new DateTime(1, 1, 1);
            var q = from a in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.ISDELETE != true && m.CERTNO != null)
                    join n in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE != true && m.ENTRYTIME != null) on a.APPCERTID equals n.APPCERTID into nrd
                    join c in unitOfWork.GetRepository<NCI_CARETYPE>().dbSet on a.AGENCYAPPROVEDCARETYPE equals c.CARETYPEID into nct
                    from nr in nrd.DefaultIfEmpty()
                    from nc in nct.DefaultIfEmpty()
                    select new AuditYearCertModel
                    {
                        CertAppcertid = a.APPCERTID,
                        HospAppcertid = nr.APPCERTID,
                        AppHospid = nr.APPHOSPID,
                        Name = nr.NAME,
                        Gender = nr.GENDER,
                        Age = nr.AGE,
                        SsNo = nr.SSNO,
                        IdNo = nr.IDNO,
                        NsID = nr.NSID,
                        Phone = nr.PHONE,
                        CertStatus = a.STATUS,
                        HospStatus = (int)nr.AGENCYRESULT,
                        Entrytime = nr.ENTRYTIME.Value,
                        CertNo = a.CERTNO,
                        Certstarttime = a.CERTSTARTTIME.Value,
                        Certexpiredtime = a.CERTEXPIREDTIME.Value,
                        Caretypeid = (int)a.AGENCYAPPROVEDCARETYPE,
                        NCIpaylevel = nc.NCIPAYLEVEL,
                        NCIpayscale = nc.NCIPAYSCALE,
                        UpdateTime = a.UPDATETIME,
                    };
            var s = unitOfWork.GetRepository<NCI_SUPERVISE>().dbSet.Where(m => m.VALID != false).ToList();
            q = q.Where(m => m.CertStatus == m.HospStatus && m.CertNo != "" && m.Entrytime != odt);

            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name));
            }

            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo.ToUpper().Trim() == request.Data.IdNo.ToUpper().Trim() || m.SsNo == request.Data.IdNo);
            }
            if (request.Data.NsID != "-1")
            {
                q = q.Where(m => m.NsID.ToUpper().Trim() == request.Data.NsID.ToUpper().Trim());
            }
            if (request.Data.Status == "Y") //启用中
            {
                q = q.Where(m => m.HospStatus == (int)ApplicationEnum.ExaminationPassed && m.CertStatus == (int)ApplicationEnum.ExaminationPassed);
            }
            else if (request.Data.Status == "N") //停用中
            {
                q = q.Where(m => m.CertStatus == (int)ApplicationEnum.AuditNotPassed && m.CertStatus == (int)ApplicationEnum.AuditNotPassed);

            }

            q = q.OrderByDescending(m => m.Entrytime);

            var list = q.ToList();
            list = list.Distinct(new PersonNumComparer()).ToList();

            response.RecordsCount = list.Count();
            if (request.PageSize > 0)
            {
                response.Data = list.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }

            return response;
        }

        public class PersonNumComparer : IEqualityComparer<AuditYearCertModel>
        {
            public bool Equals(AuditYearCertModel p1, AuditYearCertModel p2)
            {
                if (p1 == null)
                    return p2 == null;
                return p1.IdNo == p2.IdNo;
            }

            public int GetHashCode(AuditYearCertModel p)
            {
                if (p == null)
                    return 0;
                return p.IdNo.GetHashCode();
            }
        }

        public BaseResponse UpdateYearCert(AuditYearCertModel request)
        {
            var response = new BaseResponse();
            if (request.HospStatus == 6 && request.CertStatus == 6)  //  强制修改护理险资格为无效
            {
                var appcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(x => x.CERTNO == request.CertNo && x.APPCERTID == request.CertAppcertid && x.ISDELETE != true).FirstOrDefault();
                appcert.STATUS = 9;
                appcert.UPDATETIME = DateTime.Now;
                unitOfWork.GetRepository<NCIA_APPCERT>().Update(appcert);

                var appHosp = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(x => x.ENTRYTIME == request.Entrytime && x.APPHOSPID == request.AppHospid && x.ISDELETE != true).FirstOrDefault();
                appHosp.UPDATETIME = DateTime.Now;
                appHosp.AGENCYRESULT = 9;
                unitOfWork.GetRepository<NCIA_APPHOSP>().Update(appHosp);

                var nursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO == request.IdNo && m.ISDELETE != true);
                nursingHomeAsstRecord.LASTCERTRESULT = (int)ApplicationEnum.AuditNotPassed;
                nursingHomeAsstRecord.LASTHOSPRESULT = (int)ApplicationEnum.AuditNotPassed;
                unitOfWork.GetRepository<NCIA_APPLICANT>().Update(nursingHomeAsstRecord);

                var supervise = unitOfWork.GetRepository<NCI_SUPERVISE>().dbSet.Where(x => x.APPCERTID == request.CertAppcertid && appHosp.APPHOSPID == request.AppHospid && x.VALID == true).FirstOrDefault();
                if (supervise != null)
                {
                    supervise.VALID = false;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Update(supervise);
                    NCI_SUPERVISE model = new NCI_SUPERVISE();
                    model.APPCERTID = request.CertAppcertid;
                    model.APPHOSPID = request.AppHospid;
                    model.REASON = request.Reason;
                    model.STATUS = 1;
                    model.VALID = true;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CREATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
                }
                else
                {
                    NCI_SUPERVISE model = new NCI_SUPERVISE();
                    model.APPCERTID = request.CertAppcertid;
                    model.APPHOSPID = request.AppHospid;
                    model.REASON = request.Reason;
                    model.STATUS = 1;
                    model.VALID = true;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CREATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
                }

            }
            else if (request.CertStatus == 9 && request.HospStatus == 9)
            {
                var appcert = unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(x => x.CERTNO == request.CertNo && x.APPCERTID == request.CertAppcertid && x.ISDELETE != true).FirstOrDefault();
                appcert.UPDATETIME = DateTime.Now;
                appcert.STATUS = 6;
                unitOfWork.GetRepository<NCIA_APPCERT>().Update(appcert);

                var appHosp = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(x => x.ENTRYTIME == request.Entrytime && x.APPHOSPID == request.AppHospid && x.ISDELETE != true).FirstOrDefault();
                appHosp.UPDATETIME = DateTime.Now;
                appHosp.AGENCYRESULT = 6;
                unitOfWork.GetRepository<NCIA_APPHOSP>().Update(appHosp);
                var nursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO == request.IdNo && m.ISDELETE != true);
                nursingHomeAsstRecord.LASTCERTRESULT = (int)ApplicationEnum.ExaminationPassed;
                nursingHomeAsstRecord.LASTHOSPRESULT = (int)ApplicationEnum.ExaminationPassed;
                unitOfWork.GetRepository<NCIA_APPLICANT>().Update(nursingHomeAsstRecord);

                var supervise = unitOfWork.GetRepository<NCI_SUPERVISE>().dbSet.Where(x => x.APPCERTID == request.CertAppcertid && appHosp.APPHOSPID == request.AppHospid && x.VALID == true).FirstOrDefault();
                if (supervise != null)
                {
                    supervise.VALID = false;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Update(supervise);

                    NCI_SUPERVISE model = new NCI_SUPERVISE();
                    model.APPCERTID = request.CertAppcertid;
                    model.APPHOSPID = request.AppHospid;
                    model.REASON = request.Reason;
                    model.STATUS = 0;
                    model.VALID = true;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CREATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
                }
                else
                {
                    NCI_SUPERVISE model = new NCI_SUPERVISE();
                    model.APPCERTID = request.CertAppcertid;
                    model.APPHOSPID = request.AppHospid;
                    model.REASON = request.Reason;
                    model.STATUS = 0;
                    model.VALID = true;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CREATETIME = DateTime.Now;
                    unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
                }
            }

            unitOfWork.Save();
            return response;
        }
        #endregion

        #region 结案作业数据
        public BaseResponse UpdateAppHospCertInfo(RegNCIInfo request)
        {
            var response = new BaseResponse();
            var caretype = Convert.ToInt16(request.Caretypeid);
            var appHosp = unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(x => x.ENTRYTIME == request.ApplyHosTime && x.ISDELETE != true && x.IDNO.ToUpper().Trim() == request.IdNo.ToUpper().Trim() && x.AGENCYRESULT == (int)ApplicationEnum.ExaminationPassed && x.CARETYPE == caretype).FirstOrDefault();
            appHosp.ISDELETE = true;
            appHosp.UPDATEBY = "结案作业";
            appHosp.UPDATETIME = DateTime.Now;
            appHosp.LEAVETIME = request.ipdoutTime;
            unitOfWork.GetRepository<NCIA_APPHOSP>().Update(appHosp);

            var nursingHomeAsstRecord = unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.FirstOrDefault(m => m.IDNO.ToUpper().Trim() == request.IdNo.ToUpper().Trim() && m.ISDELETE != true);
            nursingHomeAsstRecord.LASTHOSPRESULT = (int)ApplicationEnum.AuditNotPassed;
            unitOfWork.GetRepository<NCIA_APPLICANT>().Update(nursingHomeAsstRecord);

            var supervise = unitOfWork.GetRepository<NCI_SUPERVISE>().dbSet.Where(x => x.APPCERTID == appHosp.APPCERTID && appHosp.APPHOSPID == appHosp.APPHOSPID && x.VALID == true).FirstOrDefault();
            if (supervise != null)
            {
                supervise.VALID = false;
                unitOfWork.GetRepository<NCI_SUPERVISE>().Update(supervise);
                NCI_SUPERVISE model = new NCI_SUPERVISE();
                model.APPCERTID = appHosp.APPCERTID;
                model.APPHOSPID = appHosp.APPHOSPID;
                model.REASON = "结案作业取消资格";
                model.STATUS = 1;
                model.VALID = true;
                model.CREATEBY = "API";
                model.CREATETIME = DateTime.Now;
                unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
            }
            else
            {
                NCI_SUPERVISE model = new NCI_SUPERVISE();
                model.APPCERTID = appHosp.APPCERTID;
                model.APPHOSPID = appHosp.APPHOSPID;
                model.REASON = "结案作业取消资格";
                model.STATUS = 1;
                model.VALID = true;
                model.CREATEBY = "API";
                model.CREATETIME = DateTime.Now;
                unitOfWork.GetRepository<NCI_SUPERVISE>().Insert(model);
            }
            unitOfWork.Save();
            return response;
        }
        #endregion

        public BaseResponse<IList<AuditYearCertModel>> GetAduitYearCertList(BaseRequest<AuditYearCertFilter> request)
        {
            var response = new BaseResponse<IList<AuditYearCertModel>>();
            var q = from a in unitOfWork.GetRepository<NCIA_APPCERT>().dbSet.Where(m => m.ISDELETE != true && (m.STATUS == (int)ApplicationEnum.ExaminationPassed || m.STATUS == (int)ApplicationEnum.ApplyForEndCert))
                    join n in unitOfWork.GetRepository<NCI_SUPERVISE>().dbSet on a.APPCERTID equals n.APPCERTID into sup
                    join c in unitOfWork.GetRepository<NCIA_APPLICANT>().dbSet.Where(m => m.ISDELETE != true) on a.APPLICANTID equals c.APPLICANTID into alc
                    join h in unitOfWork.GetRepository<NCIA_APPHOSP>().dbSet.Where(m => m.ISDELETE != true) on a.APPCERTID equals h.APPCERTID into hosp
                    from sv in sup.DefaultIfEmpty()
                    from ac in alc.DefaultIfEmpty()
                    from hp in hosp.DefaultIfEmpty()
                    select new AuditYearCertModel
                    {
                        Name = ac.NAME,
                        Gender = a.GENDER,
                        Age = a.AGE,
                        IdNo = ac.IDNO,
                        SsNo = ac.IDNO,
                        NsID = hp.NSID,
                        AppHospid = hp.APPHOSPID,
                        //Type = sv.TYPE,
                        Status = sv.STATUS,
                        Valid = sv.VALID,
                        CreateTime = sv.CREATETIME,
                    };

            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name));
            }

            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo.ToUpper().Trim() == request.Data.IdNo.ToUpper().Trim() || m.SsNo.ToUpper().Trim() == request.Data.IdNo.ToUpper().Trim());
            }

            if (request.Data.NsID != "-1")
            {
                q = q.Where(m => m.NsID.ToUpper().Trim() == request.Data.NsID.ToUpper().Trim());
            }

            var list = q.OrderByDescending(m => m.CreateTime).ToList();

            var onlist = list.Where(m => m.Type == 1 && m.Status == 0 && m.Valid.HasValue && m.Valid.Value == true).ToList();
            var stoplist = list.Where(m => m.Type == 1 && m.Status == 0 && m.Valid.HasValue && m.Valid.Value == false).ToList();
            var cancellist = list.Where(m => m.Type == 0 && m.Status == 0 && m.Valid.HasValue && m.Valid.Value == false).ToList();
            if (onlist != null && onlist.Count > 0)
            {
                foreach (var l in onlist)
                {
                    l.AduitYearStatus = "使用中";
                    l.OrgName = monfeeService.QueryOrgNsHomeByID(l.NsID) == null ? "" : monfeeService.QueryOrgNsHomeByID(l.NsID).NsName;
                }
            }

            if (stoplist != null && stoplist.Count > 0)
            {
                foreach (var s in stoplist)
                {
                    s.AduitYearStatus = "暂停中";
                    s.OrgName = monfeeService.QueryOrgNsHomeByID(s.NsID) == null ? "" : monfeeService.QueryOrgNsHomeByID(s.NsID).NsName;
                }
            }

            if (cancellist != null && cancellist.Count > 0)
            {
                foreach (var c in cancellist)
                {
                    c.AduitYearStatus = "已停用";
                    c.OrgName = monfeeService.QueryOrgNsHomeByID(c.NsID) == null ? "" : monfeeService.QueryOrgNsHomeByID(c.NsID).NsName;
                }
            }

            if (request.Data.Status == "Y") // 使用中
            {
                list = onlist;
            }
            else if (request.Data.Status == "N") //暂停资格
            {
                list = stoplist;
            }
            else if (request.Data.Status == "E") // 取消资格
            {
                list = cancellist;
            }
            else
            {
                list = onlist.Union(stoplist).Union(cancellist).ToList();
            }

            response.RecordsCount = list.Count();
            if (request.PageSize > 0)
            {
                response.Data = list.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }
    }
}
