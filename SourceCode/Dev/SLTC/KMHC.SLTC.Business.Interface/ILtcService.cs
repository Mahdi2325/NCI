using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ILtcService : IBaseService 
    {
        void SaveData(MonthFeeModel model);
        object CancelData(string date, string nsno);
        object GetOrgMonthDataList(string beginTime, string endTime, string nsno);
        object GetOrgMonthData(long NSMonFeeID);
        object GetResMonthData(BaseRequest<MonthFeeFilter> request);
        object GetResMonthData(BaseRequest request, string sDate, string eDate, string nsno);
    }
}
