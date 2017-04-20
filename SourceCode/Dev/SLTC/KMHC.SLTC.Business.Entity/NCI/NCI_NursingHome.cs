using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCI
{
    /// <summary>
    /// 定点服务机构
    /// </summary>
    public class NCI_NursingHome
    {
        public int NsId { get; set; }
        public Nullable<int> GovId { get; set; }
        public string NsName { get; set; }
        public Nullable<int> NsType { get; set; }
        public string Address { get; set; }
        public string Contactor { get; set; }
        public string Phone { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<int> IsDelete { get; set; }
    }
}
