using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCI
{

    public class NCI_User
    {
        public int UserId { get; set; }
        public int? ParentUserId { get; set; }
        
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string IDNo { get; set; }
        public int Status { get; set; }
        public string OrgId { get; set; }
        public string BelongToGovId { get; set; }
        public string RoleId { get; set; }
        public string RoleType { get; set; }

        public int OrgType { get; set; }
        //系统
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
    }

}
