using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IAuditAppcertService : IBaseService
    {
        /// <summary>
        ///查询审核列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        BaseResponse<IList<AppcertEntity>> QueryAppcertList(BaseRequest<AuditAppcretEntityFilter> request);

        /// <summary>
        /// 根据政府机构码查询定点服务机构数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NursingHome>> QueryOrgNsHome(BaseRequest<AgencyORGFilter> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> QueryAppcert(int id);
        /// <summary>
        /// 保存ADL评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AgencyAsstRecordData> SaveAdlRec(AgencyAsstRecordData request);

        /// <summary>
        /// 获取评估表结果数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AgencyAsstRecordData> GetAdlRec(BaseRequest<AgencyAsstRecordDataFilter> request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppcertEntity> SaveAppcert(AppcertEntity request);
        /// <summary>
        /// 查询评估问题和答案显示数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        BaseResponse<EC_Question> GetQue(string code);

        /// <summary>
        /// 获取定点机构的护理形式
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<CareTypeEntity>> QueryCareTypeList(string nsId);
    }
}
