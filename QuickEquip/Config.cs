using System.Text.Json.Serialization;

namespace QuickEquip;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}
