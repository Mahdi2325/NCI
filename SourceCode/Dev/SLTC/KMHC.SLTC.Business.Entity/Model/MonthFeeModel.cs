using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class MonthFeeModel
    {
        public string nsNo { get; set; }
        public NSMONFEE nsMonthFee { get; set; }
        public List<RESIDENTMONFEE> rsMonthFee { get; set; }
        public List<RESIDENTMONFEEDTL> rsMonthFeeDtl { get; set; }
        public List<Deduction> nciDeduction { get; set; }
    }
}
