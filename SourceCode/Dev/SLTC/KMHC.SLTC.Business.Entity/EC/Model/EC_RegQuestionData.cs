using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_RegQuestionData
    {
        public long Id { get; set; }
        public long RecordId { get; set; }
        public int QuestionId { get; set; }
        public int MakerId { get; set; }
        public decimal MakerValue { get; set; }
        public int LimitedValueId { get; set; }   
    }
}
