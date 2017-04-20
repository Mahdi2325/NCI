using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter.NCI
{
   
    public class NCI_UserFilter
    {
        public string Account { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string OrgId { get; set; }
        public int? OrgType { get; set; }

        public int? ParentUserId { get; set; }

    }
    
}
