using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class MedicineFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public int Medid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// 健保藥碼
        /// </summary>
        public string InsNo { get; set; }

        /// <summary>
        /// 開藥醫院
        /// </summary>
        public string HospNo { get; set; }

        /// <summary>
        /// 關鍵字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
