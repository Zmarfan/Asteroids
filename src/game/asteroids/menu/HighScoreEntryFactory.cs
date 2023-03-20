using Asteroids.game.asteroids.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.rendering.text_renderer;

namespace Asteroids.game.asteroids.menu; 

public static class HighScoreEntryFactory {
    public static void Create(Transform parent, List<HighScoreEntry> entries) {
        Vector2 position = new(150, -200);

        for (int i = 0; i < entries.Count; i++) {
            GameObject obj = parent.AddChild("highScoreEntry")
                .SetLocalPosition(position)
                .Build()
                    .Transform.AddChild("rank")
                    .SetComponent(TextRendererBuilder
                        .Builder(FontNames.MAIN)
                        .SetWidth(800)
                        .SetColor(Color.WHITE)
                        .SetSize(25)
                        .SetText($"{i + 1}.")
                        .Build()
                    )
                    .Build()
                    .Transform.AddSibling("score")
                    .SetLocalPosition(new Vector2(150, 0))
                    .SetComponent(TextRendererBuilder
                        .Builder(FontNames.MAIN)
                        .SetWidth(800)
                        .SetColor(Color.WHITE)
                        .SetSize(25)
                        .SetText(entries[i].score.ToString())
                        .Build()
                    )
                    .Build()
                    .Transform.AddSibling("name")
                    .SetLocalPosition(new Vector2(425, 0))
                    .SetComponent(TextRendererBuilder
                        .Builder(FontNames.MAIN)
                        .SetWidth(800)
                        .SetColor(Color.WHITE)
                        .SetSize(25)
                        .SetText(entries[i].name)
                        .Build()
                    )
                    .Build()
                .Transform.Parent!.gameObject;
            Transform.Instantiate(obj);

            position.y -= 50;
        }
    }
}