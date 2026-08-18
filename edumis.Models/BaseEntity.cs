using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models;

public class BaseEntity<T>
{   
    [Column("rowid")]
    public T RowId { get; set; } = default!;

   // [Required]
    [Column(name: "createdby")]
    public string? CreatedBy { get; set; } = default!;

    [Column(name: "createddate")]
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    [Column(name: "modifiedby")]
    public string? ModifiedBy { get; set; } = default!;

    [Column(name: "modifieddate")]
    public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
}
