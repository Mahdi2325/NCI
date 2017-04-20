using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Medicine
    {
        #region 基本屬性
        //
        public int Medid { get; set; }
        //
        public string MedCode { get; set; }
        //中文名稱
        public string ChnName { get; set; }
        //英文學名
        public string EngName { get; set; }
        //副作用
        public string Sideeffect { get; set; }
        //劑型
        public string MedKind { get; set; }
        //藥品色相
        public string Medicolor { get; set; }
        //外觀
        public string Medifacade { get; set; }
        //規格
        public string SpecDesc { get; set; }
        //用途說明
        public string UseDesc { get; set; }
        //
        public string Commcode { get; set; }
        //藥理分類
        public string MedType { get; set; }
        //藥品圖像
        public string MedPict { get; set; }
        //健保藥碼
        public string InsNo { get; set; }
        //開藥醫院
        public string HospNo { get; set; }
        //
        public string OrgId { get; set; }
        #endregion
    }
}
