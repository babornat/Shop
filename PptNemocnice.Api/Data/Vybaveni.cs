using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Data;

public class Vybaveni
{
    [Key]// Volitelné
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int PriceCzk { get; set; }
    public DateTime BoughtDateTime { get; set; }

    //public DateTime LastRevision { get; set; } //není potřeba, už je to ve Revizes
    public List<Revize> Revizes { get; set; } = new();
}
