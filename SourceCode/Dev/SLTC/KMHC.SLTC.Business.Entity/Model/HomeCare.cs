using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{

    public class WebBaseResponse
    {
        public string errorCode { get; set; }
        public string msg { get; set; }
    }

    public class WebResponse<T> : WebBaseResponse
    { 
        public T content { get; set; }

        public WebResponse()
        {
        }
        public WebResponse(T content)
        {
            this.content = content;
        }
    }

    public class PressureContent
    {
        public List<Pressure> list { get; set; }
    }
    public class BloodSugarContent
    {
        public List<BloodSugar> list { get; set; }
    }
    public class BloodOxygenContent
    {
        public List<BloodOxygen> list { get; set; }
    }

    /// <summary>
    /// 血压
    /// </summary>
    public class Pressure
    {
     public long? Sno {get;set;}
     public string IMEI{get;set;}
     public int? TypeId{get;set;}
    /// <summary>
    /// 测量时间 timestamp
    /// </summary>
     public string BPTime{get;set;}
    /// <summary>
    /// 量测时间 字符串
    /// </summary>
     public string MeasureTime{get;set;}
     public int? HPressure{get;set;}
     public int? LPressure{get;set;}
    /// <summary>
    /// 脉搏
    /// </summary>
     public int? Puls { get; set; }
     public string CreateDate { get; set; }

    }
    /// <summary>
    /// 血糖
    /// </summary>
    public class BloodSugar
    {
        public long? Sno { get; set; }
        public string IMEI { get; set; }
        public int? TypeId { get; set; }
        /// <summary>
        /// 测量时间
        /// </summary>
        public string BSTime { get; set; }
        /// <summary>
        /// 量测时间 字符串
        /// </summary>
        public string MeasureTime { get; set; }
        /// <summary>
        /// 血糖值
        /// </summary>
        public int? Glu { get; set; }
        public string CreateDate { get; set; }
    }
    /// <summary>
    /// 血氧
    /// </summary>
    public class BloodOxygen
    {
        public long? Sno { get; set; }
        public string IMEI { get; set; }
        public int? TypeId { get; set; }
        /// <summary>
        /// 测量时间 timestamp
        /// </summary>
        public string OXYTime { get; set; }
        /// <summary>
        /// 量测时间 字符串
        /// </summary>
        public string MeasureTime { get; set; }
        /// <summary>
        /// 血氧
        /// </summary>
        public int? Spo2 { get; set; }
        /// <summary>
        /// 脉搏
        /// </summary>
        public int? Puls { get; set; }
        public string CreateDate { get; set; }
    }
}
