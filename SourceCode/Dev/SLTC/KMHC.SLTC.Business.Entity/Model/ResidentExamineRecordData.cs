using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public  class ResidentExamineRecordData
    {
       public string Item_Code1 { get; set; }
       public string Item_Name1 { get; set; }
       public string Item_Type { get; set; }
       public int ShowNumber { get; set; }
       public string Item_Code2 { get; set; }
       public string Item_Name2 { get; set; }
       public int ShowNumber1 { get; set; }
       public string Item_Code { get; set; }
       public string Item_Name { get; set; }
       public int ShowNumber2 { get; set; }
       public string Data_Type { get; set; }
       public string UOM_Name { get; set; }
       public int Item_Value_Symbol { get; set; }
       public string Item_Value { get; set; }
       public string Item_Value_EN { get; set; }
       public string Low_Bound { get; set; }
       public string Up_Bound { get; set; }
       public string Severity_Name { get; set; }
       public int Record_ID { get; set; }

    }
   public class ResidentExamineRecordData1
   {
       public IEnumerable<ResidentExamineRecordData2> ResidentExamineRecordData2List { get; set; }
       public string Item_Name1 { get; set; }
   }
   public class ResidentExamineRecordData2
   {
       public IEnumerable<ResidentExamineRecordData3> ResidentExamineRecordData3List { get; set; }
       public string Item_Name2 { get; set; }
   }
   public class ResidentExamineRecordData3
   {
       public string Item_Value { get; set; }
       public string Item_Name { get; set; }
       public string Low_Bound { get; set; }
       public string Up_Bound { get; set; }
       public string Severity_Name { get; set; }
   }
   public class ResidentExamineRecordData4
   {
       public string Item_Code1 { get; set; }
       public string Item_Name1 { get; set; }
   }
}
