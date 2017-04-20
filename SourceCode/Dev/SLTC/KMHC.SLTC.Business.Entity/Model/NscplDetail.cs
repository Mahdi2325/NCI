using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NscplDetail
    {
        public NscplDetail()
        {
            this.AssessValue = new HashSet<ASSESSVALUE>();
            this.NscplGoal = new HashSet<NSCPLGOAL>();
            this.NscplActivity = new HashSet<LTC_NSCPLActivity>();
        }
        public long SeqNo { get; set; }
        //专业类别
        public string CpType { get; set; }
        //问题层面
        public string CpLevel { get; set; }
        //護理問題診斷
        public string Cpdiag { get; set; }
        //问题描述
        public string Nsdesc { get; set; }
        //问题导因
        public string CpReason { get; set; }
        //目标
        public ICollection<NSCPLGOAL> NscplGoal { get; set; }
        //措施
        public ICollection<LTC_NSCPLActivity> NscplActivity { get; set; }
        //评值
        public ICollection<ASSESSVALUE> AssessValue { get; set; }
        //
        public Nullable<long> FeeNo { get; set; }
        //
        public Nullable<int> RegNo { get; set; }

        public string CpdiagName { get; set; }
    }
}
