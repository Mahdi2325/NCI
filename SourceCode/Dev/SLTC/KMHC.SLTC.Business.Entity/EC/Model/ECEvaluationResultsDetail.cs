using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class ECEvaluationResultsDetail
    {
        public int ID { get; set; }
        public string IDNO { get; set; }
        public Nullable<int> CONSISTENCYFLAG { get; set; }
        public Nullable<int> ADLSCORE { get; set; }
        public string ADLRESULT { get; set; }
        public Nullable<int> MMSESCORE { get; set; }
        public string MMSERESULT { get; set; }
        public Nullable<int> IADLSCORE { get; set; }
        public string IADLRESULT { get; set; }
        public Nullable<int> GDSSCORE { get; set; }
        public string GDSRESULT { get; set; }
        public Nullable<decimal> TOTALSCORE { get; set; }
        public string RESULT { get; set; }
        public Nullable<int> CARELEVELID { get; set; }
        public Nullable<System.DateTime> EXPDATE { get; set; }
        public bool ISVALID { get; set; }
        public Nullable<bool> ISAUDIT { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string CARELEVELNAME { get; set; }
        public int? MEALSCOST { get; set; }
        public int? BEDCOST { get; set; }
        public string Sex { get; set; }
        public int? CARECOST { get; set; }
        public int? MANAGECOST { get; set; }
        public int? NORMALROOM { get; set; }
        public int? TRIOROOM { get; set; }
        public int? DOUBLEROOM { get; set; }
        public int? SINGLEROOM { get; set; }
        public string SERVEADVISE { get; set; }
        public string CONCLUSION { get; set; }
        public Nullable<DateTime> BIRTHDATE { get; set; }
        /// <summary>
        /// 护理级别
        /// </summary>
        public string CARELEVEL { get; set; }
    }
}
