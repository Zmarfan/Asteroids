using Asteroids.game.names;
using Asteroids.game.scenes;
using GameEngine.engine.core;
using GameEngine.engine.core.assets;
using GameEngine.engine.core.audio;
using GameEngine.engine.core.cursor;
using GameEngine.engine.core.gizmos;
using GameEngine.engine.core.input.listener;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.core.update.physics.layers;
using GameEngine.engine.core.update.physics.settings;
using GameEngine.engine.data;
using GameEngine.engine.helper;
using GameEngine.engine.window;

namespace Asteroids.game; 

public static class AsteroidGame {
    public static Game CreateGame() {
        return new Game(GameSettingsBuilder
            .Builder()
            .SetBuildMode()
            .SetTitle("Asteroids")
            .SetAssets(DefineAssets())
            .SetWindowSettings(WindowSettingsBuilder
                .Builder()
                .SetResolution(new Vector2Int(1280, 720))
                .SetAllowFullscreen(false)
                .Build()
            )
            .SetAudioSettings(new AudioSettings(Volume.Max(), ListUtils.Of(
                new AudioChannel(ChannelNames.EFFECTS, Volume.Max()),
                new AudioChannel(ChannelNames.MUSIC, Volume.Max())
            )))
            .AddScenes(ListUtils.Of(MainMenuScene.GetScene(), AddHighScoreScene.GetScene(), HighScoreScene.GetScene(), GameScene.GetScene()))
            .AddInputListeners(ListUtils.Of(
                InputListenerBuilder
                    .Builder(InputNames.ROTATE, Button.D)
                    .SetNegativeButton(Button.A)
                    .SetAltPositiveButton(Button.RIGHT)
                    .SetAltNegativeButton(Button.LEFT)
                    .SetGravity(6)
                    .SetSensitivity(6)
                    .Build(),
                InputListenerBuilder
                    .Builder(InputNames.THRUST, Button.W)
                    .SetAltPositiveButton(Button.UP)
                    .SetSensitivity(2)
                    .Build(),
                InputListenerBuilder
                    .Builder(InputNames.MENU_UP, Button.W)
                    .SetAltPositiveButton(Button.UP)
                    .Build(),
                InputListenerBuilder
                    .Builder(InputNames.MENU_DOWN, Button.S)
                    .SetAltPositiveButton(Button.DOWN)
                    .Build(),
                InputListenerBuilder.Builder(InputNames.MENU_SELECT, Button.SPACE).Build(),
                InputListenerBuilder.Builder(InputNames.FIRE, Button.SPACE).Build()
            ))
            .SetPhysics(PhysicsSettingsBuilder
                .Builder(ListUtils.Of(LayerMask.DEFAULT), ListUtils.Of(LayerMask.IGNORE_RAYCAST))
                .AddLayer(LayerNames.ASTEROID, ListUtils.Of(LayerNames.PLAYER_SHOT, LayerNames.ENEMY_SHOT, LayerNames.ENEMY, LayerNames.PLAYER))
                .AddLayer(LayerNames.ENEMY, ListUtils.Of(LayerNames.ASTEROID, LayerNames.PLAYER_SHOT, LayerNames.PLAYER))
                .AddLayer(LayerNames.PLAYER, ListUtils.Of(LayerNames.ASTEROID, LayerNames.ENEMY_SHOT, LayerNames.ENEMY))
                .AddLayer(LayerNames.PLAYER_SHOT, ListUtils.Of(LayerNames.ENEMY, LayerNames.ASTEROID))
                .AddLayer(LayerNames.ENEMY_SHOT, ListUtils.Of(LayerNames.ASTEROID, LayerNames.PLAYER))
                .AddLayer(LayerNames.PLAY_AREA_OBJECT, ListUtils.Of(LayerNames.PLAY_AREA_OBJECT))
                .Build()
            )
            .SetCursorSettings(new CursorSettings(false, false))
            .SetGizmoSettings(GizmoSettingsBuilder
                .Builder()
                .ShowBoundingBoxes(false)
                .ShowPolygonTriangles(false)
                .Build()
            )
            .Build()
        );
    }
    
    private static Assets DefineAssets() {
        return AssetsBuilder
            .Builder()
            .AddTextures(ListUtils.Of(
                new AssetDeclaration(Path("textures\\big_asteroid_1.png"), TextureNames.BIG_ASTEROID_1),
                new AssetDeclaration(Path("textures\\big_asteroid_2.png"), TextureNames.BIG_ASTEROID_2),
                new AssetDeclaration(Path("textures\\big_asteroid_3.png"), TextureNames.BIG_ASTEROID_3),
                new AssetDeclaration(Path("textures\\medium_asteroid_1.png"), TextureNames.MEDIUM_ASTEROID_1),
                new AssetDeclaration(Path("textures\\medium_asteroid_2.png"), TextureNames.MEDIUM_ASTEROID_2),
                new AssetDeclaration(Path("textures\\medium_asteroid_3.png"), TextureNames.MEDIUM_ASTEROID_3),
                new AssetDeclaration(Path("textures\\small_asteroid_1.png"), TextureNames.SMALL_ASTEROID_1),
                new AssetDeclaration(Path("textures\\small_asteroid_2.png"), TextureNames.SMALL_ASTEROID_2),
                new AssetDeclaration(Path("textures\\small_asteroid_3.png"), TextureNames.SMALL_ASTEROID_3),
                new AssetDeclaration(Path("textures\\player.png"), TextureNames.PLAYER),
                new AssetDeclaration(Path("textures\\enemy.png"), TextureNames.ENEMY),
                new AssetDeclaration(Path("textures\\shot.png"), TextureNames.SHOT),
                new AssetDeclaration(Path("textures\\fragment.png"), TextureNames.FRAGMENT),
                new AssetDeclaration(Path("textures\\ship_fragment.png"), TextureNames.SHIP_FRAGMENT)
            ))
            .AddAudios(ListUtils.Of(
                new AssetDeclaration(Path("sounds\\bangLarge.wav"), SoundNames.BANG_LARGE),
                new AssetDeclaration(Path("sounds\\bangMedium.wav"), SoundNames.BANG_MEDIUM),
                new AssetDeclaration(Path("sounds\\bangSmall.wav"), SoundNames.BANG_SMALL),
                new AssetDeclaration(Path("sounds\\beat1.wav"), SoundNames.BEAT_1),
                new AssetDeclaration(Path("sounds\\beat2.wav"), SoundNames.BEAT_2),
                new AssetDeclaration(Path("sounds\\extraLife.wav"), SoundNames.EXTRA_LIFE),
                new AssetDeclaration(Path("sounds\\fire.wav"), SoundNames.FIRE),
                new AssetDeclaration(Path("sounds\\saucerBig.wav"), SoundNames.SAUCER_BIG),
                new AssetDeclaration(Path("sounds\\saucerSmall.wav"), SoundNames.SAUCER_SMALL),
                new AssetDeclaration(Path("sounds\\thrust.wav"), SoundNames.THRUST)
            ))
            .AddFonts(ListUtils.Of(
                new AssetDeclaration(Path("fonts\\Pixeled.ttf"), FontNames.MAIN),
                new AssetDeclaration(Path("fonts\\RoadPixel.ttf"), FontNames.TITLE)
            ))
            .Build();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}