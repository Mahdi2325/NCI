using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RESIDENTMONFEEDTL : ISoftDeleteEntity
    {
        public int RSMONFEEDTLID { get; set; }
        public Nullable<long> RSMONFEEID { get; set; }
        public string FEENAME { get; set; }
        public string FEETYPE { get; set; }
        public string MCCODE { get; set; }
        public decimal UNITPRICE { get; set; }
        public decimal QTY { get; set; }
        public decimal AMOUNT { get; set; }
        public System.DateTime TAKETIME { get; set; }
        public string OPERATORNAME { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
        public long FEENO { get; set; }
        public bool ISNCIITEM { get; set; }
    }
}
