using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NSMONFEE : ISoftDeleteEntity
    {
        public int NSMONFEEID { get; set; }
        public Nullable<int> NCIPAYGRANTID { get; set; }
        public string NSID { get; set; }
        public string NSNO { get; set; }
        public string YEARMONTH { get; set; }
        public int TOTALRESIDENT { get; set; }
        public int TOTALHOSPDAY { get; set; }
        public decimal TOTALAMOUNT { get; set; }
        public decimal TOTALNCIPAY { get; set; }
        public int STATUS { get; set; }
        public string CREATORNAME { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
        public List<RESIDENTMONFEE> NCIP_RESIDENTMONFEE { get; set; }
        public List<Deduction> NCIA_DEDUCTION { get; set; }
    }
}
