using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class ResidentInpatientRoutineCheck
    {
        public long ID { get; set; }
        public string OID { get; set; }
        public DateTime? CHECK_TIME { get; set; }
        public string CHECK_REMARK { get; set; }
        public string DOCTOR { get; set; }
        public string RECORD_DOCTOR { get; set; }
     
    }
}
