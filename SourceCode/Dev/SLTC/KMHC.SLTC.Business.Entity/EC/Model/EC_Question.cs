using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.EC.Model
{
    public class EC_Question
    {
        #region 基本屬性
        //自增長ID
        public int Id { get; set; }
        //評估量表標示ID
        public Nullable<int> QuestionId { get; set; }
        //評估量表名稱
        public string QuestionName { get; set; }
        //序列號
        public Nullable<int> ShowNumber { get; set; }
        //是否顯示
        public Nullable<bool> IsShow { get; set; }
        //評估量表描述
        public string QuestionDesc { get; set; }
        //量表類別
        public Nullable<int> CategoryId { get; set; }
        //量表Code
        public string Code { get; set; }
        //是否計算總分
        public Nullable<bool> ScoreFlag { get; set; }
        //機構ID
        public string OrgId { get; set; }
        #endregion

        #region 擴展屬性
        //记录保存后的RecordId
        public long RecordId { get; set; }
        //評估問題
        public IList<EC_MakerItem> MakerItem { get; set; }
        //評估結果
        public IList<EC_QuestionResults> QuestionResults { get; set; }
        #endregion
    }
}
