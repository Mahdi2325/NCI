using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Filter
{
    public class ECUserFilter
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public int CommunityId { get; set; }
        public int CommunityType { get; set; } /*0: 社区，1：人社局  */
    }
}
