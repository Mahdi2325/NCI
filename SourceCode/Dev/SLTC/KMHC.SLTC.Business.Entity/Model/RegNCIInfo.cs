using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class RegNCIInfo
    {
        public int ID { get; set; }
        public long Feeno { get; set; }
        public string Certno { get; set; }
        public System.DateTime CertStartTime { get; set; }
        public System.DateTime CertexpiredTime { get; set; }
        public string Caretypeid { get; set; }
        public string CaretypeName { get; set; }
        public decimal NCIPaylevel { get; set; }
        public decimal NCIPayscale { get; set; }
        public int Status { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> Createtime { get; set; }

        public Nullable<System.DateTime> ApplyHosTime { get; set; }
        public string Updateby { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }

        public string RegName { get; set; }
        public string IdNo { get; set; }
        public string SsNo { get; set; }
        public bool IsHaveNCI { get; set; }

        public DateTime? ipdoutTime { get; set; }
    }
}
