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
    public interface IAuditAppHospService : IBaseService
    {
        /// <summary>
        ///查询审核列表数据
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        BaseResponse<IList<AppHospEntity>> QueryAppHospList(BaseRequest<AuditAppHospEntityFilter> request);

        /// <summary>
        /// 入院审核
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>返回信息</returns>
        BaseResponse<AppHospEntity> AuditAppHosp(AppHospEntity request);
    }
}
