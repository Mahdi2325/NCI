using KMHC.SLTC.Business.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.Business.Interface
{
    public interface IPersonStatusReportService : IBaseService
    {
        BaseResponse<PersonStatusReportModel> QueryPersonStatusInfo(DateTime? startDate, DateTime? endDate);
    }
}
