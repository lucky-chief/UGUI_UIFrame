using UnityEngine;

namespace FSM
{
    public class DebugLog : FSMAction
    {
        public string content;

        public override void OnEnter()
        {
            Debug.Log("FSMAction::DebugLog[" + content + "]");
            base.OnEnter();
        }
    }
}
