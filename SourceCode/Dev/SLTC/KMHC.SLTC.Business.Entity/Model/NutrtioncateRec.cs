using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NutrtioncateRec
    {
        public long Id { get; set; }
        public long Feeno { get; set; }
        public int Regno { get; set; }
        public DateTime? Recorddate { get; set; }
        //飲食形態
        public string DietPattern { get; set; }
        //營養途徑
        public string NutrtionPathway { get; set; }
        //進食方式
        public string DietWay { get; set; }
        //進食頻率
        public string DinnerFreq { get; set; }
        //活動能力
        public string ActivityAbility { get; set; }
        //體重
        public decimal Weight { get; set; }
        //其他（疾病）
        public string OtherDisease { get; set; }
        //
        public decimal Bmi { get; set; }
        //體重評估
        public string WeightEval { get; set; }
        //目前飲食狀況
        public string DietState { get; set; }
        //水分攝取
        public decimal WaterUptake { get; set; }
        //熱量需求 kcal/天
        public int Kcal { get; set; }
        //主食
        public int KcalFood { get; set; }
        //肉魚豆蛋
        public int KcalFish { get; set; }
        //蔬菜
        public int KcalVegetables { get; set; }
        //水果
        public int KcalFruit { get; set; }
        //油脂
        public int KcalGrease { get; set; }
        //蛋白質需求
        public int Protein { get; set; }
        //額外鹽分攝取
        public string Salinity { get; set; }
        //管灌需求 kcal/天
        public int PipeKcal { get; set; }
        //蛋白質
        public int PipeProtein { get; set; }
        //沖管水量
        public string PipleWater { get; set; }
        //Vit補充
        public string PipleVit { get; set; }
        //其他水量
        public string PipleOtherWater { get; set; }
        //營養診斷
        public string NutrtionDiag { get; set; }
        //特殊疾病飲食
        public string SpecialDiet { get; set; }
        //其他計劃與建議
        public string Suggestion { get; set; }
        //營養師
        public string Dietitian { get; set; }       
    }
}
