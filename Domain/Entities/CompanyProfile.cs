namespace Domain.Entities
{
    public class CompanyProfile : BaseEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string TaxId { get; set; }
        public string? CompanyLogo { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid SalesCategoryId { get; set; }
        public virtual SalesCategory SalesCategory { get; set; }
        public string CompanyName { get; set; }
    }
}