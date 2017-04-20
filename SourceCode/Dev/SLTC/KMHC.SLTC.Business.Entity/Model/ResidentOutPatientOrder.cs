using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class ResidentOutPatientOrder
    {
        public long ID { get; set; }
        public string ORDER_TYPE { get; set; }
        public DateTime? PERFORMER_TIME { get; set; }
        public DateTime? START_TIME { get; set; }
        public DateTime? STOP_TIME { get; set; }
        public string DRUG_NAME { get; set; }
        public string TOTAL_DOSE { get; set; }
        public string DAYS { get; set; }
    }
}
