using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCI
{
  public class Insurance
    {
        public string ICID { get; set; }
        public string GovID { get; set; }
        public string IcName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Updateby { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDeletE { get; set; }
    }
}
