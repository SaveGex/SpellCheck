using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainData.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid JwtId { get; set; }
        [Required]
        public string Token { get; set; } = null!;
        public string? CreatedByIp { get; set; } = null!;
        public int AssociatedUserId { get; set; }
        public User AssociatedUser { get; set; } = null!;
        [Required]
        public int AssociatedClientId { get; set; }
        public Client AssociatedClient { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
    }
}
