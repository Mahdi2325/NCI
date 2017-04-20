using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class VisitPrescription
    {
        #region 基本屬性
        public int PId { get; set; }
        public int? SeqNo { get; set; }
        public int? MedId { get; set; }
        //劑型
        public string Dosage { get; set; }
        //每次顆數
        public decimal? Qty { get; set; }
        //每劑劑量
        public string TakeQty { get; set; }
        //給藥途徑
        public string TakeWay { get; set; }
        //服藥頻率
        public string Freq { get; set; }
        //給藥時間
        public string Freqtime { get; set; }
        //用法多少天
        public int? Freqday { get; set; }
        //用法多少次
        public int? Freqqty { get; set; }
        //長期藥
        public bool? LongFlag { get; set; }
        //使用中
        public bool? UseFlag { get; set; }
        //處方開始日期
        public Nullable<System.DateTime> StartDate { get; set; }
        //處方結束日期
        public Nullable<System.DateTime> EndDate { get; set; }
        //
        public string Description { get; set; }
        public string OrgId { get; set; }  
        #endregion

        #region 附加屬性
        //藥品名稱
        public string EngName { get; set; }
        public string ChnName { get; set; }
        public string FreqName { get; set; }
        
        //每顆劑量單位
        public string MedKind { get; set; }

        // 住院序號      
        public long? FeeNo { get; set; }
        //醫師姓名
        public string VisitDoctor { get; set; }
        //醫師姓名
        public string VisitDoctorName { get; set; }
        //醫院名稱
        public string VisitHospName { get; set; }
        //科別
        public string VisitDeptName { get; set; }
        //就醫類型
        public string VisitType { get; set; }

        public int? TakeDays { get; set; }
        #endregion

        public virtual VisitDocRecords VisitDocRecords { get; set; }
    }
}
