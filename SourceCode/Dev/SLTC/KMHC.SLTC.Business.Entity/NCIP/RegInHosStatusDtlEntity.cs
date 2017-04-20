using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class RegInHosStatusDtlEntity
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Age { get; set; }
        public string IdNo { get; set; }
        public string SsNo { get; set; }
        public string Phone { get; set; }
        public int McType { get; set; }
        public string Disease { get; set; }
        public string MaritalStatus { get; set; }
        public string FamilyMemberName { get; set; } 
        public string FamilyMemberRelationship { get; set; }
        public string FamilyMemberPhone { get; set; }
        public int InHosStatus { get; set; }
        public string NsId { get; set; }
        public int NsAppCareType { get; set; } 
        public string IpdFlag { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? LeHour { get; set; }               
        
    }

    public class RegInHosStatusDtlData
    {
        public int InHosCount { get; set; }
        public int OutHosCount { get; set; }
        public List<RegInHosStatusDtlEntity> RegInHosStatusDtl { get; set; }
    }
}
