using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.EC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIP
{
    public interface IRegInHosStatusListService : IBaseService
    {
        /// <summary>
        ///查询住民在院状态信息
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        BaseResponse<RegInHosStatusDtlData> QueryRegInHosStatustList(string name, string idno, string nsid, string status, BaseResponse<IList<RegInHosStatusListEntity>> resultContent, int CurrentPage, int PageSize);

        BaseResponse<RegInHosStatusDtlData> QueryRegInHosStatustList(string name, string idno, string nsid, string status, BaseResponse<IList<RegInHosStatusListEntity>> resultContent);

    }
}
