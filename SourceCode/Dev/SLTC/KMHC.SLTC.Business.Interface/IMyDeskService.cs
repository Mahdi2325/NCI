using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface {

 public interface IMyDeskService:IBaseService 
 {

	 #region 桌面數量查詢接口

	 /// <summary>
     /// 獲取當前入住人數統計
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<Resident>> QueryResidentList();

	 /// <summary>
	 /// 非計劃住民列表，并返回非計劃住民人數
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_UNPLANEDIPD>> QueryUnPlanList();


	 /// <summary>
	 /// 獲取請假人數，并返回請假住民人數
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_LEAVEHOSP>> QueryLeaveHospList();

	 /// <summary>
	 /// 獲取跌倒住名列表，并返回住民列表數
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_FALLINCIDENTEVENT>> QueryFallPersonList();


	 /// <summary>
	 /// 獲取壓瘡住民列表，并返回住民列表數
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_BEDSOREREC>> QueryPrePersonList();


	 /// <summary>
	 /// 查詢工作照會,并返回數量
	 /// </summary>
	 /// <returns></returns>
     object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request);

     object QueryKPI();

	 #endregion


	 #region 幾張圖形分析綁定接口




	 #endregion

 }

}
