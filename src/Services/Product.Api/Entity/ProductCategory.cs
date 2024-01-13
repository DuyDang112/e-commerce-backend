using Contract.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product.Api.Entity
{
    public class ProductCategory : EntityAuditBase<long>
    {
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string No { get; set; } = Guid.NewGuid().ToString(); 
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }

        // FKey Category Parent
        public long? ParentCategoryId { get; set; }
        public ICollection<ProductCategory> CategoryChildren { get; set; }
        [ForeignKey("ParentCategoryId")]
        public ProductCategory ParentCategory { get; set; }
    }
}
