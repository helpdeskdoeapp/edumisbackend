using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbms_districts")]
public class DistrictsModel : BaseEntity<int>
{   
    [Column(name: "title", TypeName = "varchar(100)")]
    public string Title { get; set; } = default!;

    [Column(name: "isactive")]
    public bool IsActive { get; set; }

    public ICollection<ZonesModel> ZoneList { get; set; } = default!;
}
