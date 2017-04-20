using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter.NCIA
{
    public  class AppHospEntityFilter
    {
        //姓名
        public string Name { get; set; }
        //身份证号
        public string IdNo { get; set; }
        //状态
        public int IcResult { get; set; }
    }
}
