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
    public class NursingHome
    {
        public string NsId { get; set; }
        public string NSNo { get; set; }
        public string GovId { get; set; }
        public string NsName { get; set; }
        public string NSType { get; set; }
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
