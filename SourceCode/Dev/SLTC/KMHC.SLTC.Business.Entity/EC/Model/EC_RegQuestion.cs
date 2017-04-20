using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_RegQuestion
    {
        #region 基本属性
        public long RecordId { get; set; }
        public Nullable<DateTime> RecordDate { get; set; }
        public string IdNo { get; set; }
        public int? QuestionId { get; set; }
        public int? EvalNumber { get; set; }
        public int? Score { get; set; }
        public string EnvResults { get; set; }
        public string Description { get; set; }
        public string EvaluateBy { get; set; }
        public Nullable<DateTime> EvalDate { get; set; }
        public int? CommunityId { get; set; }
        public int? ConsistencyFlag { get; set; }
        public bool? WaitInset { get; set; }
        public IList<EC_RegQuestionData> RegQuestionData { get; set; }
        #endregion

        #region 扩展属性
        public EC_RegQuestion()
        {
            this.EC_REGQUESTIONDATA = new HashSet<EC_RegQuestionData>();
        }

        public virtual ICollection<EC_RegQuestionData> EC_REGQUESTIONDATA { get; set; }
        public int? ProportionScore { get; set; }
        public string Code { get; set; }
        public string ltc_quetionOrgId { get; set; }
        #endregion
    }
    public class EC_RegQuestionFilter
    {

        public string IdNo { get; set; }
        public int? QuestionId { get; set; }
        public int? EvalNumber { get; set; }
    }
    public class EC_RegQuestionList
    {
        public List<EC_RegQuestion> RegQuestionList { get; set; }
        public EC_RegTotalResult RegTotalResult { get; set; }
        
    }
}
