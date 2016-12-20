//***********************************//
//      有限状态机过渡
//      古光人  @right
//**********************************//

using System.Collections.Generic;
namespace FSM
{
    public sealed class Transition
    {
        public string name { get; private set; }
        public FsmStateMachine stateMachine { get; set; }
        public string fromName { get; private set; }
        public string toName { get; private set; }
        public Dictionary<string, FSMCondition> conditions { get; private set; }

        public Transition(string name,string from,string to)
        {
            this.name = name;
            fromName = from;
            toName = to;
            conditions = new Dictionary<string, FSMCondition>();
        }


        public void OnEnter()
        {
            foreach (KeyValuePair<string, FSMCondition> condition in conditions)
            {
                condition.Value.OnEnter();
            }
        }

        public void OnExit()
        {
            foreach (KeyValuePair<string, FSMCondition> condition in conditions)
            {
                condition.Value.OnExit();
            }
        }

        public void AddCondition(FSMCondition condition)
        {
            if (conditions.ContainsKey(condition.name))
            {
                UnityEngine.Debug.LogError("状态" + name + "已经包含名为：" + condition.name + "的动作！");
            }
            else
            {
                conditions.Add(condition.name, condition);
            }
        }

        public void RmCondition(string conditionName)
        {
            conditions.Remove(conditionName);
        }

        public bool Validate()
        {
            bool established = false;
            foreach (KeyValuePair<string, FSMCondition> condition in conditions)
            {
                established = condition.Value.OnValidate();
                //如果有一个条件没满足，则返回
                if (!established)
                {
                    break;
                }
            }
            return established;
        }
    }
}
