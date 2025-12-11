using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }    
    }
}
