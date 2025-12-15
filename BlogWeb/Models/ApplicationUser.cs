using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; } // ← Добавьте ?
        public string? Bio { get; set; } // ← Добавьте ?
        public string? AvatarUrl { get; set; } // ← Добавьте ?
        public bool IsActive { get; set; } = true;
        
        // Навигационные свойства
        [InverseProperty("Author")]
        public virtual ICollection<Article> Articles { get; set; }
        
        [InverseProperty("Author")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
