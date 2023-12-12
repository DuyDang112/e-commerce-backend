using Contract.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Customer.Api.Entities
{
    public class Customer  : EntityBase<int>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string FirrtName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
