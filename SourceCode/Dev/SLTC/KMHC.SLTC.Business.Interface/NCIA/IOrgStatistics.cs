using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.NCIA
{
    public interface IOrgStatistics : IBaseService
    {
        object GetAppcertRate(string starttime, string endtime);
        object GetApphospRate(string starttime, string endtime);
        object GetcertRate(string NSID );
        object GethospRate(string NSID);
        object PutAppcertRate(string NSID, int? YEAR = null);
        object PutApphospRate(string NSID, int? YEAR = null);
        object GetAppcert(string NSID, int YEAR);
        object GetApphosp(string NSID, int YEAR);
        object GetMonthFeeStatistics(string sDate, string eDate, string nsId);
    }
}
