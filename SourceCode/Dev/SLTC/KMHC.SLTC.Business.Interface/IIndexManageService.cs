using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IIndexManageService : IBaseService
    {
        #region 跌倒
        /// <summary>
        /// 獲取跌倒指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FallIncidentEvent>> QueryFallIncidentEvent(BaseRequest<FallIncidentEventFilter> request);
        /// <summary>
        /// 獲取跌倒指標
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<FallIncidentEvent> GetFallIncidentEvent(long id);
        /// <summary>
        /// 保存跌倒指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FallIncidentEvent> SaveFallIncidentEvent(FallIncidentEvent request);
        /// <summary>
        /// 删除跌倒指標
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteFallIncidentEvent(long id);
        #endregion
        #region 壓瘡
        /// <summary>
        /// 獲取壓瘡指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedSoreRec>> QueryBedSoreRec(BaseRequest<BedSoreRecFilter> request);
        /// <summary>
        /// 獲取壓瘡指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<IList<BedSoreRec>> QueryBedSoreRecExtend(BaseRequest<BedSoreRecFilter> request);
        /// <summary>
        /// 獲取个人最近壓瘡指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<BedSoreRec> GetBedSoreRecExtend(BaseRequest<BedSoreRecFilter> request);
        /// <summary>
        /// 獲取壓瘡指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<BedSoreRec> GetBedSoreRec(long seq);
        /// <summary>
        /// 保存壓瘡指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BedSoreRec> SaveBedSoreRec(BedSoreRec request);
        /// <summary>
        /// 删除壓瘡指標
        /// </summary>
        /// <param name="seq"></param>
        BaseResponse DeleteBedSoreRec(long seq);
        #endregion
        #region 壓瘡明細
        /// <summary>
        /// 獲取壓瘡指標明細列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedSoreChgrec>> QueryBedSoreChgrec(BaseRequest<BedSoreChgrecFilter> request);
        /// <summary>
        /// 獲取个人最近壓瘡指標明細
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<BedSoreChgrec> GetBedSoreChgrecExtend(long feeNo);
        /// <summary>
        /// 獲取壓瘡指標明細
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<BedSoreChgrec> GetBedSoreChgrec(long seq);
        /// <summary>
        /// 保存壓瘡指標明細
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BedSoreChgrec> SaveBedSoreChgrec(BedSoreChgrec request);
        /// <summary>
        /// 删除壓瘡指標明細
        /// </summary>
        /// <param name="seq"></param>
        BaseResponse DeleteBedSoreChgrec(long seq);
        #endregion
        #region 約束
        /// <summary>
        /// 獲取約束指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ConstraintRec>> QueryConstraintRec(BaseRequest<ConstraintRecFilter> request);
        /// <summary>
        /// 獲取約束指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<IList<ConstraintRec>> QueryConstraintRecExtend(BaseRequest<ConstraintRecFilter> request);
         /// <summary>
        /// 獲取約束指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<ConstraintRec> GetConstraintRecExtend(BaseRequest<ConstraintRecFilter> request);
        /// <summary>
        /// 獲取約束指標
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<ConstraintRec> GetConstraintRec(long seqNo);
        /// <summary>
        /// 保存約束指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ConstraintRec> SaveConstraintRec(ConstraintRec request);
        /// <summary>
        /// 删除約束指標
        /// </summary>
        /// <param name="seqNo"></param>
        BaseResponse DeleteConstraintRec(long seqNo);
        #endregion
        #region 約束明細 
        /// <summary>
        /// 獲取約束指標明細列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ConstrainsBeval>> QueryConstrainsBeval(BaseRequest<ConstrainsBevalFilter> request);
        /// <summary>
        /// 獲取个人最近約束指標明細
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<ConstrainsBeval> GetConstrainsBevalExtend(long seqNo);
        /// <summary>
        /// 獲取約束指標明細
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<ConstrainsBeval> GetConstrainsBeval(long id);
        /// <summary>
        /// 保存約束指標明細
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ConstrainsBeval> SaveConstrainsBeval(ConstrainsBeval request);
        /// <summary>
        /// 删除約束指標明細
        /// </summary>
        /// <param name="seq"></param>
        BaseResponse DeleteConstrainsBeval(long id);
        #endregion
        #region 感染指標
        /// <summary>
        /// 獲取感染指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<InfectionInd>> QueryInfectionInd(BaseRequest<InfectionIndFilter> request);
        /// <summary>
        /// 獲取感染指標
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<InfectionInd> GetInfectionInd(long seqNo);
        /// <summary>
        /// 保存感染指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<InfectionInd> SaveInfectionInd(InfectionInd request);
        /// <summary>
        /// 删除感染指標
        /// </summary>
        /// <param name="seqNo"></param>
        BaseResponse DeleteInfectionInd(long seqNo);

        /// <summary>
        /// 获取LTC_INFECTIONSYMPOTM列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<InfectionSympotm>> QueryInfectionSympotm(BaseRequest<InfectionSympotmFilter> request);
        /// <summary>
        /// 获取LTC_INFECTIONSYMPOTM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<InfectionSympotm> GetInfectionSympotm(long id);
        /// <summary>
        /// 保存LTC_INFECTIONSYMPOTM
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<InfectionSympotm> SaveInfectionSympotm(InfectionSympotm request);
        /// <summary>
        /// 删除LTC_INFECTIONSYMPOTM
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteInfectionSympotm(long id);
        /// <summary>
        /// 获取LTC_LABEXAMREC列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LabExamRec>> QueryLabExamRec(BaseRequest<LabExamRecFilter> request);
        /// <summary>
        /// 获取LTC_LABEXAMREC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<LabExamRec> GetLabExamRec(long id);
        /// <summary>
        /// 保存LTC_LABEXAMREC
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<LabExamRec> SaveLabExamRec(LabExamRec request);
        /// <summary>
        /// 删除LTC_LABEXAMREC
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteLabExamRec(long id);


        void SaveInfectionSympotms(List<InfectionSympotm> request);


        void DeleteInfectionSympotms(long[] ids);

        #endregion
        #region 疼痛评估
        /// <summary>
        /// 獲取初步疼痛評估列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PainEvalRec>> QueryPainEvalRec(BaseRequest<PainEvalRecFilter> request);
        /// <summary>
        /// 獲取初步疼痛評估列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PainEvalRec>> QueryPainEvalRecExtend(BaseRequest<PainEvalRecFilter> request);
        /// <summary>
        /// 獲取初步疼痛評估
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<PainEvalRec> GetPainEvalRecExtend(BaseRequest<PainEvalRecFilter> request);
        /// <summary>
        /// 獲取初步疼痛評估
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<PainEvalRec> GetPainEvalRec(long seqNo);
        /// <summary>
        /// 保存初步疼痛評估
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<PainEvalRec> SavePainEvalRec(PainEvalRec request);
        /// <summary>
        /// 删除初步疼痛評估
        /// </summary>
        /// <param name="seqNo"></param>
        BaseResponse DeletePainEvalRec(long seqNo);
        #endregion
        #region 疼痛詳細
        /// <summary>
        /// 獲取疼痛明細指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PainBodyPartRec>> QueryPainBodyPartRec(BaseRequest<PainBodyPartRecFilter> request);
        /// <summary>
        /// 獲取个人最近疼痛明細指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<PainBodyPartRec> GetPainBodyPartRecExtend(long seq);
        /// <summary>
        /// 獲取疼痛明細指標
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        BaseResponse<PainBodyPartRec> GetPainBodyPartRec(long id);
        /// <summary>
        /// 保存疼痛明細指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<PainBodyPartRec> SavePainBodyPartRec(PainBodyPartRec request);
        /// <summary>
        /// 删除疼痛明細指標
        /// </summary>
        /// <param name="seq"></param>
        BaseResponse DeletePainBodyPartRec(long seq);
        #endregion
        #region 非計劃性減重
        /// <summary>
        /// 獲取非計劃性減重指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<UnPlanWeightInd>> QueryUnPlanWeightInd(BaseRequest<UnPlanWeightIndFilter> request);
        /// <summary>
        /// 獲取非計劃性減重指標
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<UnPlanWeightInd> GetUnPlanWeightInd(long id);
        /// <summary>
        /// 保存非計劃性減重指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<UnPlanWeightInd> SaveUnPlanWeightInd(UnPlanWeightInd request);
        /// <summary>
        /// 删除非計劃性減重指標
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteUnPlanWeightInd(long id);
        #endregion
        #region 非計劃性住院
        /// <summary>
        /// 獲取非計劃性住院指標列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<UnPlanEdipd>> QueryUnPlanEdipd(BaseRequest<UnPlanEdipdFilter> request);

        /// <summary>
        /// 獲取非計劃性住院指標列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>集合</returns>
        BaseResponse<IList<UnPlanEdipd>> QueryNewUnPlanEdipd(BaseRequest<UnPlanEdipdFilter> request);
        
        /// <summary>
        /// 獲取非計劃性住院指標
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<UnPlanEdipd> GetUnPlanEdipd(long id);
        /// <summary>
        /// 保存非計劃性住院指標
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<UnPlanEdipd> SaveUnPlanEdipd(UnPlanEdipd request);
        /// <summary>
        /// 删除非計劃性住院指標
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteUnPlanEdipd(long id);
        #endregion

        #region 感染項目
        /// <summary>
        /// 獲取感染項目列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<InfectionItem>> QueryInfectionItem(BaseRequest<InfectionItemFilter> request);
        #endregion
        #region 感染症狀
        /// <summary>
        /// 獲取感染症狀列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<SymptomItem>> QuerySymptomItem(BaseRequest<SymptomItemFilter> request);
        #endregion


    }
}
