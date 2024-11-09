using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace QuickEquip.Mods;

public class PlayerHudPatch() : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        MultiTokenWaiter equipItem = new([
            t => t is IdentifierToken { Name: "_equip_item"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken { Name: "item"},
            t => t.Type is TokenType.Comma
        ]);

        TokenConsumer newlineConsumer = new(t => t.Type is TokenType.Newline);

        foreach (var token in tokens) {
            if (newlineConsumer.Check(token)) {
                continue;
            } else if (newlineConsumer.Ready) {
                yield return token;
                newlineConsumer.Reset();
            }

            if (equipItem.Check(token)) {
                yield return token;

                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.ParenthesisClose);

                newlineConsumer.SetReady();
            } else {
                yield return token;
            }
        }
    }
}