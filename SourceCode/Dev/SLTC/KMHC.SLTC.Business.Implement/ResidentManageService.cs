/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Other;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using KMHC.SLTC.Business.Entity.EC.Model;

namespace KMHC.SLTC.Business.Implement
{
    public class ResidentManageService : BaseService, IResidentManageService
    {
        #region 住民
        public BaseResponse<IList<Person>> QueryPerson(BaseRequest<PersonFilter> request)
        {
            var response = base.Query<LTC_REGFILE, Person>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.Name) && !string.IsNullOrEmpty(request.Data.IdNo))
                {
                    q = q.Where(m => m.NAME.Contains(request.Data.Name) || m.IDNO.Contains(request.Data.IdNo));
                }
                if (!string.IsNullOrEmpty(request.Data.IdNo))
                {
                    q = q.Where(m => m.IDNO == request.Data.IdNo);
                }
                if (!string.IsNullOrEmpty(request.Data.ResidengNo))
                {
                    q = q.Where(m => m.RESIDENGNO == request.Data.ResidengNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<Person>> QueryPersonExtend(BaseRequest<PersonFilter> request)
        {
            BaseResponse<IList<Person>> response = new BaseResponse<IList<Person>>();
            var q = from reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on reg.REGNO equals rel.REGNO into reg_rels
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO into reg_ipds
                    from reg_ipd in reg_ipds.DefaultIfEmpty()
                    join bed in
                        (from in_bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet where in_bed.FEENO != null select in_bed) on reg_ipd.FEENO equals bed.FEENO into ipd_beds
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    from ipd_bed in ipd_beds.DefaultIfEmpty()
                    select new Person
                    {
                        FeeNo = reg_ipd.FEENO,
                        RegNo = reg.REGNO,
                        IdNo = reg.IDNO,
                        CreateDate = reg.CREATEDATE,
                        Name = reg.NAME,
                        Sex = reg.SEX,
                        Age = reg.AGE ?? 0,
                        Floor = ipd_bed.FLOOR,
                        BedNo = ipd_bed.BEDNO,
                        OrgId = reg.ORGID,
                        Brithdate = reg.BRITHDATE,
                        IpdFlag = reg_ipd.IPDFLAG,
                        Relation = new Relation() { PhotoPath = reg_rel.PHOTOPATH }
                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (!string.IsNullOrEmpty(request.Data.Name) && !string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name) || m.IdNo.Contains(request.Data.IdNo));
            }
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                q = q.Where(m => m.IpdFlag == request.Data.IpdFlag);
            }
            //20160927修改 Duke 排序按登记日期倒叙排列
            q = q.OrderByDescending(m => m.CreateDate);
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

        public BaseResponse<Person> GetPerson(int regNo)
        {
            var response = base.Get<LTC_REGFILE, Person>((q) => q.REGNO == regNo);
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.Data.RegNo = regNo;

            var residentList = this.QueryResident(request);
            if (residentList.Data.Count > 0)
            {
                response.Data.FeeNo = residentList.Data[0].FeeNo;
            }
            return response;
        }

        public BaseResponse<Person> SavePerson(Person request)
        {
            //if (request != null)
            //{
            //    var response = base.Get<LTC_REGFILE, Person>((q) => q.IDNO == request.IdNo && q.REGNO != request.FeeNo);
            //    if (response != null && response.Data != null)
            //    {
            //        BaseResponse<Person> person = new BaseResponse<Person>();
            //        person.ResultMessage = "該身份證重復,請重新輸入！";
            //        response.ResultCode = (int)EnumResponseStatus.ExceptionHappened;
            //        return person;
            //    }
            //}

            if (request.RegNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.CreateDate = DateTime.Now;
                request.RegNo = int.Parse(base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.RegNo));
            }
            unitOfWork.BeginTransaction();
            var responsePerson = base.Save<LTC_REGFILE, Person>(request, (q) => q.REGNO == request.RegNo);
            if (request.Relation != null)
            {
                request.Relation.FeeNo = request.FeeNo ?? 0;
                request.Relation.RegNo = request.RegNo;
                request.Relation.OrgId = request.OrgId;
                this.SaveRelation(request.Relation);
            }
            if (request.RelationDtls != null)
            {
                request.RelationDtls.ForEach(m => m.FeeNo = request.FeeNo ?? 0);
                this.SaveRelationDtl(request.FeeNo ?? 0, request.RelationDtls);
            }
            if (request.AttachArchives != null)
            {
                request.AttachArchives.ForEach(m => { m.FeeNo = request.FeeNo ?? 0; m.RegNo = request.RegNo; m.OrgId = request.OrgId; });
                this.SaveAttachFile(request.FeeNo ?? 0, request.AttachArchives);
            }
            unitOfWork.Commit();
            return responsePerson;
        }

