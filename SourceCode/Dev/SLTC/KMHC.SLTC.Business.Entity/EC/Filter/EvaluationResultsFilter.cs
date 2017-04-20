using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Filter
{
   public class EvaluationResultsFilter
    {
        public string keyWord { get; set; }
        public string IDNO { get; set; }
        public Nullable<bool> ISAUDIT { get; set; }
        public bool ISVALID { get; set; }
        public int? COMMUNITYID { get; set; }
    }
}
