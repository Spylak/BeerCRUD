using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BeerCRUD.Entities;

public class Beer
{
    public int BeerId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    [Column(TypeName = "jsonb")]
    [JsonIgnore]
    public string Ratings { get; set; } = "[]";
    [NotMapped]
    public int TempRating
    {
        get { return 0;}
        set
        {
            var ratings = JsonConvert.DeserializeObject<List<int>>(Ratings);
            ratings.Add(value);
            Ratings = JsonConvert.SerializeObject(ratings);
            if (ratings.Count > 0)
            {
                AvgRating = Math.Round( (decimal)(ratings.Sum() / ratings.Count),2);
            }
            else
            {
                AvgRating = 0M;
            }
        }
    }

    public decimal AvgRating { get; set; }
}