/*
创建人: 肖国栋
创建日期:2016-03-09
说明:护理工作站
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using KMHC.SLTC.Business.Entity.DC.Model;

namespace KMHC.SLTC.Business.Implement
{
    public class NursingWorkstationService : BaseService, INursingWorkstationService
    {
        #region 生命体征
        public BaseResponse<IList<Vitalsign>> QueryVitalsign(BaseRequest<VitalsignFilter> request)
        {
            var response = base.Query<LTC_VITALSIGN, Vitalsign>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderByDescending(m => m.SEQNO);
                return q;
            });
            return response;
        }

        public BaseResponse<Vitalsign> GetVitalsign(long seqNO)
        {
            return base.Get<LTC_VITALSIGN, Vitalsign>((q) => q.SEQNO == seqNO);
        }
        public BaseResponse<Vitalsign> GetVitalsignToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_VITALSIGN, Vitalsign>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECORDDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<Vitalsign> SaveVitalsign(Vitalsign request)
        {
            if (request.SeqNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_VITALSIGN, Vitalsign>(request, (q) => q.SEQNO == request.SeqNo);
        }

        public BaseResponse<List<Vitalsign>> SaveVitalsign(List<Vitalsign> request)
        {
            BaseResponse<List<Vitalsign>> response = new BaseResponse<List<Vitalsign>>();
            Mapper.CreateMap<Vitalsign, LTC_VITALSIGN>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_VITALSIGN>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                if (model.SEQNO > 0)
                {
                    unitOfWork.GetRepository<LTC_VITALSIGN>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_VITALSIGN>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteVitalsign(long seqNO)
        {
            return base.Delete<LTC_VITALSIGN>(seqNO);
        }
        #endregion

        #region 輸出量
        public BaseResponse<IList<OutValueModel>> QueryOutValue(BaseRequest<OutValueFilter> request)
        {
            BaseResponse<IList<OutValueModel>> response = new BaseResponse<IList<OutValueModel>>();
            var q = from n in unitOfWork.GetRepository<LTC_OUTVALUE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        OutValueModel = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.OutValueModel.FEENO == request.Data.FeeNo);
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OutValueModel.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.OutValueModel.OUTNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<OutValueModel>();
                foreach (dynamic item in list)
                {
                    OutValueModel newItem = Mapper.DynamicMap<OutValueModel>(item.OutValueModel);
                    newItem.RecordNameBy = item.EmpName;
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

        public BaseResponse<OutValueModel> GetOutValue(long outNo)
        {
            return base.Get<LTC_OUTVALUE, OutValueModel>((q) => q.OUTNO == outNo);
        }
        public BaseResponse<OutValueModel> GetOutValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_OUTVALUE, OutValueModel>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<OutValueModel> SaveOutValue(OutValueModel request)
        {
            if (request.OutNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_OUTVALUE, OutValueModel>(request, (q) => q.OUTNO == request.OutNo);
        }

        public BaseResponse<List<OutValueModel>> SaveOutValue(List<OutValueModel> request)
        {
            BaseResponse<List<OutValueModel>> response = new BaseResponse<List<OutValueModel>>();
            DateTime nowTime = DateTime.Now;
            Mapper.CreateMap<OutValueModel, LTC_OUTVALUE>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_OUTVALUE>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                model.UPDATEDATE = nowTime;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                if (model.OUTNO > 0)
                {
                    unitOfWork.GetRepository<LTC_OUTVALUE>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_OUTVALUE>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteOutValue(long outNo)
        {
            return base.Delete<LTC_OUTVALUE>(outNo);
        }

        #endregion

        #region 輸入量
        public BaseResponse<IList<InValueModel>> QueryInValue(BaseRequest<InValueFilter> request)
        {
            BaseResponse<IList<InValueModel>> response = new BaseResponse<IList<InValueModel>>();
            var q = from n in unitOfWork.GetRepository<LTC_INVALUE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        InValueModel = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.InValueModel.FEENO == request.Data.FeeNo);
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.InValueModel.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.InValueModel.INNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<InValueModel>();
                foreach (dynamic item in list)
                {
                    InValueModel newItem = Mapper.DynamicMap<InValueModel>(item.InValueModel);
                    newItem.RecordNameBy = item.EmpName;
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

        public BaseResponse<InValueModel> GetInValue(long inNo)
        {
            return base.Get<LTC_INVALUE, InValueModel>((q) => q.INNO == inNo);
        }
        public BaseResponse<InValueModel> GetInValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_INVALUE, InValueModel>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<InValueModel> SaveInValue(InValueModel request)
        {
            if (request.InNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_INVALUE, InValueModel>(request, (q) => q.INNO == request.InNo);
        }

        public BaseResponse<List<InValueModel>> SaveInValue(List<InValueModel> request)
        {
            BaseResponse<List<InValueModel>> response = new BaseResponse<List<InValueModel>>();
            DateTime nowTime = DateTime.Now;
            Mapper.CreateMap<InValueModel, LTC_INVALUE>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_INVALUE>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                model.UPDATEDATE = nowTime;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                if (model.INNO > 0)
                {
                    unitOfWork.GetRepository<LTC_INVALUE>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_INVALUE>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteInValue(long inNo)
        {
            return base.Delete<LTC_INVALUE>(inNo);
        }

        #endregion

        #region 護理記錄
        public BaseResponse<IList<NursingRec>> QueryNursingRec(BaseRequest<NursingRecFilter> request)
        {
            BaseResponse<IList<NursingRec>> response = new BaseResponse<IList<NursingRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_NURSINGREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        NursingRec = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.FEENO == request.Data.FeeNo);
            }
            if (request.Data.RegNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.REGNO == request.Data.RegNo);
            }
            q = q.OrderByDescending(m => m.NursingRec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<NursingRec>();
                foreach (dynamic item in list)
                {
                    NursingRec newItem = Mapper.DynamicMap<NursingRec>(item.NursingRec);
                    newItem.RecordNameBy = item.EmpName;
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

        public BaseResponse<NursingRec> GetNursingRec(long id)
        {
            return base.Get<LTC_NURSINGREC, NursingRec>((q) => q.ID == id);
        }

        public BaseResponse<NursingRec> SaveNursingRec(NursingRec request)
        {
            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_NURSINGREC, NursingRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteNursingRec(long id)
        {
            return base.Delete<LTC_NURSINGREC>(id);
        }
        #endregion

        #region 護理交班
        public BaseResponse<IList<NursingHandover>> QueryNursingHandover(BaseRequest<NursingHandoverFilter> request)
        {
            BaseResponse<IList<NursingHandover>> response = new BaseResponse<IList<NursingHandover>>();
            var q = from n in unitOfWork.GetRepository<LTC_NURSINGHANDOVER>().dbSet
                    join e_d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_D equals e_d.EMPNO into re_ds
                    join e_e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_E equals e_e.EMPNO into re_es
                    join e_n in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_N equals e_n.EMPNO into re_ns
                    from re_d in re_ds.DefaultIfEmpty()
                    from re_e in re_es.DefaultIfEmpty()
                    from re_n in re_ns.DefaultIfEmpty()
                    select new
                    {
                        NursingRec = n,
                        Nurse_D = re_d.EMPNAME,
                        Nurse_E = re_e.EMPNAME,
                        Nurse_N = re_n.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.FEENO == request.Data.FeeNo);
            }
            if (request.Data.RegNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.REGNO == request.Data.RegNo);
            }
            q = q.OrderByDescending(m => m.NursingRec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<NursingHandover>();
                foreach (dynamic item in list)
                {
                    NursingHandover newItem = Mapper.DynamicMap<NursingHandover>(item.NursingRec);
                    newItem.Nurse_D = item.Nurse_D;
                    newItem.Nurse_E = item.Nurse_E;
                    newItem.Nurse_N = item.Nurse_N;
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

        public BaseResponse<NursingHandover> GetNursingHandover(long id)
        {
            return base.Get<LTC_NURSINGHANDOVER, NursingHandover>((q) => q.ID == id);
        }
        public BaseResponse<NursingHandover> SaveNursingHandover(NursingHandover request)
        {
       

            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_NURSINGHANDOVER, NursingHandover>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse SaveMulNursingHandover(List<NursingHandover> request)
        {
           

            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    if (p.Id == 0)
                    {
                        p.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                        p.CreateDate = DateTime.Now;
                        p.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    }
                    base.Save<LTC_NURSINGHANDOVER, NursingHandover>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }

        public BaseResponse DeleteNursingHandover(long id)
        {
            return base.Delete<LTC_NURSINGHANDOVER>(id);
        }
        #endregion

        #region 行政交班

        public BaseResponse<IList<AffairsHandover>> QueryAffairsHandover(BaseRequest<AffairsHandoverFilter> request)
        {
            var response = base.Query<LTC_AFFAIRSHANDOVER, AffairsHandover>(request, (q) =>
            {
                if (request != null && !string.IsNullOrEmpty(request.Data.RecorderName))
                {
                    q = q.Where(m => m.RECORDERNAME.Contains(request.Data.RecorderName));
                }
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<AffairsHandover>> QueryAffairsHandoverExtend(BaseRequest<AffairsHandoverFilter> request)
        {
            BaseResponse<IList<AffairsHandover>> response = new BaseResponse<IList<AffairsHandover>>();
            var q = from u in unitOfWork.GetRepository<LTC_AFFAIRSHANDOVER>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on u.RECORDBY equals e.EMPNO into e1
                    from show1 in e1.DefaultIfEmpty()
                    join s in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on u.EXECUTEBY equals s.EMPNO into e2
                    from show2 in e2.DefaultIfEmpty()
                    select new AffairsHandover
                    {
                        Id = u.ID,
                        RecordDate = u.RECORDDATE,
                        RecorderName = show1.EMPNAME,
                        ExecuteDate = u.EXECUTEDATE,
                        ExecutiveName = show2.EMPNAME,
                        FinishFlag = u.FINISHFLAG,
                        Content = u.CONTENT,
                        FinishDate = u.FINISHDATE
                    };

            if (request != null && !string.IsNullOrEmpty(request.Data.RecorderName))
            {
                q = q.Where(m => m.RecorderName.Contains(request.Data.RecorderName));
            }
            q = q.OrderByDescending(m => m.RecordDate);
            response.RecordsCount = q.Count();
            List<AffairsHandover> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            return response;
        }

        public BaseResponse<AffairsHandover> GetAffairsHandover(long id)
        {
            return base.Get<LTC_AFFAIRSHANDOVER, AffairsHandover>((q) => q.ID == id);
        }

        public BaseResponse<AffairsHandover> SaveAffairsHandover(AffairsHandover request)
        {
            return base.Save<LTC_AFFAIRSHANDOVER, AffairsHandover>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse SaveMulAffairsHandover(List<AffairsHandover> request)
        {
          

            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    base.Save<LTC_AFFAIRSHANDOVER, AffairsHandover>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }
        public BaseResponse DeleteAffairsHandover(long id)
        {
            return base.Delete<LTC_AFFAIRSHANDOVER>(id);
        }
        #endregion

        #region 工作照會
        public BaseResponse<IList<AssignTask>> QueryAssignTask(BaseRequest<AssignTaskFilter> request)
        {
            var response = new BaseResponse<IList<AssignTask>>();
            var q = from m in unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on m.FEENO equals ipd.FEENO into ipd_Reg
                    from ipdReg in ipd_Reg.DefaultIfEmpty()
                    join regf in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdReg.REGNO equals regf.REGNO into reg_f
                    from reg_file in reg_f.DefaultIfEmpty()
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on m.ASSIGNEE equals emp.EMPNO into emp_f
                    from emp_file in emp_f.DefaultIfEmpty()
                    select new 
                    {
                        AssignTask = m,
                        ResidentName = reg_file.NAME,
                        EMPNAME= emp_file.EMPNAME
                    };

            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.AssignTask.ORGID == request.Data.OrgId);
            }
            if (request != null && request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.AssignTask.FEENO == request.Data.FeeNo);
            }
            if (request != null && request.Data.RecStatus.HasValue)
            {
                q = q.Where(m => m.AssignTask.RECSTATUS == request.Data.RecStatus);
            }
            if (request != null && request.Data.NewRecFlag.HasValue)
            {
                q = q.Where(m => m.AssignTask.NEWRECFLAG == request.Data.NewRecFlag);
            }

            if (request != null && request.Data.SDate.HasValue && request.Data.EDate.HasValue)
            {
                var endDate = request.Data.EDate.Value.AddDays(1);
                q = q.Where(m => m.AssignTask.PERFORMDATE >= request.Data.SDate && m.AssignTask.PERFORMDATE < endDate);
            }
            else if (request != null && request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.AssignTask.PERFORMDATE >= request.Data.SDate);
            }
            else if (request != null && request.Data.EDate.HasValue)
            {
                var endDate = request.Data.EDate.Value.AddDays(1);
                q = q.Where(m => m.AssignTask.PERFORMDATE < endDate);
            }
            if (request != null && !string.IsNullOrWhiteSpace(request.Data.Assignee))
            {
                q = q.Where(m => m.AssignTask.ASSIGNEE == request.Data.Assignee);
            }
            q = q.OrderByDescending(m => m.AssignTask.NEWRECFLAG).ThenBy(m => m.AssignTask.RECSTATUS).ThenByDescending(m => m.AssignTask.PERFORMDATE).ThenByDescending(m => m.AssignTask.ID);

            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<AssignTask>();
                foreach (dynamic item in list)
                {
                    AssignTask newItem = Mapper.DynamicMap<AssignTask>(item.AssignTask);
                    newItem.ResidentName = item.ResidentName;
                    newItem.EMPNAME = item.EMPNAME;
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
            //var response = base.Query<LTC_ASSIGNTASK, AssignTask>(request, (q) =>
            //{
            //    if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            //    {
            //        q = q.Where(m => m.ORGID == request.Data.OrgId);
            //    }
            //    if (request != null && request.Data.FeeNo.HasValue)
            //    {
            //        q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    }
            //    if (request != null && request.Data.SDate.HasValue && request.Data.EDate.HasValue)
            //    {
            //        var endDate = request.Data.EDate.Value.AddDays(1);
            //        q = q.Where(m => m.ASSIGNDATE >= request.Data.SDate && m.ASSIGNDATE < endDate);
            //    }
            //    else if (request != null && request.Data.SDate.HasValue)
            //    {
            //        q = q.Where(m => m.ASSIGNDATE >= request.Data.SDate);
            //    }
            //    else if (request != null && request.Data.EDate.HasValue)
            //    {
            //        var endDate = request.Data.EDate.Value.AddDays(1);
            //        q = q.Where(m => m.ASSIGNDATE < endDate);
            //    }
            //    if (request != null && !string.IsNullOrWhiteSpace(request.Data.Assignee))
            //    {
            //        q = q.Where(m => m.ASSIGNEE == request.Data.Assignee);
            //    }
            //    q = q.OrderByDescending(m => m.ID);
            //    return q;
            //});
            return response;
        }

        public BaseResponse<AssignTask> GetAssignTask(long id)
        {
            return base.Get<LTC_ASSIGNTASK, AssignTask>((q) => q.ID == id);
        }

        /// <summary>
        /// 更新工作狀態
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="recStatus">recStatus</param>
        /// <param name="finishDate">finishDate</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse ChangeRecStatus(int id, bool? recStatus, DateTime? finishDate, bool? newrecFlag)
        {
            BaseResponse response = new BaseResponse();
            LTC_ASSIGNTASK tr = unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet.Where(x => x.ID == id && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            tr.RECSTATUS = recStatus;
            tr.FINISHDATE = finishDate;
            tr.NEWRECFLAG = newrecFlag;
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Update(tr);
            unitOfWork.Commit();
            return response;
        }
        /// <summary>
        /// 更新未读状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newRecFlag">newRecFlag</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse ChangeNewRecStatus(int id, bool? newRecFlag)
        {
            BaseResponse response = new BaseResponse();
            LTC_ASSIGNTASK tr = unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet.Where(x => x.ID == id && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            tr.NEWRECFLAG = newRecFlag;
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Update(tr);
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse<AssignTask> SaveAssignTask(AssignTask request)
        {
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            request.RecStatus = false;
            request.NewrecFlag = true;
            request.AutoFlag = false;
            return base.Save<LTC_ASSIGNTASK, AssignTask>(request, (q) => q.ID == request.Id);
        }
        public BaseResponse<AssignTask> SaveAssignTask2(AssignTask2 request)
        {
            AssignTask ass = new AssignTask {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                PerformDate=request.NEXTEVALDATE,
                Assignee = request.NEXTEVALUATEBY == null ? SecurityHelper.CurrentPrincipal.EmpNo : request.NEXTEVALUATEBY,
                FeeNo=request.FEENO,
                Content = AutoTaskTmp.AutoTask[request.KEY].Content,
                RecStatus=false,
                NewrecFlag=true,
                AutoFlag=true,
                AssignDate=DateTime.Now
            };
            return base.Save<LTC_ASSIGNTASK, AssignTask>(ass, (q) => (false));
        }
        public BaseResponse SaveAssignTask(List<AssignTask> request)
        {
            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    if (p.Id == 0)
                    {
                        p.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        p.RecStatus = false;
                    }
                    base.Save<LTC_ASSIGNTASK, AssignTask>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }

        public BaseResponse DeleteAssignTask(long id)
        {
            return base.Delete<LTC_ASSIGNTASK>(id);
        }

        public BaseResponse ReAssignTask(AssignTask oldTask, IList<TaskEmpFile> empList)
        {
            BaseResponse response = new BaseResponse();
            //var oldTask = GetAssignTask(id);
            List<LTC_ASSIGNTASK> newTaskList = empList.Select(t => new LTC_ASSIGNTASK
            {
                ID = 0,
                ASSIGNEDBY = SecurityHelper.CurrentPrincipal.EmpNo,
                ASSIGNEE = t.EmpNo,
                ASSIGNNAME = t.EmpName,
                NEWRECFLAG = true,
                ASSIGNDATE = DateTime.Now,
                CONTENT = oldTask.Content,
                FEENO=oldTask.FeeNo,
                ORGID=oldTask.OrgId,
            }).ToList();
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().InsertRange(newTaskList);
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Delete(oldTask.Id);
            unitOfWork.Commit();
            return response;
        }

        #endregion

        #region 醫師評估
        public BaseResponse<IList<DoctorEvalRec>> QueryDocEvalRecData(BaseRequest<DoctorEvalRecFilter> request)
        {
            //var response = base.Query<LTC_DOCTOREVALREC, DoctorEvalRec>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderByDescending(m => m.FEENO);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<DoctorEvalRec>> response = new BaseResponse<IList<DoctorEvalRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_DOCTOREVALREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.DOCNAME equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        DoctorEvalRec = n,
                        EmpName = re.EMPNAME
                    };


            q = q.Where(m => m.DoctorEvalRec.FEENO == request.Data.FeeNo);

            q = q.OrderByDescending(m => m.DoctorEvalRec.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DoctorEvalRec>();
                foreach (dynamic item in list)
                {
                    DoctorEvalRec newItem = Mapper.DynamicMap<DoctorEvalRec>(item.DoctorEvalRec);
                    newItem.DocActName = item.EmpName;
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

        public BaseResponse<DoctorEvalRec> GetDocEvalRecData(long id)
        {
            return base.Get<LTC_DOCTOREVALREC, DoctorEvalRec>((q) => q.ID == id);
        }

        public BaseResponse<DoctorEvalRec> SaveDocEvalRecData(DoctorEvalRec request)
        {
            //request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo; 
            request.CreateBy = "1";
            request.CreateDate = DateTime.Now;
            return base.Save<LTC_DOCTOREVALREC, DoctorEvalRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDocEvalRecData(long id)
        {
            return base.Delete<LTC_DOCTOREVALREC>(id);
        }
        #endregion

        #region 醫師巡診
        public BaseResponse<IList<DoctorCheckRec>> QueryDocCheckRecData(BaseRequest<DoctorCheckRecFilter> request)
        {
            //var response = base.Query<LTC_DOCTORCHECKREC, DoctorCheckRec>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderByDescending(m => m.ID);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<DoctorCheckRec>> response = new BaseResponse<IList<DoctorCheckRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_DOCTORCHECKREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.DOCNO equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        DoctorCheckRec = n,
                        EmpName = re.EMPNAME
                    };


            q = q.Where(m => m.DoctorCheckRec.FEENO == request.Data.FeeNo);

            q = q.OrderByDescending(m => m.DoctorCheckRec.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DoctorCheckRec>();
                foreach (dynamic item in list)
                {
                    DoctorCheckRec newItem = Mapper.DynamicMap<DoctorCheckRec>(item.DoctorCheckRec);
                    newItem.DocName = item.EmpName;
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

        public BaseResponse<DoctorCheckRec> GetDocCheckRecData(long id)
        {
            return base.Get<LTC_DOCTORCHECKREC, DoctorCheckRec>((q) => q.ID == id);
        }

        public BaseResponse<DoctorCheckRec> SaveDocCheckRecData(DoctorCheckRec request)
        {

            //request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo; 

            return base.Save<LTC_DOCTORCHECKREC, DoctorCheckRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDocCheckRecData(long id)
        {
            return base.Delete<LTC_DOCTORCHECKREC>(id);
        }
        #endregion

        #region 用藥記錄
        public BaseResponse<IList<VisitPrescription>> QueryVisitPrescription(BaseRequest<VisitPrescriptionFilter> request)
        {
            BaseResponse<IList<VisitPrescription>> response = new BaseResponse<IList<VisitPrescription>>();
            var q = from it in unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet
                    join n in unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet on it.SEQNO equals n.SEQNO into nns
                    from nn in nns.DefaultIfEmpty()
                    join m in unitOfWork.GetRepository<LTC_MEDICINE>().dbSet on it.MEDID equals m.MEDID into mms
                    from mm in mms.DefaultIfEmpty()
                    join e2 in unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet on nn.VISITHOSP equals e2.HOSPNO into vr_e2
                    from vr_emp2 in vr_e2.DefaultIfEmpty()
                    join e3 in unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet on nn.VISITDEPT equals e3.DEPTNO into vr_e3
                    from vr_emp3 in vr_e3.DefaultIfEmpty()
                    join e4 in unitOfWork.GetRepository<LTC_VISITDOCTOR>().dbSet on nn.VISITDOCTOR equals e4.DEPTNO into vr_e4
                    from vr_emp4 in vr_e4.DefaultIfEmpty()
                    select new VisitPrescription
                    {
                        PId = it.PID,
                        SeqNo = it.SEQNO,
                        MedId = it.MEDID,
                        TakeQty = it.TAKEQTY,
                        Qty = it.QTY,
                        Freq = it.FREQ,
                        Freqday = it.FREQDAY,
                        Freqqty = it.FREQQTY,
                        TakeWay = it.TAKEWAY,
                        Freqtime = it.FREQTIME,
                        LongFlag = it.LONGFLAG,
                        UseFlag = it.USEFLAG,
                        StartDate = it.STARTDATE,
                        EndDate = it.ENDDATE,
                        Description = it.DESCRIPTION,
                        OrgId = it.ORGID,
                        EngName = mm.ENGNAME,
                        MedKind = mm.MEDKIND,
                        FeeNo = nn.FEENO,
                        VisitDoctorName = vr_emp4.DOCNAME,
                        VisitHospName = vr_emp2.HOSPNAME,
                        VisitDeptName = vr_emp3.DEPTNAME,
                        VisitType = nn.VISITTYPE,
                        TakeDays = nn.TAKEDAYS
                    };
            q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            if (request.Data.StartDate != null && request.Data.EndDate != null)
            {
                q = q.Where(m => m.StartDate.Value >= request.Data.StartDate.Value && m.StartDate.Value <= request.Data.EndDate.Value);
            }
            if (request.Data.StartDate != null && request.Data.EndDate == null)
            {
                q = q.Where(m => m.StartDate.Value >= request.Data.StartDate.Value);
            }
            if (request.Data.StartDate == null && request.Data.EndDate != null)
            {
                q = q.Where(m => m.StartDate.Value <= request.Data.EndDate.Value);
            }
            q = q.OrderByDescending(m => m.SeqNo);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        #endregion

        #region 團隊活動
        public BaseResponse<IList<GroupActivityRec>> QueryGroupActivityRec(BaseRequest<GroupActivityRecFilter> request)
        {
            BaseResponse<IList<GroupActivityRec>> response = new BaseResponse<IList<GroupActivityRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_GROUPACTIVITYREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.LEADERNAME equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        GroupActivityRec = n,
                        EmpName = re.EMPNAME
                    };
            if (!string.IsNullOrEmpty(request.Data.ActivityName))
            {
                q = q.Where(m => m.GroupActivityRec.ACTIVITYNAME.Contains(request.Data.ActivityName));
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.GroupActivityRec.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.GroupActivityRec.RECORDDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<GroupActivityRec>();
                foreach (dynamic item in list)
                {
                    GroupActivityRec newItem = Mapper.DynamicMap<GroupActivityRec>(item.GroupActivityRec);
                    newItem.LeaderName = item.EmpName;
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

        public BaseResponse<GroupActivityRec> GetGroupActivityRec(int id)
        {
            BaseResponse < GroupActivityRec > response= base.Get<LTC_GROUPACTIVITYREC, GroupActivityRec>((q) => q.ID == id);
            if(response.Data!=null)
            {
                if(!string.IsNullOrEmpty(response.Data.AttendNo))
                {
                    List<string> resident = new List<string>(response.Data.AttendNo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                    List<long> residentNo=new List<long>();
                    residentNo.AddRange(resident.ConvertAll(o=>long.Parse(o)));
                    var residentName = (from r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                                                 join ip in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(x => residentNo.Contains(x.FEENO)) on r.REGNO equals ip.REGNO
                                                 select new
                                                 {
                                                     r.NAME
                                                 });
                    if(residentName!=null)
                    {
                        response.Data.AttendName = string.Join(",", residentName.Select(x=>x.NAME).ToList()); 
                    }
                }
            }
            return response;
        }

        public BaseResponse<GroupActivityRec> SaveGroupActivityRec(GroupActivityRec request)
        {
            if (request.Id == 0)
            {
                if (string.IsNullOrEmpty(request.CreateBy))
                {
                    request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                }
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_GROUPACTIVITYREC, GroupActivityRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteGroupActivityRec(int id)
        {
            return base.Delete<LTC_GROUPACTIVITYREC>(id);
        }
        #endregion
    }
}