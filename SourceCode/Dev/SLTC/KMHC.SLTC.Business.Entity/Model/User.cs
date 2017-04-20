/*
 * 描述:User
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:02:29 PM   Admin            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class User
    {
        public string RealName { get; set; }
        public int Age { get; set; }
        public string ImgUrl { get; set; }
        public string Organization { get; set; }
        public int UserId { get; set; }
        public string UserIdStr { get; set; }
        public string LogonName { get; set; }
        public string Pwd { get; set; }
        public Nullable<System.DateTime> PwdExpDate { get; set; }
        public Nullable<int> PwdDuration { get; set; }
        public Nullable<System.DateTime> LastLogonDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? Status { get; set; }
        public string OrgId { get; set; }
    
        public string Email { get; set; }
    }
}