using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglmapdesigwithusertype")]
public class DesignationUserTypeMapping : BaseEntity<int>
{   
    [Column(name: "designationid")]
    public int DesignationId { get; set; }

    [Column(name: "usertype")]
    public int UserType { get; set; }
}
