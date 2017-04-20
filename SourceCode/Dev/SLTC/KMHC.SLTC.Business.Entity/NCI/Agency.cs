using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCI
{
     public  class Agency
    {
         public string AgencyId { get; set; }
         public string GovId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyNo { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CreatBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? IsDelete { get; set; }
    }
}
