using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWeb.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; } // Краткое описание
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Внешний ключ теперь строковый (IdentityUser.Id)
        public string AuthorId { get; set; }
        
        // Навигационные свойства
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
        
    }
}
