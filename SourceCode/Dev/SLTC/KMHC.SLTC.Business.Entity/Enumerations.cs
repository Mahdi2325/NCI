using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace KMHC.SLTC.Business.Entity
{
    public enum EnumResponseStatus
    {
        Unauthorized = -2,
        ExceptionHappened = -1,
        Success = 0
    }

    public enum EnumCodeKey
    {
        Demo,
        RegNo,
        OrgId,
        GroupId,
        BillNo,
        PayBillNo,
        RoleId,
        StaffId,
        LeaveHospId,
        ManufactureNo,
        GoodNo,
        GoodsLoanNo,
        GoodsSaleNo,
        EmpSysPre,//累加前綴,用於生產每個機構的固定前綴
        EmpPrefix,//員工編號前綴
        QuestionId,//Add by Duke
        LimitedId,//Add by Duke
        ApplicantId,//Add by Amaya
        AppcertSN,//Add by Duke 资格申请流水号
        AppHospSN,//add by amaya  住院申请流水号
        CertNo,//Add by Duke 资格证书流水号
        GrantId,
        serviceId,
    }

    /// <summary>
    /// 固定資費週期
    /// </summary>
    public enum EnumPeriod
    {
        Day,
        Month
    }

    public enum EnumBillState
    {
        [DescriptionAttribute("新产生账单")]
        /// <summary>
        /// 新產生賬單
        /// </summary>
        Open,
        [DescriptionAttribute("关账账单")]
        /// <summary>
        /// 關賬
        /// </summary>
        Close,
        [DescriptionAttribute("取消账单")]
        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }



    public enum MajorType
    {
        社工照顧計劃 = 1,
        護理照顧計劃 = 2
    }
    /// <summary>
    /// 住院资格申请状态
    ///  0：未提交   （数据保存，但尚未提交）
    ///  1：已撤回   （提交后，下一级机构尚未审批，撤回）
    ///  3: 待审核    （已提交，待上一级机构审核）
    ///  6：审核通过 
    ///  9：审核不通过
    /// </summary>
    public enum EnumAppCertStatus
    {
        未提交 = 0,   //0   0-10 定点机构使用
        已撤回 = 1,   //1
        待承办机构审核=2,  //已提交 20  承办机构进行审核  20-29 承办机构使用
        待审核 = 3,//承办机构审核通过,待经办机构审核30 ,  承办机构审核不通过 8   30-39 经办机构使用 
        审核通过 = 6, //经办机构审核通过  40 
        审核不通过 = 9,  // 经办机构审核不通过]
        重新审核 = 11
    }
    /// <summary>
    /// 入院申请状态
    ///  0：未提交   （数据保存，但尚未提交）
    ///  3: 待审核    （已提交，待上一级机构审核）
    ///  6：审核通过 
    ///  9：审核不通过
    /// </summary>
    public enum EnumAppHospStatus
    {
        未提交 = 0,
        待审核 = 3,
        审核通过 = 6,
        审核不通过 = 9
    }

   /// <summary>
   ///  机构类型以及相关描述   描述信息请勿删除： 退出登录
   /// </summary>
    public enum OrgType
    {
        /// <summary>
        /// 系统   暂无登录页
        /// </summary>
        [Description("Systerm")]
        Systerm = 0,
        /// <summary>
        /// 定点机构   
        /// </summary>
        [Description("Nursing")]
        NursingHome = 1,
        /// <summary>
        /// 经办机构
        /// </summary>
        [Description("Agency")]
        Agency = 2,
        /// <summary>
        /// 承办机构
        /// </summary>
        [Description("Ins")]
        InsuranceCompany = 3,
        /// <summary>
        ///  政府机构  暂无登录页
        /// </summary>
        [Description("Gover")]
        Government = 4
    }
    public enum SystemOperator
    {
        Api,
        Job
    }
    public enum NCIPStatusEnum
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Created = 0,
        /// <summary>
        /// 已撤回
        /// </summary>
        Withdrawn = 1,
        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 10,
        /// <summary>
        /// 审核通过
        /// </summary>
        Passed = 20,
        /// <summary>
        /// 已拨款
        /// </summary>
        Appropriated = 30,
        /// <summary>
        /// 审核不通过
        /// </summary>
        NotPassed = 90,
    }
    public enum AccountStatus
    {
        Disable = 0,
        Enable = 1
    }
    public enum ReportFileType
    {
        Doc = 0,
        Xls = 1,
        Pdf = 2
    }

    public enum FeeType
    {
        药品,
        耗材,
        服务
    }
    public enum CareType
    {
        专护 = 1,
        机构护理 = 2
    }

    /// <summary>
    /// 扣款类型
    /// </summary>
    public enum DeductionType
    {
        [DescriptionAttribute("请假")]
        LeaveHosp = 0,
        [DescriptionAttribute("经办机构操作")]
        NCIOpr = 1
    }

    /// <summary>
    /// 扣款状态
    /// </summary>
    public enum DeductionStatus
    {
        [DescriptionAttribute("未扣款")]
        UnCharge = 0,
        [DescriptionAttribute("已扣款")]
        Charged = 1
    }

    public enum OrgValue
    {
        [DescriptionAttribute("县医院")]
        Xyy = 1,
        [DescriptionAttribute("祈康医院")]
        Qkyy = 3,
        [DescriptionAttribute("健民医院")]
        Jmyy = 2
    }
}