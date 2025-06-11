using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project14.Models
{
    [Table("table_Favorite")]
    public class table_Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("MemberId")]
        public virtual table_Member Member { get; set; }

        [ForeignKey("ProductId")]
        public virtual table_Product Product { get; set; }
    }
}