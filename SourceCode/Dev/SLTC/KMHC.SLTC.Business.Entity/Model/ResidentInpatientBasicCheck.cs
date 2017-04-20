using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class ResidentInpatientBasicCheck
    {
       public long ID { get; set; }
       public string OID { get; set; }
       public DateTime? CHECK_TIME { get; set; }
       public string S_DESC { get; set; }
       public string CURRENT_DISEASE { get; set; }
       public string PAST_DISEASE { get; set; }
       public string PERSONAL_HISTORY { get; set; }
       public string FAMILY_HISTORY { get; set; }
       public string BODY_CHECK { get; set; }
       public string INITIAL_DIAGNOSIS { get; set; }
       public string DOCTOR { get; set; }


    }
}
