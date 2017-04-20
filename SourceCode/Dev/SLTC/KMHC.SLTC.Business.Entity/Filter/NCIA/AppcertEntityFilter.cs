using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter.NCIA
{
    public class AppcertEntityFilter
    {
        //姓名
        public string Name { get; set; }

        //身份证号
        public string IdNo { get; set; }

        //社会保障号
        public string SsNo { get; set; }

        //审批状态: 
        //0：未提交   （数据保存，但尚未提交）
        //1：已撤回   （提交后，下一级机构尚未审批，撤回）
        //3: 待审核    （已提交，待上一级机构审核）
        //6：审核通过 
        //9：审核不通过
        public int Status { get; set; }
    }
}
