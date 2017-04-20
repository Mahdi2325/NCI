using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NCIA
{
    public class AppHospEntity
    {
        public int AppHospid { get; set; }
        public Nullable<int> Appcertid { get; set; }
        public string AppHospsn { get; set; }
        public string NsID { get; set; }
        public int CareType { get; set; }
        public string DoctorName { get; set; }
        public System.DateTime EntryTime { get; set; }
        public string FamilyMemberName { get; set; }
        public string FamilyMemberphone { get; set; }
        public string Phone { get; set; }
        public int Mctype { get; set; }
        public string Disease { get; set; }
        public string Address { get; set; }
        public string AppReason { get; set; }
        public string UploadFiles { get; set; }
        public string NsComment { get; set; }
        public Nullable<System.DateTime> NsoperateTime { get; set; }
        public string NsstaffName { get; set; }
        public Nullable<int> IcResult { get; set; }
        public string Iccomment { get; set; }
        public Nullable<System.DateTime> IcoperateTime { get; set; }
        public string IcstaffName { get; set; }
        public string IcID { get; set; }
        public Nullable<int> AgencyResult { get; set; }
        public string AgencyComment { get; set; }
        public Nullable<System.DateTime> AgencyoperateTime { get; set; }
        public string AgencystafFName { get; set; }
        public string AgencyId { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Updateby { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public int ActionStatus { get; set; }

        #region 扩展属性
        //人员信息ID
        public string ApplicantId { get; set; }
        //姓名
        public string Name { get; set; }
        //性别
        public string Gender { get; set; }
        //年龄
        public int Age { get; set; }
        //身份证号
        public string IdNO { get; set; }
        //社会保障号
        public string SsNO { get; set; }
        #endregion
    }


    public class AppCertBaseInfo
    {
        //人员信息ID
        public string ApplicantId { get; set; }

        public int? Appcertid { get; set; }
        public string NsID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string IdNO { get; set; }
        public string SsNO { get; set; }
        public int Mctype { get; set; }
        public int? CareType { get; set; }
        public string Disease { get; set; }
        public string FamilyMemberName { get; set; }

        public string FamilyMemberphone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public string CertNo { get; set; }
        public DateTime? CertStartTime { get; set; }
        public DateTime? CertExpiredTime { get; set; }
        public bool? Isdelete { get; set; }
    }
}
