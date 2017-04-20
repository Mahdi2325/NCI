using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class MonthFeeStatistics
    {
        public string yearMonth { get; set; }
        public decimal? tAmount { get; set; }
        public decimal? tNciPay { get; set; }
        public int? tRes { get; set; }
    }
}
