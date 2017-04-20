using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class ResidentExamineRecord
    {
        public long ID { get; set; }
        public DateTime? EXAMINE_DATE { get; set; }
        public string YEAR_NUMBER { get; set; }
        public int CHECK_NUMBER { get; set; }
      
        public int REGNO { get; set; }
       
    }
}
