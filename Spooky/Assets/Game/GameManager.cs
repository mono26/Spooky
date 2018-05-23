using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

public class GameManager : PersistenSingleton<GameManager>
{
    protected int targetframeRate = 60;

    public bool IsPaused { get; protected set; }

    public void PauseLevel()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            //pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            return;
        }
        else if (!IsPaused)
        {
            //pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            return;
        }
    }
}
