using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace QuickEquip.Mods;

public class PlayerPatch() : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        MultiTokenWaiter equipHotbarIf = new([
            t => t is IdentifierToken { Name: "has"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken { Name: "slot"},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon
        ]);

        MultiTokenWaiter equipItem = new([
            t => t is IdentifierToken { Name: "_equip_item"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken { Name: "item_data"},
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

            if (equipHotbarIf.Check(token)) {
                yield return token;

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfMatch);
                yield return new IdentifierToken("state");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                yield return new IdentifierToken("STATES");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("FISHING");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("STATES");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("FISHING_CAST");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("STATES");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("FISHING_CHARGE");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("cam_push");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new RealVariant(-0.3));
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("locked");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("interact_prevention");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("rod_cast_dist");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new RealVariant(0.0));
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("animation_data");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("bobber_visible"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 4);

                yield return new IdentifierToken("_enter_animation");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("equip"));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new RealVariant(1.5));
                yield return new Token(TokenType.ParenthesisClose);

                newlineConsumer.SetReady();
            } else if (equipItem.Check(token)) {
                yield return token;

                yield return new IdentifierToken("skip_anim");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("forced");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("set_prev");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                newlineConsumer.SetReady();
            }  else {
                yield return token;
            }
        }
    }
}