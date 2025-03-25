using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Payment.Domain;

[Table("payments")]
public class Payment
{
    [Key]
    [Column("order_id")]
    public Guid OrderId { get; set; }
    
    [Column("payment_amount")]
    [Required]
    public decimal PaymentAmount { get; set; }
    
    [Column("payed_at")]
    public DateTimeOffset? PayedAt { get; set; }
}