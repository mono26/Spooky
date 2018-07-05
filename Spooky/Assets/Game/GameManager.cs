﻿using UnityEngine;

public enum GameEventTypes
{
    CropSteal,
    LevelStart,
    LevelEnd,
    LevelCompleted,
    Pause,
    UnPause,
    SpawnStart
}

public class GameEvent : SpookyCrowEvent
{
    public GameEventTypes type;

    public GameEvent(GameEventTypes _type)
    {
        type = _type;
    }
}

public class GameManager : Singleton<GameManager>, EventHandler<GameEvent>
{
    public bool IsPaused { get; protected set; }

    protected void OnEnable()
    {
        EventManager.AddListener<GameEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<GameEvent>(this);
        return;
    }

    protected void TriggerPause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            LevelUIManager.Instance.ActivatePauseUI(true);
            Time.timeScale = 0;
            return;
        }
        else if (!IsPaused)
        {
            LevelUIManager.Instance.ActivatePauseUI(false);
            Time.timeScale = 1;
            return;
        }
    }

    public void OnEvent(GameEvent _gameEvent)
    {
        if (_gameEvent.type == GameEventTypes.Pause)
        {
            TriggerPause();
        }
        if (_gameEvent.type == GameEventTypes.UnPause)
        {
            TriggerPause();
        }

        return;
    }
}
