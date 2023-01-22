﻿using System.Diagnostics;
using SDL2;
using Worms.engine.camera;
using Worms.engine.core.input;
using Worms.engine.game_object.scripts;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.update; 

public class UpdateHandler {
    private const float FIXED_UPDATE_CYCLE_TIME = 0.02f;
    
    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;
    private float _fixedUpdateAcc;
    private float _deltaTime;

    public UpdateHandler(SceneData sceneData) {
        _sceneData = sceneData;
        _fixedUpdateAcc = 0;
    }
    
    public void Awake() {
        GameObjectHandler.AllScripts.ForEach(static script => {
            try {
                if (!script.HasRunAwake) {
                    script.Awake();
                }
            }
            catch (Exception e) {
                Logger.Error(e, $"An exception occured in {script} during the Awake callback");
            }
            script.HasRunAwake = true;
        });
        GameObjectHandler.FrameCleanup();
    }
    
    public void Start() {
        GameObjectHandler.AllActiveGameObjectScripts.ForEach(static script => {
            try {
                if (script is { IsActive: true, HasRunStart: false }) {
                    script.Start();
                }
            }
            catch (Exception e) {
                Logger.Error(e, $"An exception occured in {script} during the Start callback");
            }
            script.HasRunStart = true;
        });
        GameObjectHandler.FrameCleanup();
    }
    
    public void UpdateLoops(float deltaTime) {
        UpdateFrameTimeData(deltaTime);
        while (_fixedUpdateAcc > FIXED_UPDATE_CYCLE_TIME) {
            FixedUpdate();
            _fixedUpdateAcc -= FIXED_UPDATE_CYCLE_TIME;
        }
        Update();
        GameObjectHandler.FrameCleanup();
    }

    private void FixedUpdate() {
        foreach (Script script in GameObjectHandler.AllActiveGameObjectScripts) {
            try {
                if (script.IsActive) {
                    script.FixedUpdate(FIXED_UPDATE_CYCLE_TIME);
                }
            }
            catch (Exception e) {
                Logger.Error(e, $"An exception occured in {script} during the Fixed Update callback");
            }
        }
    }
    
    private void Update() {
        Input.Update(_deltaTime);
        
        foreach (Script script in GameObjectHandler.AllActiveGameObjectScripts) {
            try {
                if (script.IsActive) {
                    script.Update(_deltaTime);
                }
            }
            catch (Exception e) {
                Logger.Error(e, $"An exception occured in {script} during the Update callback");
            }
        }
        _sceneData.camera.Update(_deltaTime);
    }
    
    private void UpdateFrameTimeData(float deltaTime) {
        _deltaTime = deltaTime;
        _fixedUpdateAcc += _deltaTime;
    }
}