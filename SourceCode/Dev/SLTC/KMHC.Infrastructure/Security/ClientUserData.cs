using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMHC.Infrastructure.Security
{
    public class ClientUserData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// int 型用户Id
        /// </summary>
        public int UserIdInt { get; set; }

        public string LoginName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 组织结构Id
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo { get; set; }
        /// <summary>
        /// 事業類型
        /// </summary>
        public string EmpGroup { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 职别
        /// </summary>
        public string JobType { get; set; }
        public string Email { get; set; }
        //政府编码
        public string GovId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleType { get; set; }
        public int OrgType { get; set; }

        public string[] DCRoleType { get; set; }
        public string[] LTCRoleType { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string[] SysType { get; set; }

        public string CurrentLoginSys { get; set; }

        /*EC社区 */
        /// <summary>
        /// 登录用户姓名
        /// </summary>
        public string UserName { get; set; }

        public bool IsAdmin
        {
            get
            {
                return RoleType == EnumRoleType.SuperAdmin.ToString() || RoleType == EnumRoleType.Admin.ToString();
            }

        }
        public bool IsSuperAdmin
        {
            get
            {
                return RoleType == EnumRoleType.SuperAdmin.ToString();
            }
        }
    }

    /// <summary>
    /// 角色類型 
    /// SuperAdmin 是預製的 
    /// Admin類型的Role在機構維護界面生成，一個機構只有一個Admin類型Role，調用SuperAdmin類型Role作為模板，生成相應的數據，保存時取消的構選的模塊要同步該機構的所有Role
    /// Normal類型的Role在角色維護界面生成，一個機構可以有多個Normal類型Role
    /// </summary>
    public enum EnumRoleType
    {
        SuperAdmin,
        Admin,
        Normal
    }


}
