using System;
using KMHC.SLTC.Business.Entity.Report.Excel;
using System.Collections.Generic;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;

namespace KMHC.SLTC.Business.Interface
{
    public interface IReportService : IBaseService
    {
        FeeReportComData GetFeeByCareTypeReport(DateTime? starTime, DateTime? endTime);

        FeeReportComData GetFeeByOrgReport(DateTime? StartTime, DateTime? EndTime);

        FeeReportComData GetFeeByLevelReport(DateTime? StartTime, DateTime? EndTime);
        FeeByDiseaseReportData GetFeeByDiseaseReport(DateTime? startTime, DateTime? endTime);

        FeeDetailData GetFeeDetailReport(DateTime? starTime, DateTime? endTime);

        FeeReportComData GetFeeByRegionReport(DateTime? starTime, DateTime? endTime);

        FeeByCareTypeMain GetFeeByCareTypeMainReport(DateTime? starTime, DateTime? endTime);
        List<PrintMonthFee> GetPrintData(BaseResponse<IList<TreatmentAccount>> res, List<Ipdregout> ipdList);
    }
}
