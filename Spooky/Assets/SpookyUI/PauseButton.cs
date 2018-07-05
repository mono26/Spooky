using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    /// Puts the game on pause
    public virtual void PauseButtonAction(bool _pause)
    {
        if(_pause == true)
        {
            EventManager.TriggerEvent(new GameEvent(GameEventTypes.Pause));
        }
        else if (_pause == false)
        {
            EventManager.TriggerEvent(new GameEvent(GameEventTypes.UnPause));
        }

            return;
    }
}
