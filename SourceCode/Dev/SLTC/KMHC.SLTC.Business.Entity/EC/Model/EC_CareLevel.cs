using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_CareLevel
    {
        public int CarelevelId { get; set; }
        public int CostlevelId { get; set; }
        public int? Low { get; set; }
        public int? Up { get; set; }
        public string CareDescribtion { get; set; }
        public string CareLevelName { get; set; }
        public string CareLevel { get; set; }
    }
}