        public BaseResponse DeletePerson(int regNO)
        {
            unitOfWork.BeginTransaction();
            var regRepository = unitOfWork.GetRepository<LTC_REGFILE>();
            var ipdRepository = unitOfWork.GetRepository<LTC_IPDREG>();
            var ipdRegoutRepository = unitOfWork.GetRepository<LTC_IPDREGOUT>();
            var leaveHospRepository = unitOfWork.GetRepository<LTC_LEAVEHOSP>();
            var regRelationRepository = unitOfWork.GetRepository<LTC_REGRELATION>();
            var regRelationDtlRepository = unitOfWork.GetRepository<LTC_REGRELATIONDTL>();
            var regAttachFileRepository = unitOfWork.GetRepository<LTC_REGATTACHFILE>();
            var familyDiscussRecRepository = unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>();
            var regHealthRepository = unitOfWork.GetRepository<LTC_REGHEALTH>();
            var reginsDtlRepository = unitOfWork.GetRepository<LTC_REGINSDTL>();
            var ipdVerifyRepository = unitOfWork.GetRepository<LTC_IPDVERIFY>();
            var ipdCloseCaseRepository = unitOfWork.GetRepository<LTC_IPDCLOSECASE>();
            var regDemandRepository = unitOfWork.GetRepository<LTC_REGDEMAND>();

            var ipdList = ipdRepository.dbSet.Where(m => m.REGNO == regNO).ToList();

            ipdList.ForEach(item =>
            {
                ipdRegoutRepository.Delete(m => m.FEENO == item.FEENO);
                leaveHospRepository.Delete(m => m.FEENO == item.FEENO);
                regRelationRepository.Delete(m => m.FEENO == item.FEENO);
                regRelationDtlRepository.Delete(m => m.FEENO == item.FEENO);
                regAttachFileRepository.Delete(m => m.FEENO == item.FEENO);
                familyDiscussRecRepository.Delete(m => m.FEENO == item.FEENO);
                regHealthRepository.Delete(m => m.FEENO == item.FEENO);
                reginsDtlRepository.Delete(m => m.FEENO == item.FEENO);
                ipdVerifyRepository.Delete(m => m.FEENO == item.FEENO);
                ipdCloseCaseRepository.Delete(m => m.FEENO == item.FEENO);
                regDemandRepository.Delete(m => m.FEENO == item.FEENO);

                ipdRepository.Delete(item);
            });
            var response = new BaseResponse();
            regRepository.Delete(regNO);
            unitOfWork.Commit();
            return response;
        }
        #endregion

