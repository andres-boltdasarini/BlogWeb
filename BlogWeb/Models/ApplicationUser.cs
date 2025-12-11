using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Навигационные свойства
        [InverseProperty("Author")]
        public virtual ICollection<Article> Articles { get; set; }
        
        [InverseProperty("Author")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
