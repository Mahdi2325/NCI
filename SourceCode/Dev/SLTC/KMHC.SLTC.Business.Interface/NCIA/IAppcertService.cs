using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IAppcertService : IBaseService
    {
        /// <summary>
        /// 获取护理保险待遇申请信息列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        BaseResponse<IList<AppcertEntity>> QueryAppcertList(BaseRequest<AppcertEntityFilter> request);
        /// <summary>
        /// 获取单个护理保险待遇申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> QueryAppcert(int id);
        /// <summary>
        /// 根据社会保障号或身份证号带出基本信息
        /// </summary>
        /// <param name="keyNo">社会保障号或身份证号</param>
        /// <returns></returns>
        BaseResponse<ApplicantEntity> QueryApplicant(string keyNo);

        BaseResponse<AppcertLTCEntity> QueryLTCAppcertInfo(string idno, string type);

        /// <summary>
        /// 保存护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> SaveAppcert(AppcertEntity request);

        /// <summary>
        /// 护理保险待遇重新申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> SaveReAppcert(AppcertEntity request);
        /// <summary>
        /// 提交护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> SubmitAppcert(AppcertEntity request);

        /// <summary>
        /// 撤销护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> CancelSubmitAppcert(AppcertEntity request);

        /// <summary>
        /// 删除护理保险待遇申请信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
            BaseResponse<AppcertEntity> DeleteAppcert(AppcertEntity request);
        /// <summary>
        /// 查询评估问题和答案显示数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        BaseResponse<EC_Question> GetQue(string code);

        /// <summary>
        /// 保存ADL评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<NursingHomeAsstRecordData> SaveAdlRec(NursingHomeAsstRecordData request);

        /// <summary>
        /// 获取ADL评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<NursingHomeAsstRecordData> GetAdlRec(BaseRequest<NursingHomeAsstRecordDataFilter> request);

        /// <summary>
        ///  获取当前机构的护理形式
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<CareTypeEntity>> QueryCareTypeList();
    }
}
