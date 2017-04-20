using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RegEvaluateModel
    {
        public long? FeeNo { get; set; }
        public int? RegNo { get; set; }
        public DateTime? EvalDate { get; set; }
        public DateTime? NextEvalDate { get; set; }
        public int? Count { get; set; }
        public long Id { get; set; }
        public string EvaluateBy { get; set; }
        public string NextEvaluateBy { get; set; }
        /// <summary>
        /// 使用辅具
        /// </summary>
        public string AidTools { get; set; }
        /// <summary>
        /// 手册障礙類別
        /// </summary>
        public string BookType { get; set; }
        /// <summary>
        /// 手冊障礙等級
        /// </summary>
        public string BookDegree { get; set; }
        /// <summary>
        /// 重大傷病卡名稱
        /// </summary>
        public string IllCardName { get; set; }
        /// <summary>
        /// 曾接受相關服務
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 意識狀態
        /// </summary>
        public string MindState { get; set; }
        /// <summary>
        /// 認知狀況
        /// </summary>
        public string CognitiveState { get; set; }
        /// <summary>
        /// 口語表達能力
        /// </summary>
        public string ExpressionState { get; set; }
        /// <summary>
        /// 非口語表達能力
        /// </summary>
        public string NonexpressionState { get; set; }
        /// <summary>
        /// 語言理解能力
        /// </summary>
        public string LanguageState { get; set; }
        /// <summary>
        /// 情緒狀況
        /// </summary>
        public string EmotionState { get; set; }
        /// <summary>
        /// 人格
        /// </summary>
        public string Personality { get; set; }
        /// <summary>
        /// 注意力
        /// </summary>
        public string Attention { get; set; }
        /// <summary>
        /// 現實感
        /// </summary>
        public string Realisticsense { get; set; }
        /// <summary>
        /// 社交參與度
        /// </summary>
        public string SocialParticipation { get; set; }
        /// <summary>
        /// 社交態度
        /// </summary>
        public string SocialAttitude { get; set; }
        /// <summary>
        /// 社交能力
        /// </summary>
        public string SocialSkills { get; set; }
        /// <summary>
        /// 溝通技巧
        /// </summary>
        public string CommSkills { get; set; }
        /// <summary>
        /// 應變能力
        /// </summary>
        public string ResponseSkills { get; set; }
        /// <summary>
        /// 解決問題能力
        /// </summary>
        public string FixissueSkills { get; set; }
        /// <summary>
        /// 個人興趣嗜好
        /// </summary>
        public string Hobby { get; set; }
        /// <summary>
        /// 個人專長
        /// </summary>
        public string Expertise { get; set; }
        /// <summary>
        /// 特殊行為
        /// </summary>
        public string SpecialBehavior { get; set; }
        /// <summary>
        /// 入住原因摘要
        /// </summary>
        public string AdmissionReason { get; set; }
        /// <summary>
        /// 過去病史
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 心理層面概述
        /// </summary>
        public string PsychologyDesc { get; set; }
        /// <summary>
        /// 家庭評估-家庭次系統評估
        /// </summary>
        public string FamilySubSystem { get; set; }
        /// <summary>
        /// 家庭評估-個案對家庭的功能或角色
        /// </summary>
        public string FamilyFunRole { get; set; }
        /// <summary>
        /// 家庭評估-家屬對個案之期待及支持度評估
        /// </summary>
        public string FamilySupport { get; set; }
        /// <summary>
        /// 家庭評估－家庭經濟能力評估
        /// </summary>
        public string FamilyFinancial { get; set; }
        /// <summary>
        /// 社會資源評估-人際關係
        /// </summary>
        public string SocialRelationShip { get; set; }
        /// <summary>
        /// 社會資源評估－社區支持系統
        /// </summary>
        public string SocialSupport { get; set; }
        /// <summary>
        /// 社會資源評估－正式資源
        /// </summary>
        public string SocialFormalResource { get; set; }
        /// <summary>
        /// 主要問題
        /// </summary>
        public string KeyQuestion { get; set; }
        /// <summary>
        /// 社工處遇計劃－短期目標　
        /// </summary>
        public string ProcessPlan_S { get; set; }
        /// <summary>
        /// 社工處遇計劃－中期目標　
        /// </summary>
        public string ProcessPlan_M { get; set; }
        /// <summary>
        /// 社工處遇計劃－長期目標　
        /// </summary>
        public string ProcessPlan_L { get; set; }
        /// <summary>
        /// 與家庭討論服務計劃
        /// </summary>
        public string RelativesDiscuss { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 記錄創建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 記錄創建人
        /// </summary>
        public string CreateBy { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string VerifyBy { get; set; }
        /// <summary>
        /// 機構ID
        /// </summary>
        public string OrgId { get; set; }
    }
}
