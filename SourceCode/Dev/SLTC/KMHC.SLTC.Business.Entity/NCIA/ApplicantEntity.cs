using System;
namespace KMHC.SLTC.Business.Entity.NCIA
{
    public class ApplicantEntity
    {
        #region 人员基本属性信息
        public string ApplicantId { get; set; }
        public string Idno { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Ssno { get; set; }
        public string Residence { get; set; }
        public int McType { get; set; }
        public string Disease { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string FamilyMemberName { get; set; }
        public string Familymemberphone { get; set; }
        public string Occupation { get; set; }
        public string Nativeplace { get; set; }
        public string Maritalstatus { get; set; }
        public string Economystatus { get; set; }
        public string Livecondition { get; set; }
        public string Housingproperty { get; set; }
        public string Habithos { get; set; }
        public Nullable<int> Zip { get; set; }
        public string Familymemberrelationship { get; set; }
        public Nullable<int> Lastcertresult { get; set; }
        public DateTime? Lastcertdate { get; set; }
        public Nullable<int> Lasthospresult { get; set; }
        public DateTime? Lasthospdate { get; set; }
        public string Createby { get; set; }
        public DateTime? Createtime { get; set; }
        public string Updateby { get; set; }
        public DateTime? Updatetime { get; set; }
        public Nullable<bool> Isdelete { get; set; }
        public string DiseaseDesc { get; set; }
        public string NSID { get; set; }
        #endregion

        #region 扩展属性
        //true:人员资料有变动,在人员记录表里记录一笔数据；
        //false:(默认)
        public bool IsInsertHistory { get; set; }
        //true:编辑人员资料时,身份证号有修改变动；
        //false:(默认)
        public bool IsIdNoChaged { get; set; }
        //是否已申请
        public bool IsApply { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        #endregion
    }

    public class SaveApplicantEntity : ApplicantEntity
    {

    }

    #region 参保人请求信息
    /// <summary>
    /// 参保人请求实体
    /// </summary>
    public class ApplicantFiletr
    {
        #region 构造器
        public ApplicantFiletr()
        {
            Name = string.Empty;
            IdNo = string.Empty;
            IsDelete = 1;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 参保人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// 是否删除  0  是， 1 否 
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 社保卡号
        /// </summary>
        public string SsNo { get; set; }

        /// <summary>
        /// 机构id
        /// </summary>
        public string NsId { get; set; }
        #endregion
    }
    #endregion
}
