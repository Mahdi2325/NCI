using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IAuditYearCertService : IBaseService
    {
        BaseResponse<IList<AuditYearCertModel>> GetYearCertlist(BaseRequest<AuditYearCertFilter> request);
        BaseResponse UpdateYearCert(AuditYearCertModel request);

        BaseResponse UpdateAppHospCertInfo(RegNCIInfo request);

        BaseResponse<IList<AuditYearCertModel>> GetAduitYearCertList(BaseRequest<AuditYearCertFilter> request);
    }
}
