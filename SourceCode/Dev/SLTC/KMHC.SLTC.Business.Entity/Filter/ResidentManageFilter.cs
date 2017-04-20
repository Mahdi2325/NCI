namespace KMHC.SLTC.Business.Entity.Filter
{
    public class ResidentOutPatientPresFilter
    {
       public int REGNO { get; set; }
       public string HOSPITALNAME { get; set; }
       public int flag { get; set; }

    }
    public class ResidentOutPatientOrderFilter
    {
        public string OID { get; set; }
        

    }
    public class PersonFilter
    {
        public string Name { get; set; }
        public string IdNo { get; set; }
        public string OrgId { get; set; }
        public string IpdFlag { get; set; }
        public string ResidengNo { get; set; }
        
    }

    public class ResidentFilter
    {
        public int? RegNo { get; set; }
        public long? FeeNo { get; set; }
        public string ResidengNo { get; set; }
        public string FeeNoString { get; set; }
        public string BedNo { get; set; }
        public string Name { get; set; }
        public string IpdFlag { get; set; }
        public string IdNo { get; set; }
        public string FloorName { get; set; }
        public string FloorId { get; set; }
        public string RoomName { get; set; }
        public string OrgId { get; set; }
    }

    public class VisitFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class LeaveHospFilter
    {
        public long? FeeNo { get; set; }
        public int? ShowNumber { get; set; }
        public string OrgId { get; set; }
    }

    public class DepositFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class VerifyFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class CloseCaseFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class FamilyDiscussFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class NutrtionEvalFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class ResidentDtlFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class DemandFilter
    {
        public long? Id { get; set; }
    }

    public class HealthFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class AttachFileFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class RelationFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class RelationDtlFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
}





