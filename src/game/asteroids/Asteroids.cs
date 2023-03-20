using Asteroids.game.asteroids.names;
using Asteroids.game.asteroids.scenes;
using Worms.engine.core;
using Worms.engine.core.assets;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.gizmos;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.textures;
using Worms.engine.core.update.physics.layers;
using Worms.engine.core.update.physics.settings;
using Worms.engine.helper;

namespace Asteroids.game.asteroids; 

public static class AsteroidGame {
    public static Game CreateGame() {
        return new Game(GameSettingsBuilder
            .Builder()
            .SetBuildMode()
            .SetTitle("Asteroids")
            .SetAssets(DefineAssets())
            .SetAudioSettings(new AudioSettings(Volume.Max(), ListUtils.Of(
                new AudioChannel(ChannelNames.EFFECTS, Volume.Max()),
                new AudioChannel(ChannelNames.MUSIC, Volume.Max())
            )))
            .SetWindowWidth(1280)   
            .SetWindowHeight(720)
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
            .SetCursorSettings(new CursorSettings(false))
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
                new AssetDeclaration(Path("asteroids\\textures\\big_asteroid_1.png"), TextureNames.BIG_ASTEROID_1),
                new AssetDeclaration(Path("asteroids\\textures\\big_asteroid_2.png"), TextureNames.BIG_ASTEROID_2),
                new AssetDeclaration(Path("asteroids\\textures\\big_asteroid_3.png"), TextureNames.BIG_ASTEROID_3),
                new AssetDeclaration(Path("asteroids\\textures\\medium_asteroid_1.png"), TextureNames.MEDIUM_ASTEROID_1),
                new AssetDeclaration(Path("asteroids\\textures\\medium_asteroid_2.png"), TextureNames.MEDIUM_ASTEROID_2),
                new AssetDeclaration(Path("asteroids\\textures\\medium_asteroid_3.png"), TextureNames.MEDIUM_ASTEROID_3),
                new AssetDeclaration(Path("asteroids\\textures\\small_asteroid_1.png"), TextureNames.SMALL_ASTEROID_1),
                new AssetDeclaration(Path("asteroids\\textures\\small_asteroid_2.png"), TextureNames.SMALL_ASTEROID_2),
                new AssetDeclaration(Path("asteroids\\textures\\small_asteroid_3.png"), TextureNames.SMALL_ASTEROID_3),
                new AssetDeclaration(Path("asteroids\\textures\\player.png"), TextureNames.PLAYER),
                new AssetDeclaration(Path("asteroids\\textures\\enemy.png"), TextureNames.ENEMY),
                new AssetDeclaration(Path("asteroids\\textures\\shot.png"), TextureNames.SHOT),
                new AssetDeclaration(Path("asteroids\\textures\\fragment.png"), TextureNames.FRAGMENT),
                new AssetDeclaration(Path("asteroids\\textures\\ship_fragment.png"), TextureNames.SHIP_FRAGMENT)
            ))
            .AddAudios(ListUtils.Of(
                new AssetDeclaration(Path("asteroids\\sounds\\bangLarge.wav"), SoundNames.BANG_LARGE),
                new AssetDeclaration(Path("asteroids\\sounds\\bangMedium.wav"), SoundNames.BANG_MEDIUM),
                new AssetDeclaration(Path("asteroids\\sounds\\bangSmall.wav"), SoundNames.BANG_SMALL),
                new AssetDeclaration(Path("asteroids\\sounds\\beat1.wav"), SoundNames.BEAT_1),
                new AssetDeclaration(Path("asteroids\\sounds\\beat2.wav"), SoundNames.BEAT_2),
                new AssetDeclaration(Path("asteroids\\sounds\\extraLife.wav"), SoundNames.EXTRA_LIFE),
                new AssetDeclaration(Path("asteroids\\sounds\\fire.wav"), SoundNames.FIRE),
                new AssetDeclaration(Path("asteroids\\sounds\\saucerBig.wav"), SoundNames.SAUCER_BIG),
                new AssetDeclaration(Path("asteroids\\sounds\\saucerSmall.wav"), SoundNames.SAUCER_SMALL),
                new AssetDeclaration(Path("asteroids\\sounds\\thrust.wav"), SoundNames.THRUST)
            ))
            .AddFonts(ListUtils.Of(
                new AssetDeclaration(Path("asteroids\\fonts\\Pixeled.ttf"), FontNames.MAIN),
                new AssetDeclaration(Path("asteroids\\fonts\\RoadPixel.ttf"), FontNames.TITLE)
            ))
            .Build();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}