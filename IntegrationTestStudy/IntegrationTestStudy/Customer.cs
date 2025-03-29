using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegrationTestStudy;

[Table("Customer", Schema = "dbo")]
public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Column(TypeName = "date")]
    public DateTime? BirthDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}