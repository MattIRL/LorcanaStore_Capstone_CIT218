using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace LorcanaCardCollector.Models
{

   
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        // FK from Card Table
        [Required]
        [ForeignKey(nameof(Card))]
        public string CardId { get; set; }
        public Cards Card { get; set; }

        [Display(Name = "No. in Stock")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = 0;
    }
}
