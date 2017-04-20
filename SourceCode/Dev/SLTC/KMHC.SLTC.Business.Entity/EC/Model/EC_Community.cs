using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_Community
    {
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityAddress { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int CommunityType { get; set; }
    }
}
