using Microsoft.AspNetCore.Identity;

namespace OpenCRM.Core.Data
{
    public class UserTokenEntity : IdentityUserToken<Guid>, IHasTimestamps
    {
        public DateTime? AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
