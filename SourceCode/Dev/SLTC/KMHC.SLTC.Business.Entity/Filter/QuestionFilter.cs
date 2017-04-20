using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class QuestionFilter
    {
        //評估量表名稱
        public string Questionname { get; set; }
        //評估量表描述
        public string QuestionDesc { get; set; }
        //機構ID
        public string OrgId { get; set; }

        //问题Code
        public string Code { get; set; }

        //是否顯示
        public Nullable<bool> IsShow { get; set; }
    }
    public class MakerItemFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
    public class QuestionResultsFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
    public class MakerItemLimitedValueFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
}
