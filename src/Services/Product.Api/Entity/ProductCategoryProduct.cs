using Contract.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.Entity
{
    public class ProductCategoryProduct : EntityAuditBase<long>
    {
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        [ForeignKey("ProductId")]        
        public ProductCatalog ProductCatalog { get; set; }
        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }
    }
}
