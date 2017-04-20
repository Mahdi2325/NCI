using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RegActivityRequEval
    {
        public long Id { get; set; }
        public DateTime InDate { get; set; }
        public DateTime EvalDate { get; set; }
        /// <summary>
        /// 社工编号
        /// </summary>
        public string Carer { get; set; }
        /// <summary>
        /// 社工姓名
        /// </summary>
        public string CarerName { get; set; }
        //視覺
        public string Vision { get; set; }
        //嗅覺
        public string Smell { get; set; }
        //觸覺
        public string Sensation { get; set; }
        //味覺
        public string Taste { get; set; }
        //聽覺
        public string Hearing { get; set; }
        //上肢
        public string Upperlimb { get; set; }
        //下肢
        public string Lowerlimb { get; set; }
        //幻覺
        public string Hallucination { get; set; }
        //妄想
        public string Delusion { get; set; }
        //注意力
        public string Attention { get; set; }
        //定向感
        public string Directionsense { get; set; }
        //理解力
        public string Comprehension { get; set; }
        //記憶力
        public string Memory { get; set; }
        //表達
        public string Expression { get; set; }
        //其他敘述
        public string Othernarrative { get; set; }
        //情緒
        public string Emotion { get; set; }
        //自我概念
        public string Self { get; set; }
        //行為內容
        public string Behaviorcontent { get; set; }
        //行為頻率
        public string Behaviorfreq { get; set; }
        //活動參與意見
        public string Activity { get; set; }
        //已會談個案
        public string Talkedwilling { get; set; }
        //無法會談個案
        public string Nottalked { get; set; }
        //文康休閒活動
        public string Artactivity { get; set; }
        //輔料性活動
        public string Aidsactivity { get; set; }
        //重症區活動
        public string Severeactivity { get; set; }
        //已会谈个案-无意愿
        public string Talkednowilling { get; set; }
        //
        public long FeeNo { get; set; }
        //
        public int RegNo { get; set; }
        //
        public string OrgId { get; set; }

        public string FeeName { get; set; }
        public int Directman { get; set; }
        public int Directtime { get; set; }
        public int Directaddress { get; set; }
        public int Memoryflag { get; set; }  
    }
}
