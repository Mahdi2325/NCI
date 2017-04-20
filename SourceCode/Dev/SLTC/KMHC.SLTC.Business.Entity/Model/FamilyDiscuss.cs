using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class FamilyDiscuss
    {
        public string RecordByShow { get; set; }
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string RecordBy { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string VisitType { get; set; }
        public string VisitorName { get; set; }
        public string Appellation { get; set; }
        public string BloodRelationship { get; set; }
        public Nullable<decimal> BodyTemp { get; set; }
        public string Description { get; set; }
        public string OrgId { get; set; }
    }
}
