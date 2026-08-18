using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Users;

[Table("tbms_refeshtoken")]
public class RefreshTokenModel : BaseEntity<Guid>
{
    [Column(name: "token", TypeName = "varchar(250)")]
    public string Token { get; set; } = default!;

    [Column(name: "userid")]
    public Guid UserId { get; set; }

    [Column(name: "expireson_utc")]
    public DateTime ExpiresOnUTC { get; set; }

    public UserModel User { get; set; } = default!;   
}
