//***********************************//
//      有限状态机的状态
//      古光人  @right
//**********************************//
using System.Collections.Generic;
namespace FSM
{
    public sealed class State
    {
        public Dictionary<string, FSMAction> actions { get; private set; }
        public Dictionary<string, Transition> transitions { get; private set; }
        public bool isDefault { get; set; }
        public string name { get; private set; }

        public State(string name, bool isDefault = false)
        {
            this.name = name;
            this.isDefault = isDefault;
            actions = new Dictionary<string, FSMAction>();
            transitions = new Dictionary<string, Transition>();
        }

        public void OnEnter()
        {
            foreach (KeyValuePair<string, FSMAction> action in actions)
            {
                action.Value.OnEnter();
            }
            foreach (KeyValuePair<string, Transition> transition in transitions)
            {
                transition.Value.OnEnter();
            }
        }

        public void Update()
        {

            foreach (KeyValuePair<string, FSMAction> action in actions)
            {
                action.Value.Update();
            }
        }

        public bool Validate(out Transition outTransition)
        {
            outTransition = null;
            foreach (KeyValuePair<string, Transition> transition in transitions)
            {
                if (transition.Value.Validate())
                {
                    OnExit();
                    outTransition = transition.Value;
                    return true;
                }
            }
            return false;
        }

        public void OnExit()
        {
            foreach (KeyValuePair<string, FSMAction> action in actions)
            {
                action.Value.OnExit();
            }
            foreach (KeyValuePair<string, Transition> transition in transitions)
            {
                transition.Value.OnExit();
            }
        }

        public void AddAction(FSMAction action)
        {
            if (actions.ContainsKey(action.name))
            {
                UnityEngine.Debug.LogError("状态" + name + "已经包含名为：" + action.name + "的动作！");
            }
            else
            {
                actions.Add(action.name, action);
            }
        }

        public void AddTransition(Transition transition)
        {
            if (transitions.ContainsKey(transition.name))
            {
                UnityEngine.Debug.LogError("状态" + name + "已经包含名为：" + transition.name + "的过渡条件！");
                return;
            }
            if (transition.fromName != name)
            {
                UnityEngine.Debug.LogError("过渡条件的起始状态不是名为 " + name + "的状态！");
                return;
            }

            transitions.Add(transition.name, transition);
        }

    }
}