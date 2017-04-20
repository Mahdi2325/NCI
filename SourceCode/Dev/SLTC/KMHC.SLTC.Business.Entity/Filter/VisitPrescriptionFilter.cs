using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class VisitPrescriptionFilter
    {
        /// <summary>
        /// 住院序號
        /// </summary>
        public long? FeeNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SeqNo { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
