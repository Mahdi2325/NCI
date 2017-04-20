using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC
{
    /// <summary>
    /// 申请人资料
    /// </summary>
    public class Person
    {

        #region 构造函数
        public Person()
        {
            IdNo = string.Empty;
            Name = string.Empty;
            Sex = string.Empty;
            SeCurityId = string.Empty;
            NaTion = string.Empty;
            EduCation = string.Empty;
            OccuPation = string.Empty;
            NaTivePlace = string.Empty;
            MariTalStatus = string.Empty;
            DomiCilePlace = string.Empty;
            ResiDencePlace = string.Empty;
            Zip = string.Empty;
            Phone = string.Empty;
            MobilePhone = string.Empty;
            AgentName = string.Empty;
            AgentreLation = string.Empty;
            AgentAddress = string.Empty;
            AgentZip = string.Empty;
            AgentPhone = string.Empty;
            AgentMobilePhone = string.Empty;
            EconomyStatus = string.Empty;
            LiveCondition = string.Empty;
            HousingProperty = string.Empty;
            HelpCare = false;
            WhoCare = string.Empty;
            MedicalWay = string.Empty;
            HaBithos = string.Empty;
        }
        #endregion

        #region 构造器

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别 F:女 M:男
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 社保卡号
        /// </summary>
        public string SeCurityId { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string NaTion { get; set; }
        /// <summary>
        /// 文化程度
        /// </summary>
        public string EduCation { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public Nullable<System.DateTime> BirThDate { get; set; }
        /// <summary>
        /// 曾从事过职业
        /// </summary>
        public string OccuPation { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NaTivePlace { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MariTalStatus { get; set; }
        /// <summary>
        /// 户籍所在地
        /// </summary>
        public string DomiCilePlace { get; set; }
        /// <summary>
        /// 现居住地址
        /// </summary>
        public string ResiDencePlace { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 住宅电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 代理人姓名
        /// </summary>
        public string AgentName { get; set; }
        /// <summary>
        /// 与申请人关系
        /// </summary>
        public string AgentreLation { get; set; }
        /// <summary>
        /// 代理人地址
        /// </summary>
        public string AgentAddress { get; set; }
        /// <summary>
        /// 代理人邮编
        /// </summary>
        public string AgentZip { get; set; }
        /// <summary>
        /// 代理人住宅电话
        /// </summary>
        public string AgentPhone { get; set; }
        /// <summary>
        /// 代理人手机号码
        /// </summary>
        public string AgentMobilePhone { get; set; }
        /// <summary>
        /// 目前经济状况
        /// </summary>
        public string EconomyStatus { get; set; }
        /// <summary>
        /// 居住情况
        /// </summary>
        public string LiveCondition { get; set; }
        /// <summary>
        /// 住房性质
        /// </summary>
        public string HousingProperty { get; set; }
        /// <summary>
        /// 需要帮助时，是否得到照料0：未得到 1：得到
        /// </summary>
        public Nullable<bool> HelpCare { get; set; }
        /// <summary>
        /// 有人照料时填写，照料人
        /// </summary>
        public string WhoCare { get; set; }
        /// <summary>
        /// 就诊方式
        /// </summary>
        public string MedicalWay { get; set; }
        /// <summary>
        /// 习惯就诊医院
        /// </summary>
        public string HaBithos { get; set; }
        /// <summary>
        /// 社区ID
        /// </summary>
        public int CommunityId { get; set; }

        /// <summary>
        /// true  修改     false  新增
        /// </summary>
        public bool LockCommunity{ get; set; }
        #endregion
    }

}
