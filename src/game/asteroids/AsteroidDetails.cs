using Asteroids.game.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.particle_system.ranges;
using GameEngine.engine.helper;

namespace Asteroids.game.asteroids; 

public class AsteroidDetails {
    private static readonly AsteroidDetails BIG = new(ListUtils.Of(
        new AsteroidTypeDetails(TextureNames.BIG_ASTEROID_1, new Vector2[] {
            new (-9, -42),
            new (-23, -60),
            new (-56, -27),
            new (-40, 0),
            new (-52, 26),
            new (-23, 58),
            new (3, 42),
            new (28, 58),
            new (57, 25),
            new (37, 10),
            new (57, -11),
            new (30, -58)
        }),
        new AsteroidTypeDetails(TextureNames.BIG_ASTEROID_2, new Vector2[] {
            new (-30, -55),
            new (-54, -18),
            new (-54, 26),
            new (-18, 27),
            new (-29, 53),
            new (11, 54),
            new (53, 27),
            new (53, 9),
            new (20, -2),
            new (54, -27),
            new (28, -57),
            new (12, -45)
        }),
        new AsteroidTypeDetails(TextureNames.BIG_ASTEROID_3, new Vector2[] {
            new (-27, -55),
            new (-57, -14),
            new (-35, -2),
            new (-57, 12),
            new (-16, 54),
            new (23, 55),
            new (53, 16),
            new (55, -16),
            new (25, -56),
            new (-3, -55),
            new (-4, -25)
        })
    ), AsteroidType.BIG, 25, 250, new RangeZero(10, 20), SoundNames.BANG_LARGE);

    private static readonly AsteroidDetails MEDIUM = new(ListUtils.Of(
        new AsteroidTypeDetails(TextureNames.MEDIUM_ASTEROID_1, new Vector2[] {
            new (-13, -31.5f),
            new (-29, -15.5f),
            new (-22.5f, -1.5f),
            new (-29, 12),
            new (-12, 28),
            new (0.5f, 22),
            new (14.5f, 29),
            new (28.5f, 11.5f),
            new (20.5f, 3.5f),
            new (29, -8),
            new (14, -30.5f),
            new (-5.5f, -25)
        }),
        new AsteroidTypeDetails(TextureNames.MEDIUM_ASTEROID_2, new Vector2[] {
            new (-17, -29.5f),
            new (-29.5f, -9.5f),
            new (-28.5f, 14.5f),
            new (-12.5f, 15),
            new (-16, 27),
            new (6, 27.5f),
            new (27, 13.5f),
            new (27, 4.5f),
            new (15, -1),
            new (27.5f, -12),
            new (12.5f, -29.5f),
            new (5, -23.5f)
        }),
        new AsteroidTypeDetails(TextureNames.MEDIUM_ASTEROID_3, new Vector2[] {
            new(-16, -29),
            new(-30.5f, -6.5f),
            new(-21.5f, -0.5f),
            new(-29.5f, 7),
            new(-10, 28),
            new(11, 28),
            new(28, 8),
            new(28, -8),
            new(14.5f, -28),
            new(-1.5f, -29.5f),
            new(-4, -16)
        })
    ), AsteroidType.MEDIUM, 50, 350, new RangeZero(7, 15), SoundNames.BANG_MEDIUM);

    private static readonly AsteroidDetails SMALL = new(ListUtils.Of(
        new AsteroidTypeDetails(TextureNames.SMALL_ASTEROID_1, new Vector2[] {
            new(-17, 8.5f),
            new(-6, 19.5f),
            new(2.5f, 15),
            new(8.5f, 20),
            new(20, 9),
            new(14.5f, 4.5f),
            new(21, -4),
            new(10, -18),
            new(-1.5f, -15),
            new(-8.5f, -18),
            new(-18.5f, -8),
            new(-14, 1)
        }),
        new AsteroidTypeDetails(TextureNames.SMALL_ASTEROID_2, new Vector2[] {
            new(-9, -17),
            new(-18.5f, -6.5f),
            new(-18, 10),
            new(-8, 10.5f),
            new(-10, 18),
            new(4.5f, 20),
            new(18.5f, 8),
            new(18.5f, 2),
            new(13, -0.5f),
            new(19.5f, -7.5f),
            new(9.5f, -17.5f),
            new(4, -14.5f)
        }),
        new AsteroidTypeDetails(TextureNames.SMALL_ASTEROID_3, new Vector2[] {
            new(-11, -16.5f),
            new(-21, -4),
            new(-16.5f, 1),
            new(-20.5f, 5),
            new(-9, 18.5f),
            new(7, 19.5f),
            new(17, 5.5f),
            new(16.5f, -5),
            new(6.5f, -17),
            new(-3.5f, -17),
            new(-4, -11.5f)
        })
    ), AsteroidType.SMALL, 100, 450, new RangeZero(5, 12), SoundNames.BANG_SMALL);

    public readonly List<AsteroidTypeDetails> details;
    public readonly AsteroidType type;
    public readonly float minVelocity;
    public readonly float maxVelocity;
    public readonly RangeZero particleCount;
    public readonly string explosionAudioId;

    private AsteroidDetails(
        List<AsteroidTypeDetails> details,
        AsteroidType type,
        float minVelocity,
        float maxVelocity,
        RangeZero particleCount,
        string explosionAudioId
    ) {
        this.details = details;
        this.type = type;
        this.minVelocity = minVelocity;
        this.maxVelocity = maxVelocity;
        this.particleCount = particleCount;
        this.explosionAudioId = explosionAudioId;
    }

    public static AsteroidDetails GetDetails(AsteroidType type) {
        return type switch {
            AsteroidType.BIG => BIG,
            AsteroidType.MEDIUM => MEDIUM,
            AsteroidType.SMALL => SMALL,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No support for this type")
        };
    }
    
    public class AsteroidTypeDetails {
        public readonly string textureId;
        public readonly Vector2[] polygonVertices;

        public AsteroidTypeDetails(string textureId, Vector2[] polygonVertices) {
            this.textureId = textureId;
            this.polygonVertices = polygonVertices;
        }
    }
}