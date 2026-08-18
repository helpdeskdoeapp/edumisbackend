using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.MISC;

[Table("tbmisc_swachhbharatimages")]
public class SwachhBharatImagesModel : BaseEntity<int>
{
    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column(name: "fordate", TypeName = "date")]
    public DateOnly ForDate { get; set; }

    [Column(name: "imageurl", TypeName = "varchar(500)")]
    public string ImageUrl { get; set; } = default!;

    [Column(name: "imagename", TypeName = "varchar(500)")]
    public string ImageName { get; set; } = default!;

    [Column(name: "image_contenttype", TypeName = "varchar(50)")]
    public string? ImageContentType { get; set; }

    [Column(name: "image_extn", TypeName = "varchar(20)")]
    public string? ImageFileExtn { get; set; }

    [Column(name: "iscurrent", TypeName = "boolean")]
    public bool IsCurrent { get; set; }
}
