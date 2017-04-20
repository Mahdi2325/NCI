using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report.Excel
{
    public class FeeDetailData
    {

        public List<FeeDetail> DataDetail { get; set; }

    }

    public class FeeDetail
    {
    
        public int Index { get; set; }

        public Nullable<long> RsmonfeeId { get; set; }

        //姓名
        public string Name { get; set; }

        //性别
        public string Sex { get; set; }

        //年龄
        public int Age { get; set; }

        //个人编号
        public string NCINo { get; set; }

        //住院号
        public long HospNo { get; set; }

        //身份证号
        public string IDNo { get; set; }

        //人员属性 

        //联系电话
        public string Phone { get; set; }

        //家庭住址
        public string Address { get; set; }

        //就诊医疗机构
        public string NSName { get; set; }

        //人员类别
        public string McType { get; set; }
        public int McTypeCode { get; set; }

        //护理形式
        public string CareType { get; set; }
        public int CareTypeCode { get; set; }

        //入院日期
        public Nullable<DateTime> dHospInDate { get; set; }
        public string HospInDate { get; set; }
        //出院日期
        public Nullable<DateTime> dHospOutDate { get; set; }
        public string HospOutDate { get; set; }

        //住院天数
        public Nullable<int> HospDay { get; set; }

        //疾病名称
        public string Disease { get; set; }
        public string DiseaseCode { get; set; }
        //药品费用
        public decimal DrugFee { get; set; }

        //可报药品费用
        public decimal NCIDrugFee { get; set; }

        //总发生费用
        public decimal TotalAmount { get; set; }

        //定额标准
        public decimal NCILevel { get; set; }

        //补偿比例
        public decimal NCIScale { get; set; }


        public string YearMonth { get; set; }

        public Nullable<bool> IsDelete { get; set; }

    }
}
