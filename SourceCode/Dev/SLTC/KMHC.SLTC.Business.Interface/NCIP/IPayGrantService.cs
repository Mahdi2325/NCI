using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IPayGrantService : IBaseService
    {
        long SavePayGrant(MonFeeGrantRequestEntity request);
        BaseResponse<List<NCIPayGrantEntity>> QueryPayGrantList(string year, string nsid);
        BaseResponse<NCIPayGrantEntity> SavePayGrant(NCIPayGrantEntity request);
        BaseResponse<NCIPayGrantEntity> QueryPayGrantinfo(int id);

        BaseResponse<List<NCIPayGrantEntity>> QueryLTCPayGrant(string year, string nsno);
    }
}
