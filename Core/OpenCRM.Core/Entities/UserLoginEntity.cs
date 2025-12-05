using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OpenCRM.Core.Data
{
    public class UserLoginEntity : IdentityUserLogin<Guid>, IHasTimestamps
    {
        public DateTime? AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime? DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
