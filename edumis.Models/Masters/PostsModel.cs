using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbmsposts")]
public class PostsModel : BaseEntity<int>
{   
    [Column(name: "postcode", TypeName = "varchar(50)")]
    public string PostCode { get; set; } = default!;
       
    [Column(name: "posttitle", TypeName = "varchar(500)")]
    public string PostTitle { get; set; } = default!;

    [Column(name: "isgazetted")]   
    public bool IsGazetted { get; set; }
        
    [Column(name: "orderno", TypeName = "varchar(500)")]
    public string? OrderNo { get; set; }

    [Column(name: "orderdate")]
    public DateTime? OrderDate { get; set; } = Convert.ToDateTime("01/01/1900");

    [Column(name: "isvalid")]  
    public bool IsValid { get; set; }    
}
