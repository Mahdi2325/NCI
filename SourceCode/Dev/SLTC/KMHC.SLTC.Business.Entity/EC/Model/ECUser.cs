using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC
{
    public class ECUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string Pwd { get; set; }
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int CommunityType { get; set; } /*0: 社区，1：人社局  */
        public string ImgUrl { get; set; }
        public string RealName { get; set; }
        public string EmpNo { get; set; }
    }
}
