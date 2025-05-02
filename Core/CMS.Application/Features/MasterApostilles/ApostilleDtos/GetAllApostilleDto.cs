namespace CMS.Application.Features.MasterApostilles.ApostilleDtos
{
    public class GetAllApostilleDto
    {
        public int ValueId { get; set; }
        public string ApostilleName { get; set; }
        public bool Status { get; set; }
        public int TotalRecords { get; set; }
    }
}
