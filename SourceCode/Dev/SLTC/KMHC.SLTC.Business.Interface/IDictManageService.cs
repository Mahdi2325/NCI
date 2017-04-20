/*
创建人: 肖国栋
创建日期:2016-03-09
说明:字典管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interceptor;
using System;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface
{
    public interface IDictManageService
    {
        [Caching(CachingMethod.Get)]
        BaseResponse<IList<CodeValue>> QueryCode(CodeFilter request);
        [Caching(CachingMethod.Get)]
        BaseResponse<IList<CommonUseWord>> QueryCommonUseWord(CommonUseWordFilter request);
        BaseResponse<IList<CodeValue>> QueryCode(BaseRequest<CodeFilter> request);
        [Caching(CachingMethod.Get)]
        LTC_NCIFinancialMonth GetFeeIntervalByMonth(string month);
        [Caching(CachingMethod.Get)]
        LTC_NCIFinancialMonth GetFeeIntervalByDate(string date);
        [Caching(CachingMethod.Get)]
        LTC_NCIFinancialMonth GetFeeIntervalByDate(DateTime d);
        [Caching(CachingMethod.Get)]
        LTC_NCIFinancialMonth GetFeeIntervalByYearMonth(string yearMonth);
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CodeValue> GetCode(string id, string pId);

        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CodeValue> SaveCode(CodeValue request);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        int DeleteCode(string id, string pId);
    }
}