using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCI
{
    public class InsuranceCompany
    {
        public string ICId { get; set; }
        public string GovId { get; set; }
        public string ICName { get; set; }
        public int? NsType { get; set; }
        public string Address { get; set; }
        public string Contactor { get; set; }
        public string Phone { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? IsDelete { get; set; }
    }
}
