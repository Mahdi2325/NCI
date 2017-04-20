using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class BedBasic
    {
        //床号编号
        public string BedNo { get; set; }
        public string BedDesc { get; set; }
        public string RoomNo { get; set; }
        public string RoomName { get; set; }
        public string BedClass { get; set; }
        public string Floor { get; set; }
        public string FloorName { get; set; }
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string BedType { get; set; }
        public string BedKind { get; set; }
        public string BedStatus { get; set; }
        public string SexType { get; set; }
        public string Prestatus { get; set; }
        public string InsbedFlag { get; set; }
        public bool? Status { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string ResidentName { get; set; }
        public string Habit { get; set; }
        public string Language { get; set; }
        public string Merryflag { get; set; }
        //星座
        public string Constellations { get; set; }
        public string Bloodtype { get; set; }
        public string Sex { get; set; }
        //宗教
        public string Religioncode { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<bool> Ctrflag { get; set; }
        //收案时间
        public Nullable<System.DateTime> InDete { get; set; }

        public Nullable<int> RegNo { get; set; }
      
    }
}





