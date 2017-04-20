using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.Business.Interface
{
    public interface IMonFeeService : IBaseService
    {
        BaseResponse<IList<MonFeeModel>> QueryMonFeeModel(BaseRequest<MonFeeFilter> request);
        BaseResponse<IList<MonFeeModel>> QueryMonFeeDetial(string monFeeID);
        BaseResponse<MonFeeModel> SaveMonFee(MonFeeGrantRequestEntity request);
        BaseResponse<MonFeeInfoModel> QueryMonFeeInfo(int monFeeId);
        NursingHome QueryOrgNsHomeByID(string nsId);
        object GetDeductionList(BaseRequest request, long NSMONFEEID);

        object QueryYearMonthMonFeeInfo(BillV2 request);
    }
}
