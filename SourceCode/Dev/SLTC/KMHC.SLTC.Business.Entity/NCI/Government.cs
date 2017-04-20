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
    public class Government
    {
        public int GovId { get; set; }
        public int? GovermentLevelId { get; set; }
        public int? LocationId { get; set; }
        public string GovName { get; set; }
        public string ParentGovId { get; set; }
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
