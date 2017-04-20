using System.ComponentModel;

namespace KMHC.SLTC.Business.Entity.NCIA
{

    /// <summary>
    /// 申请状态码
    /// </summary>
    public enum ApplicationEnum
    {
        /// <summary>
        /// 未提交
        /// </summary>
        [Description("未开始")]
        Unsubmitted = 0,

        /// <summary>
        /// 未提交
        /// </summary>
        [Description("已撤回")]
        Withdrawn = 1,

        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        Pending = 3,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        ExaminationPassed = 6,

        /// <summary>
        /// 审核不通过
        /// </summary>
        [Description("审核不通过")]
        AuditNotPassed = 9,

        /// <summary>
        /// 申请复审
        /// </summary>
        [Description("申请复审")]
        ApplyForReview = 11,

        /// <summary>
        /// 申请复审
        /// </summary>
        [Description("终止资格")]
        ApplyForEndCert = 10,
    }
}
