using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class DoctorCheckRec
    {
        public long Id { get; set; }
        //住院序號
        public Nullable<long> FeeNo { get; set; }
        //病例號
        public Nullable<int> RegNo { get; set; }
        //巡診日期
        public Nullable<DateTime> CheckDate { get; set; }
        //科室
        public string DeptNo { get; set; }
        //醫師
        public string DocNo { get; set; }
        //意識
        public string Consciousness { get; set; }
        //生理
        public string Physiology { get; set; }
        //體溫
        public Nullable<decimal> BodyTemp { get; set; }
        //脈搏
        public Nullable<int> Pulse { get; set; }
        //血壓
        public Nullable<int> Bp { get; set; }
        //血氧
        public Nullable<decimal> Oxygen { get; set; }
        //血糖
        public Nullable<decimal> Bs { get; set; }
        //生物異常及處置
        public string DispositionDesc { get; set; }
        //其他
        public string OtherDesc { get; set; }
        //
        public Nullable<DateTime> CreateDate { get; set; }
        //
        public string CreateBy { get; set; }
        //
        public string OrgId { get; set; }

        //醫師
        public string DocName { get; set; }
    }
}
