using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class FallIncidentEvent
    {
        public long? ID { get; set; }
        //
        public long FeeNo { get; set; }
        //住民NO
        public int RegNo { get; set; }
        //在場人員
        public string RecordBy { get; set; }
        //發生地點
        public string EventAddress { get; set; }
        //發生時間
        public Nullable<System.DateTime> EventDate { get; set; }
        //事件類別
        public string EventType { get; set; }
        //事件傷害分級
        public string EventDegree { get; set; }

        //事件原因
        public string EventCause { get; set; }
        //發生前意識狀態
        public string ConsciousState { get; set; }
        //發生前情緒狀態
        public string EmotionalState { get; set; }
        //出現征兆
        public string Signs { get; set; }
        //事件說明
        public string EventDesc { get; set; }

        //處理人員
        public string SettleBy { get; set; }
        //送醫時間
        public Nullable<System.DateTime> VisitDocDate { get; set; }
        //醫院名稱
        public string HospName { get; set; }
        //通知對象
        public string NotifyPeople { get; set; }
        //需通報主管機構
        public bool NotifyGovFlag { get; set; }
        //通報日期
        public Nullable<System.DateTime> NotifyDate { get; set; }
        //發生后意識狀態
        public string ConsciousState_a { get; set; }
        //發生后情緒狀態
        public string EmotionState_a { get; set; }
        //處理結果
        public string SettleResult { get; set; }
        //後續處理說明
        public string FollowUpInstructions { get; set; }
        //醫療爭議
        public string MedicalDispute { get; set; }
        //導致影響
        public string Affects { get; set; }
        //影響說明
        public string AffectsDesc { get; set; }
        //檢討改善
        public string Improvement { get; set; }
        //備註
        public string Description { get; set; }

        //
        public string OrgId { get; set; }

        public string Pict1 { get; set; }

        public string Pict2 { get; set; }

        //在場人員姓名
        public string RecordNameBy { get; set; }

        //處理人員姓名
        public string SettleNameBy { get; set; }

    }
}
