using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IRSMonFeeDtlService : IBaseService
    {
        BaseResponse<IList<ResidentMonFeeModel>> QueryRSMonFee(BaseRequest<MonFeeFilter> request);
        BaseResponse<IList<ResidentMonFeeDtlModel>> QueryRSMonFeeDtl(BaseRequest<MonFeeFilter> request);
    }
}
