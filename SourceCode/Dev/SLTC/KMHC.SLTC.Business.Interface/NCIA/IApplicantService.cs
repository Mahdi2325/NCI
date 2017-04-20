using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NCIA;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IApplicantService : IBaseService
    {
        /// <summary>
        /// 获取参保人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ApplicantEntity>> QueryApplicantList(BaseRequest<ApplicantFiletr> request);

        /// <summary>
        /// 修改参保人删除状态
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回实体</returns>
        BaseResponse<ApplicantEntity> SaveApplicant(ApplicantEntity request);

        /// <summary>
        /// 获取参保人详情信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        BaseResponse<ApplicantEntity> QueryApplicantInfo(string id);

        /// <summary>
        /// 保存参保人数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>保存信息</returns>
        BaseResponse<ApplicantEntity> SaveApplicantInfo(ApplicantEntity request);

        BaseResponse<IList<ApplicantEntity>> GetHadCertApplicantsByNsId(string nsId);
    }
}
