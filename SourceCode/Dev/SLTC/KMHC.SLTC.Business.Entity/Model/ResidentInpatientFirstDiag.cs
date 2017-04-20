using KMHC.SLTC.Business.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
  public  class ResidentInpatientFirstDiag
    {
        public long ID { get; set; }
        public string OID { get; set; }
        public DateTime? CHECK_TIME { get; set; }
        public string CHECK_REMARK { get; set; }
        public string INITIAL_DIAGNOSIS { get; set; }
        public string DIAGNOSIS_BASIS { get; set; }
        public string IDENTIFY_DIAGNOSIS { get; set; }
        public string TREATMENT_PLAN { get; set; }
        public string RECORD_DOCTOR { get; set; }
        public BaseResponse<IList<ResidentInpatientRoutineCheck>> ResidentInpatientRoutineCheckList { get; set; }
       
    }
}
