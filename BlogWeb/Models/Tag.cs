using System.Collections.Generic;

namespace BlogWeb.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }    
    }
}
