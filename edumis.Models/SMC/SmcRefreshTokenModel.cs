using System.ComponentModel.DataAnnotations.Schema;
using edumis.Models.Users;

namespace edumis.Models.SMC;

[Table("tbsmc_refeshtoken")]
public class SmcRefreshTokenModel : BaseEntity<Guid>
{
    [Column(name: "token", TypeName = "varchar(250)")]
    public string Token { get; set; } = default!;

    [Column(name: "userid")]
    public Guid UserId { get; set; }

    [Column(name: "expireson_utc")]
    public DateTime ExpiresOnUTC { get; set; }
}
