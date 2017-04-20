using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class MonFeeGrantRequestEntity
    {

        public MonFeeGrantRequestEntity()
        {
            monfeeList = new List<MonFeeModel>();
            payGrantModel = new NCIPayGrantModel();
        }

        public List<MonFeeModel> monfeeList { get; set; }

        public NCIPayGrantModel payGrantModel { get; set; }
    }
}
