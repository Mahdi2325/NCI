using AutoMapper;
using KMHC.Infrastructure;
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

/*
   測試類
   孫偉 新增新桌面類
 */

namespace KMHC.SLTC.Business.Implement {

	public class myDeskService : BaseService, IMyDeskService 
	{

		/// <summary>
		/// 獲取住民信息列表并返回記錄數
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<Resident>> QueryResidentList() {

			BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
			var q = from n in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
					join e in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on n.REGNO equals e.REGNO
					select new {
						regFile = n,
						ipdReg = e
					};
			q = q.Where(m => m.regFile.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.ipdReg.IPDFLAG == "I");
			response.RecordsCount = q.Count();
			return response;
		}

		/// <summary>
		/// 獲取非計劃住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_UNPLANEDIPD>> QueryUnPlanList() {

			BaseResponse<IList<LTC_UNPLANEDIPD>> response = new BaseResponse<IList<LTC_UNPLANEDIPD>>();

			var q = from n in unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet
					join e in unitOfWork.GetRepository<LTC_REGHEALTH>().dbSet on n.FEENO equals e.FEENO
					select new {
						UnPlanFile = n,
						regHealth = e
					};
			q = q.Where(m => m.UnPlanFile.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;

		}

		/// <summary>
		/// 獲取請假住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_LEAVEHOSP>> QueryLeaveHospList() 
		{
			BaseResponse<IList<LTC_LEAVEHOSP>> response = new BaseResponse<IList<LTC_LEAVEHOSP>>();

			var q = from n in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet
					select new {
						LeaveHosp = n
						
					};
			q = q.Where(m => m.LeaveHosp.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;
		}

		/// <summary>
		/// 獲取跌倒指標住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_FALLINCIDENTEVENT>> QueryFallPersonList() {

			BaseResponse<IList<LTC_FALLINCIDENTEVENT>> response = new BaseResponse<IList<LTC_FALLINCIDENTEVENT>>();

			var q = from n in unitOfWork.GetRepository<LTC_FALLINCIDENTEVENT>().dbSet
					join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO
					select new {
						FALLIN = n,

					};
			q = q.Where(m => m.FALLIN.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;
		}
		/// <summary>
		///  獲取壓瘡指標住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_BEDSOREREC>> QueryPrePersonList() {
			BaseResponse<IList<LTC_BEDSOREREC>> response = new BaseResponse<IList<LTC_BEDSOREREC>>();

			var q = from n in unitOfWork.GetRepository<LTC_BEDSOREREC>().dbSet
					select new {
						Pre = n
					};
			q = q.Where(m => m.Pre.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();

			return response;
		}

		/// <summary>
		/// 查詢工作照會，并返回工作照會的數量
		/// </summary>
		/// <returns></returns>
        public object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request)
		{

            var q = from n in unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on n.FEENO equals ipd.FEENO into ipd_Reg
                    from ipdReg in ipd_Reg.DefaultIfEmpty()
                    join regf in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdReg.REGNO equals regf.REGNO into reg_f
                    from reg_file in reg_f.DefaultIfEmpty()
                    where n.ASSIGNEE == "" + SecurityHelper.CurrentPrincipal.EmpNo + ""
                    select new
                    {
                        id = n.ID,
                        title = reg_file.NAME == null ? n.CONTENT : "(" + reg_file.NAME + ")" + n.CONTENT,
                        start=n.PERFORMDATE,
                        regName=reg_file.NAME
                    };
           
            q = q.Where(m => m.start>= request.Data.start&&m.start<=request.Data.end);
            return q;

		}
        public object QueryKPI()
        {
           string sql = @"SELECT count(*) NUM FROM LTC_IPDREG where IPDFLAG='I' and ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' UNION ALL SELECT COUNT(DISTINCT(FEENO)) FROM LTC_LEAVEHOSP WHERE ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' and (YEARWEEK(date_format(STARTDATE, '%Y-%m-%d')) = YEARWEEK(now()) OR YEARWEEK(date_format(ENDDATE, '%Y-%m-%d')) = YEARWEEK(now())) UNION ALL SELECT count(*) FROM LTC_UNPLANEDIPD WHERE YEARWEEK(date_format(INDATE, '%Y-%m-%d')) = YEARWEEK(now()) and ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' UNION ALL SELECT count(*) FROM LTC_FALLINCIDENTEVENT WHERE YEARWEEK(date_format(EVENTDATE, '%Y-%m-%d')) = YEARWEEK(now()) and ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' UNION ALL SELECT count(*) FROM LTC_BEDSOREREC WHERE YEARWEEK(date_format(OCCURDATE, '%Y-%m-%d')) = YEARWEEK(now()) and ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' UNION ALL SELECT count(*) FROM LTC_ASSIGNTASK a left JOIN LTC_IPDREG i ON a.FEENO = i.FEENO left JOIN LTC_REGFILE r ON i.REGNO = r.REGNO where a.ASSIGNEE='" + SecurityHelper.CurrentPrincipal.EmpNo + "' and a.NEWRECFLAG=1 and a.ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "';";
           List<MyDeskKPI> list=  unitOfWork.GetRepository<MyDeskKPI>().SqlQuery(sql).ToList();
           return list;
        }
	}
}
