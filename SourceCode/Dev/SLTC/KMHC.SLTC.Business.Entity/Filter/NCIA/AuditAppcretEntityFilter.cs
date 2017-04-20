namespace KMHC.SLTC.Business.Entity.Filter.NCIA
{
    public class AuditAppcretEntityFilter : AppcertEntityFilter
    {
        public string Nsid { get; set; }

        public int Status { get; set; }
    }

    public class AgencyORGFilter
    {
        public string Govid { get; set; }
    }
}
