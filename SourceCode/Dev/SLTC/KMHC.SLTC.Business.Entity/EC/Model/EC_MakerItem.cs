using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_MakerItem
    {
        #region 基本屬性
        //
        public int MakerId { get; set; }
        //問題名稱
        public string MakeName { get; set; }
        //序列號
        public Nullable<int> ShowNumber { get; set; }
        //是否顯示
        public Nullable<bool> IsShow { get; set; }
        //
        public Nullable<int> QuestionId { get; set; }
        //數據類型
        public string DataType { get; set; }
        //
        public Nullable<int> LimitedId { get; set; }
        //題目分類
        public string Category { get; set; }
        #endregion
        #region 擴展屬性
        //問題答案
        public IList<EC_MakerItemLimitedValue> MakerItemLimitedValue { get; set; }
        public int LimitedValueId { get; set; }
        public decimal LimitedValue { get; set; }
        public long RegQuestionDataId { get; set; }
        public string LimitedValueName { get; set; }
        #endregion
    }
}
