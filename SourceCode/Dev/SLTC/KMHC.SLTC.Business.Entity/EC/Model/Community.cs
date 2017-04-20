using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class Community
    {
        public int COMMUNITYID { get; set; }
        public string COMMUNITYNAME { get; set; }
        public string COMMUNITYADDRESS { get; set; }
        public string AREA { get; set; }
        public string CITY { get; set; }
        public string PROVINCE { get; set; }
        public int COMMUNITYTYPE { get; set; }
    }
}
