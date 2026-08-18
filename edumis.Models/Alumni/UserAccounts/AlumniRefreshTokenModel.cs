using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Alumni.UserAccounts;

[Table("tbalm_refeshtoken")]
public class AlumniRefreshTokenModel : BaseEntity<Guid>
{
    [Column(name: "token", TypeName = "varchar(250)")]
    public string Token { get; set; } = default!;

    [Column(name: "userid")]
    public Guid UserId { get; set; }

    [Column(name: "expireson_utc")]
    public DateTime ExpiresOnUTC { get; set; }

    public AlumniLoginModel User { get; set; } = default!;
}