        #region 入住
        public BaseResponse<IList<Resident>> QueryResident(BaseRequest<ResidentFilter> request)
        {
            var response = base.Query<LTC_IPDREG, Resident>(request, (q) =>
            {
                if (request.Data.RegNo.HasValue)
                {
                    q = q.Where(m => m.REGNO == request.Data.RegNo);
                }
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<Resident>> QueryResidentExtend(BaseRequest<ResidentFilter> request)
        {
            string _orgId = SecurityHelper.CurrentPrincipal.OrgId;
            BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { orgId = ipd.ORGID, floor = ipd.FLOOR } equals new { orgId = f.ORGID, floor = f.FLOORID } into ipd_f_set
                    from ipd_f in ipd_f_set.DefaultIfEmpty()
                    where ipd.ORGID == _orgId
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    //join bb in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bb.FEENO into ipd_bbs
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on ipd.FEENO equals rel.FEENO into reg_rels
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    //from ipd_bb in ipd_bbs.DefaultIfEmpty()
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        //FeeNoString = Convert.ToString(ipd.FEENO),
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        ResidengNo = ipd_r.RESIDENGNO,
                        Sex = ipd_r.SEX,
                        Age = ipd_r.AGE ?? 0,
                        Floor = ipd.FLOOR,//ipd_bb.FLOOR,
                        FloorName = ipd_f.FLOORNAME,
                        BedNo = ipd.BEDNO,//ipd_bb.BEDNO,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG,
                        ImgUrl = reg_rel.PHOTOPATH,//Relation = new Relation() { PhotoPath = reg_rel.PHOTOPATH }
                        InDate = ipd.INDATE,
                        IdNo = ipd_r.IDNO,
                        BirthDay = ipd_r.BRITHDATE
                    };
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                {
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                }
                else
                {
                    q = q.Where(m => m.IpdFlag == "O");
                }

            }
            if (request.Data.FeeNo.HasValue && !string.IsNullOrEmpty(request.Data.BedNo))
            {
                //q = q.Where(m => m.FeeNoString.Contains(request.Data.FeeNoString) || m.BedNo.Contains(request.Data.BedNo) || m.Name.Contains(request.Data.Name));
                q = q.Where(m => m.ResidengNo.Contains(request.Data.ResidengNo) || m.FeeNo == request.Data.FeeNo || m.BedNo.Contains(request.Data.BedNo));
            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name));
            }

            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo == request.Data.IdNo);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorId))
            {
                q = q.Where(m => m.Floor == request.Data.FloorId);
            }
            q = q.OrderByDescending(m => m.FeeNo);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.Distinct().ToList();
            }
            return response;
        }

        public BaseResponse<IList<Resident>> QueryResidentByName(BaseRequest<ResidentFilter> request)
        {
            BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    join bb in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bb.FEENO into ipd_bbs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    from ipd_bb in ipd_bbs.DefaultIfEmpty()
                    join of in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { floorID = ipd_bb.FLOOR, OrgId = ipd_bb.ORGID } equals new { floorID = of.FLOORID, OrgId = of.ORGID } into ipd_bb_ofs
                    join or in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { RoomNo = ipd_bb.ROOMNO, OrgId = ipd_bb.ORGID } equals new { RoomNo = or.ROOMNO, OrgId = or.ORGID } into ipd_bb_oms
                    from ipd_bb_of in ipd_bb_ofs.DefaultIfEmpty()
                    from ipd_bb_om in ipd_bb_oms.DefaultIfEmpty()
                    where ipd.IPDFLAG == "I"
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        //FeeNoString = Convert.ToString(ipd.FEENO),
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        Sex = ipd_r.SEX,
                        Age = ipd_r.AGE ?? 0,
                        Floor = ipd_bb.FLOOR,
                        FloorName = ipd_bb_of.FLOORNAME,
                        RoomNo = ipd_bb.ROOMNO,
                        RoomName = ipd_bb_om.ROOMNAME,
                        BedNo = ipd_bb.BEDNO,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG.Trim(),
                        IdNo = ipd_r.IDNO
                    };
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                {
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                }
                else
                {
                    q = q.Where(m => m.IpdFlag == "O");
                }

            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name == request.Data.Name);
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo == request.Data.IdNo);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorName))
            {
                q = q.Where(m => m.FloorName.Contains(request.Data.FloorName));
            }
            if (!string.IsNullOrEmpty(request.Data.RoomName))
            {
                q = q.Where(m => m.RoomName.Contains(request.Data.RoomName));
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.FeeNo);
            List<Resident> list = q.ToList();
            response.Data = list;
            return response;
        }

        public BaseResponse<Resident> GetResident(long feeNO)
        {
            return base.Get<LTC_IPDREG, Resident>((q) => q.REGNO == feeNO);
        }

        public BaseResponse<Resident> SaveResident(Resident request)
        {
            if (!request.InDate.HasValue)
            {
                request.InDate = DateTime.Now;
            }
            return base.Save<LTC_IPDREG, Resident>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteResident(long feeNO)
        {
            return base.Delete<LTC_IPDREG>(feeNO);
        }
        #endregion

        #region 入住審核
        public BaseResponse<IList<Verify>> QueryVerify(BaseRequest<VerifyFilter> request)
        {
            var response = base.Query<LTC_IPDVERIFY, Verify>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Verify> GetVerify(long feeNo)
        {
            return base.Get<LTC_IPDVERIFY, Verify>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<Verify> SaveVerify(Verify request)
        {
            return base.Save<LTC_IPDVERIFY, Verify>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteVerify(long feeNo)
        {
            return base.Delete<LTC_IPDVERIFY>(feeNo);
        }
        #endregion

        #region 请假记录
        public BaseResponse<IList<LeaveHosp>> QueryLeaveHosp(BaseRequest<LeaveHospFilter> request)
        {
            var response = base.Query<LTC_LEAVEHOSP, LeaveHosp>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 查询住民最新一笔请假记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>离院列表</returns>
        public BaseResponse<IList<LeaveHosp>> GetNewLeaveHosp(BaseRequest<LeaveHospFilter> request)
        {
            var response = base.Query<LTC_LEAVEHOSP, LeaveHosp>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<LeaveHosp> GetLeaveHosp(long leaveHospId)
        {
            return base.Get<LTC_LEAVEHOSP, LeaveHosp>((q) => q.ID == leaveHospId);
        }

        public BaseResponse<LeaveHosp> SaveLeaveHosp(LeaveHosp request)
        {

            #region 移除管路信息
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                socialWorkerManageService.RemovePipelineRec(Convert.ToInt32(request.FeeNo), keys[i], Convert.ToDateTime(request.StartDate), "請假外出自動移除");
            }
            #endregion

            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.ShowNumber = int.Parse(base.GenerateCode(request.OrgId, EnumCodeKey.LeaveHospId));
            }
            return base.Save<LTC_LEAVEHOSP, LeaveHosp>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteLeaveHosp(long leaveHospId)
        {
            return base.Delete<LTC_LEAVEHOSP>(leaveHospId);
        }
        #endregion

        #region 零用金
        public BaseResponse<IList<Deposit>> QueryDeposit(BaseRequest<DepositFilter> request)
        {
            var response = base.Query<LTC_DEPTFILE, Deposit>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                //if (request.Data.FeeNo.HasValue)
                //{
                //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                //}
                q = q.OrderByDescending(m => m.UPDATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Deposit> GetDeposit(string deptNo)
        {
            return base.Get<LTC_DEPTFILE, Deposit>((q) => q.DEPTNO == deptNo);
        }

        public BaseResponse<Deposit> SaveDeposit(Deposit request)
        {
            return base.Save<LTC_DEPTFILE, Deposit>(request, (q) => q.DEPTNO == request.DeptNo);
        }

        public BaseResponse DeleteDeposit(string deptNo)
        {
            return base.Delete<LTC_DEPTFILE>(deptNo);
        }
        #endregion

        #region 出院結案
        public BaseResponse<IList<CloseCase>> QueryCloseCase(BaseRequest<CloseCaseFilter> request)
        {
            var response = base.Query<LTC_IPDCLOSECASE, CloseCase>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderByDescending(m => m.CLOSEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<CloseCase> GetCloseCase(long feeNo)
        {
            return base.Get<LTC_IPDCLOSECASE, CloseCase>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<CloseCase> SaveCloseCase(CloseCase request)
        {
            return base.Save<LTC_IPDCLOSECASE, CloseCase>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteCloseCase(long feeNo)
        {
            return base.Delete<LTC_IPDCLOSECASE>(feeNo);
        }
        #endregion

        #region 社會福利
        public BaseResponse<IList<ResidentDtl>> QueryResidentDtl(BaseRequest<ResidentDtlFilter> request)
        {
            var response = base.Query<LTC_REGINSDTL, ResidentDtl>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }
		
        /// <summary>
        /// 获取居民的养护标准
        /// Add By Duke On 2016-11-17
        /// </summary>
        /// <param name="idNo"></param>
        /// <returns></returns>
        public BaseResponse<EC_CostLevel> QueryCostLevel(string idNo)
        {
            var response = new BaseResponse<EC_CostLevel>();
            //var q = from l in unitOfWork.GetRepository<EC_REGTOTALRESULT>().dbSet
            //        join m in unitOfWork.GetRepository<EC_CARELEVEL>().dbSet on l.CARELEVELID equals m.CARELEVELID
            //        join n in unitOfWork.GetRepository<EC_COSTLEVEL>().dbSet on m.COSTLEVELID equals n.COSTLEVELID
            //        select new EC_CostLevel
            //        {
            //            CostLevelId = n.COSTLEVELID,
            //            MealsCost = n.MANAGECOST,
            //            BedCost = n.BEDCOST,
            //            CareCost = n.CARECOST,
            //            ManageCost = n.MANAGECOST,
            //            NormalRoom = n.NORMALROOM,
            //            TrioRoom = n.TRIOROOM,
            //            DoubleRoom = n.DOUBLEROOM,
            //            SingleRoom = n.SINGLEROOM,
            //            CareLevel=m.CARELEVEL,
            //            IdNo = l.IDNO,
            //        };
            //q = q.Where(m => m.IdNo == idNo);
            //response.RecordsCount = q.ToList().Count;
            //response.Data = q.ToList().FirstOrDefault();
            return response;

        }
		
        public BaseResponse<ResidentDtl> GetResidentDtl(long feeNo)
        {
            return base.Get<LTC_REGINSDTL, ResidentDtl>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<ResidentDtl> SaveResidentDtl(ResidentDtl request)
        {
            return base.Save<LTC_REGINSDTL, ResidentDtl>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteResidentDtl(long feeNo)
        {
            return base.Delete<LTC_REGINSDTL>(feeNo);
        }
        #endregion

        #region 需求管理
        public BaseResponse<IList<Demand>> QueryDemand(BaseRequest<DemandFilter> request)
        {
            var response = base.Query<LTC_REGDEMAND, Demand>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.Id);
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Demand> GetDemand(long id)
        {
            return base.Get<LTC_REGDEMAND, Demand>((q) => q.ID == id);
        }

        public BaseResponse<Demand> SaveDemand(Demand request)
        {
            return base.Save<LTC_REGDEMAND, Demand>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDemand(long id)
        {
            return base.Delete<LTC_REGDEMAND>(id);
        }
        #endregion

        #region 健康管理
        public BaseResponse<IList<Health>> QueryHealth(BaseRequest<HealthFilter> request)
        {
            var response = base.Query<LTC_REGHEALTH, Health>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public BaseResponse<Health> GetHealth(long feeNo)
        {
            BaseResponse<Health> health = base.Get<LTC_REGHEALTH, Health>((q) => q.FEENO == feeNo);

            if (health.Data == null)
            {

                LTC_CAREDEMANDEVAL careDemand = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(x => x.FEENO == feeNo).OrderByDescending(x => x.EVALDATE).FirstOrDefault();
                if (careDemand != null)
                {
                    health = new BaseResponse<Health>();
                    health.Data = new Health();
                    health.Data.ALLERGY = string.Format("藥物過敏史:{0}\r食物過敏:{1}\r其他過敏:{2}", careDemand.ALLERGY_DRUG, careDemand.ALLERGY_FOOD, careDemand.ALLERGY_OTHERS);
                }
            }

            return health;
        }

        public BaseResponse<Health> SaveHealth(Health request)
        {
            return base.Save<LTC_REGHEALTH, Health>(request, (q) => q.FEENO == request.FEENO);
        }

        public BaseResponse DeleteHealth(long feeNo)
        {
            return base.Delete<LTC_REGHEALTH>(feeNo);
        }
        #endregion

        #region 附加檔案
        public BaseResponse<IList<AttachFile>> QueryAttachFile(BaseRequest<AttachFileFilter> request)
        {
            var response = base.Query<LTC_REGATTACHFILE, AttachFile>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<AttachFile> GetAttachFile(long feeNo)
        {
            return base.Get<LTC_REGATTACHFILE, AttachFile>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<AttachFile> SaveAttachFile(AttachFile request)
        {
            return base.Save<LTC_REGATTACHFILE, AttachFile>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse<List<AttachFile>> SaveAttachFile(long feeNo, List<AttachFile> request)
        {
            Mapper.CreateMap<AttachFile, LTC_REGATTACHFILE>();
            var attachFileRep = unitOfWork.GetRepository<LTC_REGATTACHFILE>();
            attachFileRep.dbSet.Where(m => m.FEENO == feeNo)
                .Select(m => m)
                .ToList()
                .ForEach(m => attachFileRep.Delete(m));



            var nowTime = DateTime.Now;

            BaseResponse<List<AttachFile>> response = new BaseResponse<List<AttachFile>>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_REGATTACHFILE>(m);
                if (!model.CREATEDATE.HasValue)
                {
                    model.CREATEDATE = nowTime;
                }
                if (string.IsNullOrEmpty(model.CREATEBY))
                {
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                }
                attachFileRep.Insert(model);
            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteAttachFile(long feeNo)
        {
            return base.Delete<LTC_REGATTACHFILE>(feeNo);
        }
        #endregion

        #region 通信錄住民地址
        public BaseResponse<IList<Relation>> QueryRelation(BaseRequest<RelationFilter> request)
        {
            var response = base.Query<LTC_REGRELATION, Relation>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public BaseResponse<Relation> GetRelation(long feeNo)
        {
            return base.Get<LTC_REGRELATION, Relation>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<Relation> SaveRelation(Relation request)
        {
            if (request.FeeNo == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_REGRELATION, Relation>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteRelation(long feeNo)
        {
            return base.Delete<LTC_REGRELATION>(feeNo);
        }
        #endregion

        #region 通信錄亲属地址
        public BaseResponse<IList<RelationDtl>> QueryRelationDtl(BaseRequest<RelationDtlFilter> request)
        {
            var response = base.Query<LTC_REGRELATIONDTL, RelationDtl>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<RelationDtl> GetRelationDtl(long feeNo)
        {
            return base.Get<LTC_REGRELATIONDTL, RelationDtl>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<RelationDtl> SaveRelationDtl(RelationDtl request)
        {
            if (!request.FeeNo.HasValue)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_REGRELATIONDTL, RelationDtl>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse<List<RelationDtl>> SaveRelationDtl(long feeNo, List<RelationDtl> request)
        {
            unitOfWork.BeginTransaction();
            Mapper.CreateMap<RelationDtl, LTC_REGRELATIONDTL>();
            var relationDtlRep = unitOfWork.GetRepository<LTC_REGRELATIONDTL>();
            relationDtlRep.dbSet.Where(m => m.FEENO == feeNo)
                .Select(m => m)
                .ToList()
                .ForEach(m => relationDtlRep.Delete(m));

            var nowTime = DateTime.Now;
            BaseResponse<List<RelationDtl>> response = new BaseResponse<List<RelationDtl>>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_REGRELATIONDTL>(m);
                model.CREATEDATE = nowTime;
                model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                relationDtlRep.Insert(model);
            });
            unitOfWork.Save();
            response.Data = request;

            var lastEditItem = request.Where(m => m.EconomyFlag).OrderByDescending(m => m.UpdateDate).FirstOrDefault();
            if (lastEditItem != null)
            {
                Relation relation = new Relation();
                relation.PaymentPerson = lastEditItem.Name;
                relation.BillAddress = lastEditItem.Address2;
                relation.Kinship = lastEditItem.Kinship;
                base.Save<LTC_REGRELATION, Relation>(relation, (q) => q.FEENO == feeNo, new List<string> { "PaymentPerson", "BillAddress", "Kinship" });
            }

            unitOfWork.Commit();
            return response;
        }

        public BaseResponse DeleteRelationDtl(long feeNo)
        {
            return base.Delete<LTC_REGRELATIONDTL>(feeNo);
        }

        public bool ExistResident(long regNo, string[] status)
        {
            return base.unitOfWork.GetRepository<LTC_IPDREG>().Exists(o => o.REGNO == regNo && status.Contains(o.IPDFLAG));
        }

        #endregion

        #region 住民訪視
        public BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscuss(BaseRequest<FamilyDiscussFilter> request)
        {
            var response = base.Query<LTC_FAMILYDISCUSSREC, FamilyDiscuss>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscussExtend(BaseRequest<FamilyDiscussFilter> request)
        {
            BaseResponse<IList<FamilyDiscuss>> response = new BaseResponse<IList<FamilyDiscuss>>();
            var q = from f in unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on f.RECORDBY equals e.EMPNO into ees
                    from ee in ees.DefaultIfEmpty()
                    select new FamilyDiscuss
                    {
                        Id = f.ID,
                        FeeNo = f.FEENO,
                        RecordBy = f.RECORDBY,
                        RecordByShow = ee.EMPNAME,
                        StartDate = f.STARTDATE,
                        EndDate = f.ENDDATE,
                        VisitType = f.VISITTYPE,
                        VisitorName = f.VISITORNAME,
                        Appellation = f.APPELLATION,
                        BloodRelationship = f.BLOODRELATIONSHIP,
                        Description = f.DESCRIPTION,
                        OrgId = f.ORGID
                    };

            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            q = q.OrderByDescending(m => m.StartDate);
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

        public BaseResponse<FamilyDiscuss> GetFamilyDiscuss(int Id)
        {
            return base.Get<LTC_FAMILYDISCUSSREC, FamilyDiscuss>((q) => q.ID == Id);
        }

        public BaseResponse<FamilyDiscuss> SaveFamilyDiscuss(FamilyDiscuss request)
        {
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_FAMILYDISCUSSREC, FamilyDiscuss>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteFamilyDiscuss(int Id)
        {
            return base.Delete<LTC_FAMILYDISCUSSREC>(Id);
        }
        #endregion

        #region 預約登記
        public BaseResponse<IList<Preipd>> QueryPreipd(BaseRequest<PreipdFilter> request)
        {
            var response = new BaseResponse<IList<Preipd>>();
            var q = from pr in unitOfWork.GetRepository<LTC_PREIPD>().dbSet
                    join dp in unitOfWork.GetRepository<LTC_DEPTFILE>().dbSet on pr.DEPTNO equals dp.DEPTNO into dp_p
                    from dp_part in dp_p.DefaultIfEmpty()
                    select new
                    {
                        preipd = pr,
                        DeptName = dp_part.DEPTNAME

                    };
            q = q.OrderByDescending(m => m.preipd.PREFEENO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Preipd>();
                foreach (dynamic item in list)
                {
                    Preipd newItem = Mapper.DynamicMap<Preipd>(item.preipd);
                    newItem.DeptName = item.DeptName;
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

            //var response = base.Query<LTC_PREIPD, Preipd>(request, (q) =>
            //{
            //    q = q.OrderByDescending(m => m.PREFEENO);
            //    return q;
            //});
            //return response;
        }

        public BaseResponse<Preipd> SavePreipd(Preipd request)
        {
            return base.Save<LTC_PREIPD, Preipd>(request, (q) => q.PREFEENO == request.PreFeeNo);
        }

        public BaseResponse DeletePreipd(long PreFeeNo)
        {
            return base.Delete<LTC_PREIPD>(PreFeeNo);
        }
        #endregion

        #region 出院辦理
        public BaseResponse<Ipdregout> GetIpdregout(long feeNo)
        {
            return base.Get<LTC_IPDREGOUT, Ipdregout>((q) => q.FEENO == feeNo);
        }
        public BaseResponse<Ipdregout> SaveIpdregout(Ipdregout request)
        {
            var response = new BaseResponse<Ipdregout>();
            var cm = Mapper.CreateMap<Ipdregout, LTC_IPDREGOUT>();
            Mapper.CreateMap<LTC_IPDREGOUT, Ipdregout>();
            var reqQueIpdregout = unitOfWork.GetRepository<LTC_IPDREGOUT>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueCloseCase = unitOfWork.GetRepository<LTC_IPDCLOSECASE>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueResident = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueBedBasic = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            DateTime? _inDate = new DateTime();
            //清床位信息
            if (reqQueBedBasic != null)
            {
                reqQueBedBasic.FEENO = null;
                reqQueBedBasic.BEDSTATUS = "E";
                unitOfWork.GetRepository<LTC_BEDBASIC>().Update(reqQueBedBasic);
            }

            #region 移除管路信息
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                socialWorkerManageService.RemovePipelineRec(request.FeeNo, keys[i], DateTime.Now, "結案作業自動移除");
            }
            #endregion

            //更新住民信息表
            if (reqQueResident != null)
            {
                reqQueResident.IPDFLAG = "O";//状态更新为 O 结案
                reqQueResident.OUTDATE = request.CloseDate;//出院日期更新为结案日期
                unitOfWork.GetRepository<LTC_IPDREG>().Update(reqQueResident);
                _inDate = reqQueResident.INDATE;
            }
            //
            if (reqQueCloseCase == null)
            {
                reqQueCloseCase = new LTC_IPDCLOSECASE();
                reqQueCloseCase.FEENO = request.FeeNo;
                reqQueCloseCase.CLOSEFLAG = request.CloseFlag;//结案状态
                reqQueCloseCase.CLOSEDATE = request.CloseDate;//结案日期
                reqQueCloseCase.CLOSEREASON = request.CloseReason;//结案原因
                unitOfWork.GetRepository<LTC_IPDCLOSECASE>().Insert(reqQueCloseCase);
            }
            else
            {
                reqQueCloseCase.CLOSEFLAG = request.CloseFlag;
                reqQueCloseCase.CLOSEDATE = request.CloseDate;
                reqQueCloseCase.CLOSEREASON = request.CloseReason;
                unitOfWork.GetRepository<LTC_IPDCLOSECASE>().Update(reqQueCloseCase);
            }
            //
            if (reqQueIpdregout == null)
            {
                reqQueIpdregout = Mapper.Map<LTC_IPDREGOUT>(request);
                reqQueIpdregout.INDATE = _inDate;
                unitOfWork.GetRepository<LTC_IPDREGOUT>().Insert(reqQueIpdregout);
            }
            else
            {
                reqQueIpdregout.INDATE = _inDate;
                Mapper.Map(request, reqQueIpdregout);
                unitOfWork.GetRepository<LTC_IPDREGOUT>().Update(reqQueIpdregout);
            }
            unitOfWork.Save();
            Mapper.Map(reqQueIpdregout, request);
            response.Data = request;
            return response;


            //CloseCase requestCloseCase;
            //BaseResponse<CloseCase> brCloseCase = GetCloseCase(request.FeeNo);
            //Resident resident = base.Get<LTC_IPDREG, Resident>((q) => q.FEENO == request.FeeNo).Data;
            //resident.IpdFlag = "O";
            //if (brCloseCase.Data != null)
            //{
            //    requestCloseCase = brCloseCase.Data;
            //    requestCloseCase.CloseFlag = request.CloseFlag;
            //    requestCloseCase.CloseDate = request.CloseDate;
            //    requestCloseCase.CloseReason = request.CloseReason;
            //}
            //else
            //{
            //    requestCloseCase = new CloseCase
            //   {
            //       FeeNo = request.FeeNo,
            //       CloseFlag = request.CloseFlag,
            //       CloseDate = request.CloseDate,
            //       CloseReason = request.CloseReason,
            //   };
            //}
            //request.InDate = resident.InDate;
            //BaseResponse<Ipdregout> baseRes = base.Save<LTC_IPDREGOUT, Ipdregout>(request, (q) => q.FEENO == request.FeeNo);
            //base.Save<LTC_IPDREG, Resident>(resident, (q) => q.FEENO == resident.FeeNo);
            //base.Save<LTC_IPDCLOSECASE, CloseCase>(requestCloseCase, (q) => q.FEENO == request.FeeNo);
            //return baseRes;
        }
        #endregion

        #region 退住院
        public BaseResponse<Resident> GetLeaveNursing(long feeNo)
        {
            var filter = new ResidentDtlFilter
            {
                FeeNo = feeNo
            };
            var request = new BaseRequest<ResidentDtlFilter>
            {
                Data = filter
            };
            var br = new BaseResponse<Resident>();
            var response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.CARER equals e1.EMPNO into ipd_e1
                    join e2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSENO equals e2.EMPNO into ipd_e2
                    join e3 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NUTRITIONIST equals e3.EMPNO into ipd_e3
                    join e4 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.PHYSIOTHERAPIST equals e4.EMPNO into ipd_e4
                    join e5 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.DOCTOR equals e5.EMPNO into ipd_e5
                    join bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bed.FEENO into ipd_bedb
                    from ipd_ename1 in ipd_e1.DefaultIfEmpty()
                    from ipd_ename2 in ipd_e2.DefaultIfEmpty()
                    from ipd_ename3 in ipd_e3.DefaultIfEmpty()
                    from ipd_ename4 in ipd_e4.DefaultIfEmpty()
                    from ipd_ename5 in ipd_e5.DefaultIfEmpty()
                    from ipd_bed in ipd_bedb.DefaultIfEmpty()
                    select new
                    {

                        resident = ipd,
                        CarerName = ipd_ename1.EMPNAME,
                        NurseName = ipd_ename2.EMPNAME,
                        NutritionistName = ipd_ename3.EMPNAME,
                        PhysiotherapistName = ipd_ename4.EMPNAME,
                        DoctorName = ipd_ename5.EMPNAME,
                        BedStatus = ipd_bed.BEDSTATUS
                    };
            q = q.Where(m => m.resident.FEENO == feeNo);
            q = q.OrderByDescending(m => m.resident.FEENO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Resident>();
                foreach (dynamic item in list)
                {
                    Resident newItem = Mapper.DynamicMap<Resident>(item.resident);
                    newItem.CarerName = item.CarerName;
                    newItem.NurseName = item.NurseName;
                    newItem.NutritionistName = item.NutritionistName;
                    newItem.PhysiotherapistName = item.PhysiotherapistName;
                    newItem.DoctorName = item.DoctorName;
                    newItem.BedStatus = item.BedStatus;
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
            if (response.Data.Count > 0)
            {

                br.Data = response.Data[0];
            }
            return br;

        }
        public BaseResponse<BedBasic> GetLeaveNursingBedInfo(long feeNo)
        {
            return base.Get<LTC_BEDBASIC, BedBasic>((q) => q.FEENO == feeNo);
        }
        public BaseResponse<Resident> SaveLeaveNursing(Resident resident)
        {
            var response = new BaseResponse<Resident>();
            var reqQueBedBasic = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.Where(m => m.FEENO == resident.FeeNo).FirstOrDefault();
            //清床位信息
            if (reqQueBedBasic != null)
            {
                reqQueBedBasic.FEENO = null;
                reqQueBedBasic.BEDSTATUS = "E";
                unitOfWork.GetRepository<LTC_BEDBASIC>().Update(reqQueBedBasic);
            }
            var reqQueResident = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == resident.FeeNo).FirstOrDefault();
            var cm = Mapper.CreateMap<Resident, LTC_IPDREG>();
            Mapper.CreateMap<LTC_IPDREG, Resident>();
            Mapper.Map(resident, reqQueResident);
            reqQueResident.IPDFLAG = "D";//状态更新为 D 出院
            unitOfWork.GetRepository<LTC_IPDREG>().Update(reqQueResident);
            unitOfWork.Save();
            Mapper.Map(reqQueResident, resident);
            response.Data = resident;
            return response;




            ////var result = true;
            ////獲取床位信息
            //BedBasic requestBedBasic = base.Get<LTC_BEDBASIC, BedBasic>((q) => q.FEENO == resident.FeeNo).Data;
            //if (requestBedBasic != null)
            //{
            //    requestBedBasic.FEENO = null;
            //    requestBedBasic.BedStatus = "E";
            //    //清空住民床位信息
            //    base.Save<LTC_BEDBASIC, BedBasic>(requestBedBasic, (q) => q.BEDNO == requestBedBasic.BedNo);
            //}
            //////獲取住民信息
            ////Resident requestResident = base.Get<LTC_IPDREG, Resident>((q) => q.FEENO == resident.FeeNo).Data;
            ////if (resident != null)
            ////{
            //resident.IpdFlag = "D";
            ////更新住民信息
            //return base.Save<LTC_IPDREG, Resident>(resident, (q) => q.FEENO == resident.FeeNo);
            ////}
            ////else
            ////{
            ////    result = false;
            ////}
            ////return result;
        }
        #endregion

        #region 营养晒查

        public BaseResponse<IList<LTCNUTRTION72EVAL>> QueryNutrtionEvalExtend(BaseRequest<NutrtionEvalFilter> request)
        {

            BaseResponse<IList<LTCNUTRTION72EVAL>> response = new BaseResponse<IList<LTCNUTRTION72EVAL>>();
            var q = from f in unitOfWork.GetRepository<LTC_NUTRTION72EVAL>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on f.RECORDBY equals e.EMPNO into ees
                    from ee in ees.DefaultIfEmpty()
                    select new LTCNUTRTION72EVAL
                    {
                        ID = f.ID,
                        FEENO = f.FEENO,
                        REGNO = f.REGNO,
                        RECORDDATE = f.RECORDDATE,
                        RECORDBY = f.RECORDBY,
                        CURRENTWEIGHT = f.CURRENTWEIGHT,
                        IDEALWEIGHT = f.IDEALWEIGHT,
                        HEIGHT = f.HEIGHT,
                        BMI = f.BMI,
                        DIETARY = f.DIETARY,
                        FEEDING = f.FEEDING,
                        BREAKFAST = f.BREAKFAST,
                        LUNCH = f.LUNCH,
                        DINNER = f.DINNER,
                        SNACK = f.SNACK,
                        LIKEFOOD = f.LIKEFOOD,
                        NOTLIKEFOOD = f.NOTLIKEFOOD,
                        ALLERGICFOOD = f.ALLERGICFOOD,
                        GASTROINTESTINAL = f.GASTROINTESTINAL,
                        FUNCTIONALEVAL = f.FUNCTIONALEVAL,
                        FATREDUCTION = f.FATREDUCTION,
                        MUSCLEWEAK = f.MUSCLEWEAK,
                        EDEMA = f.EDEMA,
                        ASCITES = f.ASCITES,
                        BEDSORE = f.BEDSORE,
                        BEDSORELEVEL = f.BEDSORELEVEL,
                        EVALRESULT = f.EVALRESULT,
                        ORGID = f.ORGID,
                        CHEW = f.CHEW,
                        SWALLOW = f.SWALLOW,
                    };

            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNo);
            }
            q = q.OrderByDescending(m => m.RECORDDATE);
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

        public BaseResponse<LTCNUTRTION72EVAL> SaveNutrtionEval(LTCNUTRTION72EVAL request)
        {

            if (request.ID == 0)
            {
                request.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_NUTRTION72EVAL, LTCNUTRTION72EVAL>(request, (q) => q.ID == request.ID);
        }


        public BaseResponse<LTCNUTRTION72EVAL> GetNutrtionEval(int Id)
        {
            return base.Get<LTC_NUTRTION72EVAL, LTCNUTRTION72EVAL>((q) => q.ID == Id);
        }

        public BaseResponse DeleteNutrtionEval(int Id)
        {
            return base.Delete<LTC_NUTRTION72EVAL>(Id);
        }
        #endregion

        #region 住民資料 PostCode
        public BaseResponse<IList<ZipFile>> QueryPost(BaseRequest<ZipFileFilter> request)
        {
            BaseResponse<IList<ZipFile>> response = new BaseResponse<IList<ZipFile>>();
            Mapper.CreateMap<LTC_ZIPFILE, ZipFile>();
            var q = from m in unitOfWork.GetRepository<LTC_ZIPFILE>().dbSet
                    select m;
            if (!string.IsNullOrEmpty(request.Data.KeyWord))
            {
                q = q.Where(m => m.POSTCODE.Contains(request.Data.KeyWord) || m.TOWN.Contains(request.Data.KeyWord) || m.CITY.Contains(request.Data.KeyWord));
            }
            q = q.OrderByDescending(m => m.ID);
            response.RecordsCount = q.Count();
            var list = q.OrderBy(x => x.ID).ToList();
            response.Data = Mapper.Map<IList<ZipFile>>(list);
            return response;
        }

        #endregion
    }
}