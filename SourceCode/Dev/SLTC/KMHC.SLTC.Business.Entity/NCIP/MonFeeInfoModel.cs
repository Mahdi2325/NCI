using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class MonFeeInfoModel
    {

        public MonFeeInfoModel()
        {
            ResidentMonFeeEntityList = new List<ResidentMonFeeEntity>();
            MonFeeEntity = new MonFeeEntity();
        }

        public List<ResidentMonFeeEntity> ResidentMonFeeEntityList { get; set; }
        public MonFeeEntity MonFeeEntity { get; set; }
        public double? deAmount { get; set; }
    }

    public class ResidentMonFeeEntity : ResidentMonFeeModel
    {
        public string Name { get; set; }
    }
}
