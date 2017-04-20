/*
创建人: 肖国栋
创建日期:2016-03-09
说明:护理工作站
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.DC.Model;
using System;

namespace KMHC.SLTC.Business.Interface
{
    public interface INursingWorkstationService : IBaseService
    {
        /// <summary>
        /// 获取生命体征列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Vitalsign>> QueryVitalsign(BaseRequest<VitalsignFilter> request);
        /// <summary>
        /// 获取生命体征
        /// </summary>
        /// <param name="seqNO"></param>
        /// <returns></returns>
        BaseResponse<Vitalsign> GetVitalsign(long seqNO);
        /// <summary>
        /// 获取生命体征
        /// </summary>
        /// <param name="seqNO"></param>
        /// <returns></returns>
        BaseResponse<Vitalsign> GetVitalsignToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE);
        /// <summary>
        /// 保存生命体征  
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Vitalsign> SaveVitalsign(Vitalsign request);
        /// <summary>
        /// 批量保存生命体征
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<List<Vitalsign>> SaveVitalsign(List<Vitalsign> request);
        /// <summary>
        /// 删除生命体征
        /// </summary>
        /// <param name="seqNO"></param>
        BaseResponse DeleteVitalsign(long seqNO);
        /// <summary>
        /// 获取输出量列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OutValueModel>> QueryOutValue(BaseRequest<OutValueFilter> request);
        /// <summary>
        /// 获取输出量
        /// </summary>
        /// <param name="outNo"></param>
        /// <returns></returns>
        BaseResponse<OutValueModel> GetOutValue(long outNo);
        /// <summary>
        /// 获取输出量
        /// </summary>
        /// <param name="outNo"></param>
        /// <returns></returns>
        BaseResponse<OutValueModel> GetOutValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE);
        /// <summary>
        /// 保存输出量
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OutValueModel> SaveOutValue(OutValueModel request);
        /// <summary>
        /// 批量输出量
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<List<OutValueModel>> SaveOutValue(List<OutValueModel> request);
        /// <summary>
        /// 删除输出量
        /// </summary>
        /// <param name="outNo"></param>
        BaseResponse DeleteOutValue(long outNo);
        /// <summary>
        /// 获取输入量列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<InValueModel>> QueryInValue(BaseRequest<InValueFilter> request);
        /// <summary>
        /// 获取输入量
        /// </summary>
        /// <param name="inNo"></param>
        /// <returns></returns>
        BaseResponse<InValueModel> GetInValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE);
        /// <summary>
        /// 获取输入量
        /// </summary>
        /// <param name="inNo"></param>
        /// <returns></returns>
        BaseResponse<InValueModel> GetInValue(long inNo);
        /// <summary>
        /// 保存输入量
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<InValueModel> SaveInValue(InValueModel request);
        /// <summary>
        /// 批量输入量
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<List<InValueModel>> SaveInValue(List<InValueModel> request);
        /// <summary>
        /// 删除输入量
        /// </summary>
        /// <param name="inNo"></param>
        BaseResponse DeleteInValue(long inNo);
        /// <summary>
        /// 获取護理記錄列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NursingRec>> QueryNursingRec(BaseRequest<NursingRecFilter> request);
        /// <summary>
        /// 获取護理記錄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NursingRec> GetNursingRec(long id);
        /// <summary>
        /// 保存護理記錄
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NursingRec> SaveNursingRec(NursingRec request);
        /// <summary>
        /// 删除護理記錄
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteNursingRec(long id);
        /// <summary>
        /// 获取護理交班列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NursingHandover>> QueryNursingHandover(BaseRequest<NursingHandoverFilter> request);
        /// <summary>
        /// 获取護理交班
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NursingHandover> GetNursingHandover(long id);
        /// <summary>
        /// 保存護理交班
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NursingHandover> SaveNursingHandover(NursingHandover request);
        /// <summary>
        /// 批量保存護理交班
        /// </summary>
        /// <param name="request"></param>
        BaseResponse SaveMulNursingHandover(List<NursingHandover> request);
        /// <summary>
        /// 删除護理交班
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteNursingHandover(long id);
        /// <summary>
        /// 获取行政交班列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<AffairsHandover>> QueryAffairsHandover(BaseRequest<AffairsHandoverFilter> request);
        /// <summary>
        /// 获取行政交班列表擴展
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<AffairsHandover>> QueryAffairsHandoverExtend(BaseRequest<AffairsHandoverFilter> request);
        /// <summary>
        /// 获取行政交班
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<AffairsHandover> GetAffairsHandover(long id);
        /// <summary>
        /// 保存行政交班
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<AffairsHandover> SaveAffairsHandover(AffairsHandover request);
        /// <summary>
        /// 保存行政交班
        /// </summary>
        /// <param name="request"></param>
        BaseResponse SaveMulAffairsHandover(List<AffairsHandover> request);
        /// <summary>
        /// 删除行政交班
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteAffairsHandover(long id);
        /// <summary>
        /// 获取工作照會列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<AssignTask>> QueryAssignTask(BaseRequest<AssignTaskFilter> request);
        /// <summary>
        /// 获取工作照會
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<AssignTask> GetAssignTask(long id);
        /// <summary>
        /// 保存工作照會
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<AssignTask> SaveAssignTask(AssignTask request);
        /// <summary>
        /// 扩展保存工作照會
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AssignTask> SaveAssignTask2(AssignTask2 request);

        /// <summary>
        /// 更新完成状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="recStatus">recStatus</param>
        /// <param name="finishDate">finishDate</param>
        /// <returns>BaseResponse</returns>
        BaseResponse ChangeRecStatus(int id, bool? recStatus, DateTime? finishDate, bool? newrecFlag);

        /// <summary>
        /// 更新未读状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newRecFlag">newRecFlag</param>
        /// <returns>BaseResponse</returns>
        BaseResponse ChangeNewRecStatus(int id, bool? newRecFlag);
        
        /// <summary>
        /// 批量保存工作照會
        /// </summary>
        /// <param name="request"></param>
        BaseResponse SaveAssignTask(List<AssignTask> request);
        /// <summary>
        /// 删除工作照會
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteAssignTask(long id);

        /// <summary>
        ///  重新分配工作照会
        /// </summary>
        /// <param name="oldTask"></param>
        /// <param name="empList"></param>
        /// <returns></returns>
        BaseResponse ReAssignTask(AssignTask oldTask, IList<TaskEmpFile> empList);
        /// <summary>
        /// 获取醫師評估列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DoctorEvalRec>> QueryDocEvalRecData(BaseRequest<DoctorEvalRecFilter> request);
        /// <summary>
        /// 获取醫師評估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<DoctorEvalRec> GetDocEvalRecData(long id);
        /// <summary>
        /// 保存醫師評估
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<DoctorEvalRec> SaveDocEvalRecData(DoctorEvalRec request);
        /// <summary>
        /// 删除醫師評估
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteDocEvalRecData(long id);

        /// <summary>
        /// 获取醫師巡診列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DoctorCheckRec>> QueryDocCheckRecData(BaseRequest<DoctorCheckRecFilter> request);
        /// <summary>
        /// 获取醫師巡診
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<DoctorCheckRec> GetDocCheckRecData(long id);
        /// <summary>
        /// 保存醫師巡診
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<DoctorCheckRec> SaveDocCheckRecData(DoctorCheckRec request);
        /// <summary>
        /// 删除醫師巡診
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteDocCheckRecData(long id);

        /// <summary>
        /// 查詢用藥記錄
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitPrescription>> QueryVisitPrescription(BaseRequest<VisitPrescriptionFilter> request);
        #region 團隊活動
        /// <summary>
        /// 获取團隊活動列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<GroupActivityRec>> QueryGroupActivityRec(BaseRequest<GroupActivityRecFilter> request);
        /// <summary>
        /// 获取團隊活動
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<GroupActivityRec> GetGroupActivityRec(int id);
        /// <summary>
        /// 保存團隊活動
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<GroupActivityRec> SaveGroupActivityRec(GroupActivityRec request);
        /// <summary>
        /// 删除團隊活動
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteGroupActivityRec(int id);
        #endregion
    }
}