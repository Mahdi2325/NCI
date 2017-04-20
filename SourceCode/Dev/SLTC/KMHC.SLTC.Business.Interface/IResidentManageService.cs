/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace KMHC.SLTC.Business.Interface
{
    public interface IResidentManageService
    {
        /// <summary>
        /// 获取住民信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Person>> QueryPerson(BaseRequest<PersonFilter> request);
        /// <summary>
        /// 获取住民扩展信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Person>> QueryPersonExtend(BaseRequest<PersonFilter> request);
        /// <summary>
        /// 获取住民信息
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<Person> GetPerson(int regNo);
        /// <summary>
        /// 保存住民信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Person> SavePerson(Person request);
        /// <summary>
        /// 删除住民信息
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeletePerson(int regNo);
        /// <summary>
        /// 获取入住信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResident(BaseRequest<ResidentFilter> request);
        /// <summary>
        /// 获取入住扩展信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResidentExtend(BaseRequest<ResidentFilter> request);
        /// <summary>
        /// 根據住民姓名獲得同名住民列表（姓名完全匹配）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResidentByName(BaseRequest<ResidentFilter> request);
        /// <summary>
        /// 获取入住信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Resident> GetResident(long feeNo);
        /// <summary>
        /// 保存入住信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Resident> SaveResident(Resident request);
        /// <summary>
        /// 删除入住信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteResident(long feeNo);
        /// <summary>
        /// 获取入住審核信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Verify>> QueryVerify(BaseRequest<VerifyFilter> request);
        /// <summary>
        /// 获取出入住審核信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Verify> GetVerify(long feeNo);
        /// <summary>
        /// 保存入住審核信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Verify> SaveVerify(Verify request);
        /// <summary>
        /// 删除入住審核信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteVerify(long feeNo);
        /// <summary>
        /// 获取请假记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LeaveHosp>> QueryLeaveHosp(BaseRequest<LeaveHospFilter> request);

        /// <summary>
        /// 查询住民最新一笔请假记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>离院记录</returns>
        BaseResponse<IList<LeaveHosp>> GetNewLeaveHosp(BaseRequest<LeaveHospFilter> request);
        /// <summary>
        /// 获取请假记录信息
        /// </summary>
        /// <param name="leaveHospId"></param>
        /// <returns></returns>
        BaseResponse<LeaveHosp> GetLeaveHosp(long leaveHospId);
        /// <summary>
        /// 保存请假记录信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<LeaveHosp> SaveLeaveHosp(LeaveHosp request);
        /// <summary>
        /// 删除请假记录信息
        /// </summary>
        /// <param name="leaveHospId"></param>
        BaseResponse DeleteLeaveHosp(long leaveHospId);
        /// <summary>
        /// 获取零用金列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Deposit>> QueryDeposit(BaseRequest<DepositFilter> request);
        /// <summary>
        /// 获取零用金信息
        /// </summary>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        BaseResponse<Deposit> GetDeposit(string deptNo);
        /// <summary>
        /// 保存零用金信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Deposit> SaveDeposit(Deposit request);
        /// <summary>
        /// 删除零用金信息
        /// </summary>
        /// <param name="deptNo"></param>
        BaseResponse DeleteDeposit(string deptNo);
        /// <summary>
        /// 获取出院結案信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CloseCase>> QueryCloseCase(BaseRequest<CloseCaseFilter> request);
        /// <summary>
        /// 获取出院結案信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<CloseCase> GetCloseCase(long feeNo);
        /// <summary>
        /// 保存出院結案信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CloseCase> SaveCloseCase(CloseCase request);
        /// <summary>
        /// 删除出院結案信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteCloseCase(long feeNo);
        /// <summary>
        /// 获取社會福利信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ResidentDtl>> QueryResidentDtl(BaseRequest<ResidentDtlFilter> request);
        /// <summary>
        /// 获取社會福利信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<ResidentDtl> GetResidentDtl(long feeNo);
        /// <summary>
        /// 保存社會福利信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ResidentDtl> SaveResidentDtl(ResidentDtl request);
        /// <summary>
        /// 删除社會福利信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteResidentDtl(long feeNo);
        /// <summary>
        /// 获取需求管理信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Demand>> QueryDemand(BaseRequest<DemandFilter> request);
        /// <summary>
        /// 获取需求管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Demand> GetDemand(long id);
        /// <summary>
        /// 保存需求管理信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Demand> SaveDemand(Demand request);
        /// <summary>
        /// 删除需求管理信息
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteDemand(long id);
        /// <summary>
        /// 获取健康管理信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Health>> QueryHealth(BaseRequest<HealthFilter> request);
        /// <summary>
        /// 获取	健康管理信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Health> GetHealth(long feeNo);
        /// <summary>
        /// 保存健康管理信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Health> SaveHealth(Health request);
        /// <summary>
        /// 删除健康管理信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteHealth(long feeNo);
        /// <summary>
        /// 获取	附加檔案信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<AttachFile>> QueryAttachFile(BaseRequest<AttachFileFilter> request);
        /// <summary>
        /// 获取附加檔案信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<AttachFile> GetAttachFile(long feeNo);
        /// <summary>
        /// 保存附加檔案信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<AttachFile> SaveAttachFile(AttachFile request);
        /// <summary>
        /// 批量保存附加檔案信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="request"></param>
        BaseResponse<List<AttachFile>> SaveAttachFile(long feeNo, List<AttachFile> request);
        /// <summary>
        /// 删除附加檔案信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteAttachFile(long feeNo);
        /// <summary>
        /// 获取通信錄住民地址信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Relation>> QueryRelation(BaseRequest<RelationFilter> request);
        /// <summary>
        /// 获取通信錄住民地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Relation> GetRelation(long feeNo);
        /// <summary>
        /// 保存通信錄住民地址信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Relation> SaveRelation(Relation request);
        /// <summary>
        /// 删除通信錄住民地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteRelation(long feeNo);
        /// <summary>
        /// 获取通信錄亲属地址信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<RelationDtl>> QueryRelationDtl(BaseRequest<RelationDtlFilter> request);
        /// <summary>
        /// 获取通信錄亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<RelationDtl> GetRelationDtl(long feeNo);
        /// <summary>
        /// 保存通信錄亲属地址信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<RelationDtl> SaveRelationDtl(RelationDtl request);
        /// <summary>
        /// 批量保存通信錄亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="request"></param>
        BaseResponse<List<RelationDtl>> SaveRelationDtl(long feeNo, List<RelationDtl> request);
        /// <summary>
        /// 删除通信錄亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteRelationDtl(long feeNo);

        /// <summary>
        /// 是否住民已登记过
        /// </summary>
        /// <param name="regNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ExistResident(long regNo,string[] status);

        /// <summary>
        /// 获取住民訪視列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscuss(BaseRequest<FamilyDiscussFilter> request);
        /// <summary>
        /// 获取住民訪視列表拓展
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscussExtend(BaseRequest<FamilyDiscussFilter> request);
        /// <summary>
        /// 获取营养筛查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTCNUTRTION72EVAL>> QueryNutrtionEvalExtend(BaseRequest<NutrtionEvalFilter> request);
 
        /// <summary>
        /// 保存营养筛查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTCNUTRTION72EVAL> SaveNutrtionEval(LTCNUTRTION72EVAL request);
        /// <summary>
        /// 取某条营养筛查记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BaseResponse<LTCNUTRTION72EVAL> GetNutrtionEval(int Id);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        BaseResponse DeleteNutrtionEval(int Id); 
        /// <summary>
        /// 获取住民訪視
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<FamilyDiscuss> GetFamilyDiscuss(int Id);
        /// <summary>
        /// 保存住民訪視
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FamilyDiscuss> SaveFamilyDiscuss(FamilyDiscuss request);
        /// <summary>
        /// 删除住民訪視
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeleteFamilyDiscuss(int Id);
        /// <summary>
        /// 獲取預約登記信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Preipd>> QueryPreipd(BaseRequest<PreipdFilter> request);
        /// <summary>
        /// 保存預約登記信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Preipd> SavePreipd(Preipd request);
        /// <summary>
        /// 刪除預約登記信息
        /// </summary>
        /// <param name="PreFeeNo"></param>
        /// <returns></returns>
        BaseResponse DeletePreipd(long PreFeeNo);
        /// <summary>
        /// 獲取出院辦理信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Ipdregout> GetIpdregout(long feeNo);
        /// <summary>
        /// 保存出院辦理信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Ipdregout> SaveIpdregout(Ipdregout request);
        /// <summary>
        /// 获取住民退住院信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Resident> GetLeaveNursing(long feeNo);
        /// <summary>
        /// 获取住民退住院床位信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<BedBasic> GetLeaveNursingBedInfo(long feeNo);
        /// <summary>
        /// 保存住民退住院信息
        /// </summary>
        /// <param name="resident"></param>
        /// <returns></returns>
        BaseResponse<Resident> SaveLeaveNursing(Resident resident);
        /// <summary>
        /// 獲取Post數據
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ZipFile>> QueryPost(BaseRequest<ZipFileFilter> request);
    }
}