using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWeb.Models
{
    public class ArticleTag
    {
        public int ArticleId { get; set; }
        public int TagId { get; set; }

        // Навигационные свойства
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}