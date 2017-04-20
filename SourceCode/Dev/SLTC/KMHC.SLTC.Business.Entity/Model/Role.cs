using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Role
    {
        public string RoleId { get; set; }
        public string ParentRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string Desc { get; set; }
        public int Status { get; set; }
        public List<TreeNode> CheckModuleList { get; set; }
        public string OrgId { get; set; }
        public int OrgType { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
    }
}





