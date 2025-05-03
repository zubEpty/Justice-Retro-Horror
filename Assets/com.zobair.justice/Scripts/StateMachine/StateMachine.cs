using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<T> : Subject<StateBase<T>>
{

    public StateBase<T> CurrentState { get; set; }
    public StateBase<T> SubState { get; set; }
    public StateBase<T> SuperState { get; set; }

    public void Initialize(StateBase<T> startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(StateBase<T> newState, bool forceChange = false)
    {
        /*if (!newState.isRoot || (PlayerManager.Instance.sm.CurrentState == PlayerManager.Instance.playerStateHandler.DisabledState && !forceChange))
            return;*/

        /*if(typeof(T) == typeof(PlayerBase))
            PlayerManager.Instance.ClearStateDictionary();
*/
        //Debug.LogError(CurrentState);



        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();

        //Notifier Test
        /*if (typeof(T) == typeof(PlayerBase))
        {
            NotifyObservers(newState);
        }*/

    }

    public void SetSubState(StateBase<T> newSubState)
    {
        /* if(newSubState == null || newSubState == SubState)
         {
             return;
         }
 */
        if (SubState != null)
        {
            SubState.ExitState();
        }


        SubState = newSubState;
        SubState.EnterState();
        SuperState = CurrentState;
    }
}
