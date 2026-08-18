using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbms_zones")]
public class ZonesModel : BaseEntity<int>
{
    [Column(name: "districtid")]
    public int DistrictId { get; set; }

    [Column(name: "title", TypeName ="varchar(100)")]
    public string Title { get; set; } = default!;

    [Column(name: "isactive")]
    public bool IsActive { get; set; }

    public DistrictsModel DistrictNavigation { get; set; } = default!;
 }
