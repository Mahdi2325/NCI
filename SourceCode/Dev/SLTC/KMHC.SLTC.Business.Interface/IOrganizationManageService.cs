/*
创建人: 肖国栋
创建日期:2016-03-09
说明:机构管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;

namespace KMHC.SLTC.Business.Interface
{
    public interface IOrganizationManageService : IBaseService
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Organization>> QueryOrg(BaseRequest<OrganizationFilter> request);
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        BaseResponse<Organization> GetOrg(string orgID);
        /// <summary>
        /// 保存机构
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Organization> SaveOrg(Organization request);
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="orgID"></param>
        BaseResponse DeleteOrg(string orgID);
        /// <summary>
        /// 获取楼层基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgFloor>> QueryOrgFloor(BaseRequest<OrgFloorFilter> request);
        /// <summary>
        /// 获取楼层基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgFloor>> QueryOrgFloorExtend(BaseRequest<OrgFloorFilter> request);
        /// <summary>
        /// 获取楼层基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<OrgFloor> GetOrgFloor(string bedNO);
        /// <summary>
        /// 保存楼层基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OrgFloor> SaveOrgFloor(OrgFloor request);
        /// <summary>
        /// 删除楼层基本
        /// </summary>
        BaseResponse DeleteOrgFloor(string bedNO);
        /// <param name="bedNO"></param>
        /// <summary>
        /// 获取房间基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoom(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoomExtend(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<OrgRoom> GetOrgRoom(string bedNO);
        /// <summary>
        /// 保存房间基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OrgRoom> SaveOrgRoom(OrgRoom request);
        /// <summary>
        /// 删除房间基本
        /// </summary>
        /// <param name="bedNO"></param>
        BaseResponse DeleteOrgRoom(string bedNO);
        /// <summary>
        /// 获取床位基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedBasic>> QueryBedBasic(BaseRequest<BedBasicFilter> request);
        /// <summary>
        /// 获取床位基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedBasic>> QueryBedBasicExtend(BaseRequest<BedBasicFilter> request);
        /// <summary>
        /// 获取床位基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<BedBasic> GetBedBasic(string bedNO);
        /// <summary>
        /// 保存床位基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BedBasic> SaveBedBasic(BedBasic request);
        /// <summary>
        /// 床位被使用，更新床位状态
        /// </summary>
        /// <param name="request"></param>
        BaseResponse UpdateBedBasic(BedBasic request);
        /// <summary>
        /// 删除床位基本
        /// </summary>
        /// <param name="bedNO"></param>
        BaseResponse DeleteBedBasic(string bedNO);
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Role>> QueryRole(BaseRequest<RoleFilter> request);
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        BaseResponse<Role> GetRole(string roleID);

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Role> SaveRole(Role request);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID"></param>
        BaseResponse DeleteRole(string roleID);
        /// <summary>
        /// 根據角色獲取模塊樹的數據
        /// </summary>
        /// <param name="requestByRole">選擇的模塊</param>
        /// <param name="requestByTree">展現的模塊</param>
        /// <returns></returns>

        #region Org
        BaseResponse<IList<Agency>> QueryOrgAgency(BaseRequest<ORGFilter> request);
        BaseResponse<IList<NursingHome>> QueryOrgNsHome(BaseRequest<ORGFilter> request);
        BaseResponse<IList<Insurance>> QueryOrgInHome(BaseRequest<ORGFilter> request);

        #endregion

        #region User
        /// <summary>
        /// 经办机构人员 信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NCI_User>> QueryUserExtend(BaseRequest<NCI_UserFilter> request);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NCI_User>> QueryUser(BaseRequest<NCI_UserFilter> request);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        BaseResponse<NCI_User> QueryUserByAccount(string account);
        BaseResponse<NCI_User> GetUser(int userID);
        BaseResponse<List<NCI_User>> GetUsreByRoleType(string orgId, string roleType);
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NCI_User> SaveUser(NCI_User request);
        BaseResponse DeleteUser(int userID);
        BaseResponse ChangePassWord(int userId, string oldPassword, string newPassword);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="logonName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        BaseResponse ResetPassword(string orgID, string logonName, string password);
        #endregion
        BaseResponse<IList<TreeNode>> GetModuleByRole(BaseRequest<RoleFilter> requestByRole, BaseRequest<RoleFilter> requestByTree);
        /// <summary>
        /// 获取員工列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Employee>> QueryEmployee(BaseRequest<EmployeeFilter> request);
        /// <summary>
        /// 获取員工聯合用戶信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Employee>> QueryUserUnionEmp(BaseRequest<EmployeeFilter> request);
        /// <summary>
        /// 获取員工
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        BaseResponse<Employee> GetEmployee(string empNo);
        /// <summary>
        /// 保存員工
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Employee> SaveEmployee(Employee request);
        /// <summary>
        /// 删除員工
        /// </summary>
        /// <param name="empNo"></param>
        BaseResponse DeleteEmployee(string empNo, string orgId);
        /// <summary>
        /// 获取部門列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Dept>> QueryDept(BaseRequest<DeptFilter> request);
        /// <summary>
        /// 获取部門列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Dept>> QueryDeptExtend(BaseRequest<DeptFilter> request);
        /// <summary>
        /// 获取部門
        /// </summary>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        BaseResponse<Dept> GetDept(string deptNo);
        /// <summary>
        /// 保存部門
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Dept> SaveDept(Dept request);
        /// <summary>
        /// 删除部門
        /// </summary>
        /// <param name="deptNo"></param>
        BaseResponse DeleteDept(string deptNo, string orgId);

        /// <summary>
        /// 获取集團列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Groups>> QueryGroup(BaseRequest<GroupFilter> request);
        /// <summary>
        /// 获取集團
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        BaseResponse<Groups> GetGroup(string GroupID);
        /// <summary>
        /// 保存集團
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Groups> SaveGroup(Groups request);
        /// <summary>
        /// 删除集團
        /// </summary>
        /// <param name="roleID"></param>
        BaseResponse DeleteGroup(string GroupID);


        /// <summary>
        /// 依据角色Id获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<Module> GetRoleModule(RoleFilter request);


        /// <summary>
        /// 依据角色Id获取菜单树列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<MenuTree> GetMenus(RoleFilter request);


        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CodeFile>> QueryCodeFile(BaseRequest<CommonFilter> request);
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CodeFile> GetCodeFile(string id);
        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CodeFile> SaveCodeFile(CodeFile request);
        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCodeFile(string id);

        /// <summary>
        /// 获取字典小项列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CodeDtl>> QueryCodeDtl(BaseRequest<CommonFilter> request);

        /// <summary>
        /// 获取字典小项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        BaseResponse<CodeDtl> GetCodeDtl(string id, string type);
        /// <summary>
        /// 保存字典小项
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CodeDtl> SaveCodeDtl(CodeDtl request);

        /// <summary>
        /// 删除字典小项
        /// </summary>
        /// <param name="id"></param>
        int DeleteCodeDtl(string id, string type);



        /// <summary>
        /// 获取院内公告列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Notice>> QueryNotices(BaseRequest<NoticeFilter> request);

        /// <summary>
        /// 获取院内公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Notice> GetNotice(int id);

        /// <summary>
        /// 保存公告
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Notice> SaveNotice(Notice request);
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteNotice(int id);

        /// <summary>
        /// 查詢評估量表List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_Question>> QueryQueList(BaseRequest<QuestionFilter> request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_Question>> QueryEvalTempSetList(BaseRequest<QuestionFilter> request);
        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<LTC_Question> GetQue(int id);
        /// <summary>
        /// 評估問題List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_MakerItem>> QueryMakerItemList(BaseRequest<MakerItemFilter> request);
        /// <summary>
        /// 評估結果List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_QuestionResults>> QueryQuestionResultsList(BaseRequest<QuestionResultsFilter> request);
        /// <summary>
        /// 保存評估量表設置
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<List<EvalTempSetModel>> SaveEvalTemplateSet(string orgId, List<EvalTempSetModel> request);
        /// <summary>
        /// 保存评估表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_Question> SaveQuestion(LTC_Question request);

        /// <summary>
        /// 導入問題數據
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        BaseResponse SaveQuestionModleData(int questionId, string orgId, int exportQuestionId);
        /// <summary>
        /// 導入結果數據
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="orgId"></param>
        /// <param name="exportQuestionId"></param>
        /// <returns></returns>
        BaseResponse SaveResultModleData(int questionId, string orgId, int exportQuestionId);
        /// <summary>
        /// 保存評估問題數據
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_MakerItem> SaveMakerItem(LTC_MakerItem request);
        /// <summary>
        /// 保存評估結果數據
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_QuestionResults> SaveQuestionResults(LTC_QuestionResults request);
        /// <summary>
        /// 保存答案數據
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_MakerItemLimitedValue> SaveAnswer(LTC_MakerItemLimitedValue request);
        /// <summary>
        /// 刪除評估結果數據
        /// </summary>
        /// <param name="ResultId"></param>
        /// <returns></returns>
        BaseResponse DeleteQuestionResults(int ResultId);
        /// <summary>
        /// 刪除評估問題答案
        /// </summary>
        /// <param name="LimitedValueId"></param>
        /// <returns></returns>
        BaseResponse DeleteAnswer(int LimitedValueId);

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="MakerId"></param>
        /// <returns></returns>
        BaseResponse DeleteMakerItem(int MakerId);
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BaseResponse DeleteQuestion(int Id);
        BaseResponse<Role> SaveRoleNew(Role request);
    }
}
