using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IAppHospService : IBaseService
    {
        /// <summary>
        /// 查询住院申请列表数据
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回信息</returns>
        BaseResponse<IList<AppHospEntity>> QueryAppHospList(BaseRequest<AppHospEntityFilter> request);

        /// <summary>
        /// 查询参保人详情信息
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>入院申请记录信息</returns>
        BaseResponse<AppHospEntity> QueryAppShopInfo(int appHospid);
        /// <summary>
        /// 根据身份证号码或医保卡号查询资格信息
        /// </summary>
        /// <param name="keyNo">社保卡号或者身份证号码</param>
        /// <returns>资格证书信息</returns>
        BaseResponse<AppCertBaseInfo> QueryAppcertInfo(string keyNo);
        /// <summary>
        /// 保存入院申请记录信息
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>保存数据</returns>
        BaseResponse<AppHospEntity> SaveAppHosp(AppHospEntity request);

        /// <summary>
        /// 逻辑删除入院申请数据 
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>删除数据</returns>
        BaseResponse<AppHospEntity> ChangeAppHosp(AppHospEntity request);
        /// <summary>
        /// 撤回操作
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns>
        BaseResponse<AppHospEntity> ChangeAgencyResult(AppHospEntity request);
        
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<AppHospEntity> submitAppHosp(AppHospEntity request);
    }
}
