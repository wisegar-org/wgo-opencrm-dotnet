namespace OpenCRM.Core.Data
{
    public class UserSessionEntity : BaseEntity
    {
        public DateTime IssuedDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public required string UserName { get; set; }
        public required string CypherToken { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public Guid LanguageId { get; set; }
        public LanguageEntity Language { get; set; } = null!;

    }
}
