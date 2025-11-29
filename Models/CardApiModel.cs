using LorcanaCardCollector.Models;

public class CardApiModel
{
    public string Unique_ID { get; set; }
    public string Name { get; set; }
    public string Franchise { get; set; }
    public string Image { get; set; }
    public string Set_Name { get; set; }
    public int? Cost { get; set; }
    public int? Willpower { get; set; }
    public int? Strength { get; set; }
    public string Color { get; set; }

    // Map API Color string to GemColor enum
    public GemColor? GemColorValue
    {
        get
        {
            return Color switch
            {
                "Amber" => GemColor.Amber,
                "Amethyst" => GemColor.Amethyst,
                "Emerald" => GemColor.Emerald,
                "Ruby" => GemColor.Ruby,
                "Sapphire" => GemColor.Sapphire,
                "Steel" => GemColor.Steel,

                "Amber, Amethyst" => GemColor.AmberAmethyst,
                "Amber, Emerald" => GemColor.AmberEmerald,
                "Amber, Ruby" => GemColor.AmberRuby,
                "Amber, Sapphire" => GemColor.AmberSapphire,
                "Amber, Steel" => GemColor.AmberSteel,

                "Amethyst, Emerald" => GemColor.AmethystEmerald,
                "Amethyst, Ruby" => GemColor.AmethystRuby,
                "Amethyst, Sapphire" => GemColor.AmethystSapphire,
                "Amethyst, Steel" => GemColor.AmethystSteel,

                "Emerald, Ruby" => GemColor.EmeraldRuby,
                "Emerald, Sapphire" => GemColor.EmeraldSapphire,
                "Emerald, Steel" => GemColor.EmeraldSteel,

                "Ruby, Sapphire" => GemColor.RubySapphire,
                "Ruby, Steel" => GemColor.RubySteel,
                "Sapphire, Steel" => GemColor.SapphireSteel,

                _ => null
            };
        }
    }
}
