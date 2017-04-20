using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RESIDENTMONFEE : ISoftDeleteEntity
    {
        public int RSMONFEEID { get; set; }
        public Nullable<long> NSMONFEEID { get; set; }
        public string NSID { get; set; }
        public long FEENO { get; set; }
        public string RESIDENTSSID { get; set; }
        public string CERTNO { get; set; }
        public System.DateTime HOSPENTRYDATE { get; set; }
        public System.DateTime HOSPDISCHARGEDATE { get; set; }
        public int HOSPDAY { get; set; }
        public decimal NCIPAYLEVEL { get; set; }
        public decimal NCIPAYSCALE { get; set; }
        public decimal TOTALAMOUNT { get; set; }
        public decimal NCIPAY { get; set; }
        public int STATUS { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
        public List<RESIDENTMONFEEDTL> NCIP_RESIDENTMONFEEDTL { get; set; }
    }
}
