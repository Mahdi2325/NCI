using KMHC.SLTC.Business.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResidentOutPatientPres
   {
       public long ID { get; set; }
       public DateTime? DATE { get; set; }
       public string HOSPITAL_REG { get; set; }
       public string HOSPITALNAME { get; set; }
       public int HOSPITALSTATUS { get; set; }
        public string DEPARTMENT { get; set; }
       public string DOCTOR { get; set; }
       public string DIAGNOSIS { get; set; }
       public string OID { get; set; }
       public int REGNO { get; set; }
       public BaseResponse<IList<ResidentOutPatientOrder>> ResidentOutPatientOrderList { get; set; }
   }
}
