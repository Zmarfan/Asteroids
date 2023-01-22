﻿using Worms.engine.core.gizmos;
using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;

namespace Worms.game; 

public class GizmoScript : Script {
    public GizmoScript() : base(true) {
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("action")) {
            if (SceneManager.ActiveScene == "main") {
                SceneManager.LoadScene("second");
            }
            else {
                SceneManager.LoadScene("main");
            }
        }
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRay(Transform.Position, Transform.Up * 100f, Color.GREEN);
        Gizmos.DrawRay(Transform.Position, Transform.Right * 100f, Color.BLUE);
        Gizmos.DrawRay(Transform.Position, Transform.Left * 100f, Color.BLACK);
        Gizmos.DrawRay(Transform.Position, Transform.Down * 100f, Color.WHITE);
        Gizmos.DrawCircle(Transform.Position, 100f, new Color(1f, 0.5f, 0.75f));
        Gizmos.DrawIcon(Transform.Position, new Color(1f, 1f, 0.75f));
    }
}