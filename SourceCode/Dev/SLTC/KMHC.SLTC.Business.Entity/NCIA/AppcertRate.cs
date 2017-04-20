using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCIA
{
   public class AppcertRate
    {
        public int ID { get; set; }
        public string NSID { get; set; }
        public decimal PASSRATE { get; set; }
        public string YEAR { get; set; }
        public long TOTALPEOPLE { get; set; }
        public long PASSPEOPLE { get; set; }
        public System.DateTime UPDATETIME { get; set; }
    }
}
