using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Внешний ключ
        public string AuthorId { get; set; }
        public int ArticleId { get; set; }

        // Навигационные свойства
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}