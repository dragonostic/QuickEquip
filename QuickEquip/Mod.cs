using GDWeave;
using QuickEquip.Mods;

namespace QuickEquip;

public class Mod : IMod {
    public Config Config;

    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new PlayerPatch());
        modInterface.RegisterScriptMod(new PlayerHudPatch());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
