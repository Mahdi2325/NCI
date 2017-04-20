using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_CostLevel
    {
        public int? CostLevelId { get; set; }
        public int? MealsCost { get; set; }
        public int? BedCost { get; set; }
        public int? CareCost { get; set; }
        public int? ManageCost { get; set; }
        public int? NormalRoom { get; set; }
        public int? TrioRoom { get; set; }
        public int? DoubleRoom { get; set; }
        public int? SingleRoom { get; set; }
        public string CareLevel { get; set; }
        #region 扩展属性
        public string IdNo { get; set; }
        public string CostLevelName{ get; set; }
        #endregion
    }
}
