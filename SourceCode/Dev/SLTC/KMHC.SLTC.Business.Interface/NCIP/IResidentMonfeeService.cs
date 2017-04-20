using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IResidentMonfeeService : IBaseService
    {
        void SaveResidentMonfeeStatus(int monfeeId, int status);
        void SaveDeductionMonfeeStatus(int monfeeId, int status);
    }
}
