//***********************************//
//      有限状态机的状态
//      古光人  @right
//**********************************//
using System.Collections.Generic;
using System.Xml;
using System;
using System.Reflection;
using Reflection = System.Reflection.Assembly;

namespace FSM
{
    public sealed class FsmStateMachine
    {
        public string fsmName { get; private set; }
        public Dictionary<string, State> states { get; private set; }
        public State currentState { get; private set; }
        public State defaultState { get; private set; }
        public bool pause { get; private set; }

        private Transition outTransition;

        public FsmStateMachine(string name = "")
        {
            pause = true;
            fsmName = string.IsNullOrEmpty(name) ? GetHashCode().ToString() : name;
            states = new Dictionary<string, State>();
        }

        public FsmStateMachine(XmlDocument xmlDoc)
        {
            this.states = new Dictionary<string, State>();
            XmlNode root = xmlDoc.SelectSingleNode("StateMachine");
            fsmName = root.Attributes["name"].Value;
            XmlNodeList states = root.SelectNodes("State");
            for(int i = 0,size = states.Count; i < size; i++)
            {
                XmlNode stateNode = states[i];
                State state = new State(stateNode.Attributes["name"].Value);
                state.isDefault = stateNode.Attributes["isDefault"].Value == "true";
                XmlNodeList actions = stateNode.SelectSingleNode("Actions").SelectNodes("Action");
                for(int j = 0; j < actions.Count; j++)
                {
                    XmlNode actionNode = actions[j];
                    string className = actionNode.Attributes["name"].Value;
                    FSMAction action = (FSMAction)Reflection.GetExecutingAssembly().CreateInstance(className);
                    action.name = className;
                    XmlNodeList paramsNodes = actionNode.SelectSingleNode("params").SelectNodes("param");
                    for(int k = 0; k < paramsNodes.Count; k++)
                    {
                        XmlNode paramNode = paramsNodes[k];
                        string propertyName = paramNode.Attributes["key"].Value;
                        Type tp = action.GetType();
                        FieldInfo pInfo = tp.GetField(propertyName);
                        if (null == pInfo)
                        {
                            UnityEngine.Debug.LogError("获取条件名为： " + action.name + "的属性 " + paramNode.Attributes["key"].Value + "不存在");
                            return;
                        }
                        pInfo.SetValue(action, paramNode.Attributes["value"].Value);
                    }
                    state.AddAction(action);
                }

                XmlNodeList transitions = stateNode.SelectSingleNode("Transitions").SelectNodes("Transition");
                for (int j = 0; j < transitions.Count; j++)
                {
                    XmlNode transitionNode = transitions[j];
                    Transition trans = new Transition(transitionNode.Attributes["name"].Value,state.name,transitionNode.Attributes["toState"].Value);
                    XmlNodeList conditionsNodes = transitionNode.SelectSingleNode("Conditions").SelectNodes("Condition");
                    for(int k = 0; k < conditionsNodes.Count; k++)
                    {
                        XmlNode conditionNode = conditionsNodes[k];
                        string className = conditionNode.Attributes["name"].Value;
                        FSMCondition condition = (FSMCondition)Reflection.GetExecutingAssembly().CreateInstance(className);
                        condition.name = className;
                        XmlNodeList paramsNodes = conditionNode.SelectSingleNode("params").SelectNodes("param");
                        for (int m = 0; m < paramsNodes.Count; m++)
                        {
                            XmlNode paramNode = paramsNodes[m];
                            FieldInfo pInfo = condition.GetType().GetField(paramNode.Attributes["key"].Value);
                            if(null == pInfo)
                            {
                                UnityEngine.Debug.LogError("获取条件名为： " + condition.name + "的属性 " + paramNode.Attributes["key"].Value + "不存在");
                                return;
                            }
                           // UnityEngine.Debug.Log("获取条件名为： " + condition.name + "的属性 " + paramNode.Attributes["key"].Value + "不存在");
                            pInfo.SetValue(condition, paramNode.Attributes["value"].Value);
                        }
                        trans.AddCondition(condition);
                    }
                    state.AddTransition(trans);
                }

                AddState(state);
            }
        }

        public void Start()
        {
            pause = false;
            currentState.OnEnter();
        }

        public void Update()
        {
            if (pause) return;
            if (currentState.Validate(out outTransition))
            {
                SwitchState(outTransition.toName);
            }

            foreach (KeyValuePair<string, FSMAction> action in currentState.actions)
            {
                action.Value.Update();
            }
        }

        public void AddState(State state)
        {
            if(null != defaultState && state.isDefault)
            {
                state.isDefault = false;
                UnityEngine.Debug.LogError("状态机已经有了默认状态");
            }
            if (states.ContainsKey(state.name))
            {
                UnityEngine.Debug.LogError("状态机已经包含名为：" + state.name + "的状态！");
            }
            else
            {
                if (null == defaultState)
                {
                    if(state.isDefault)
                    {
                        currentState = state;
                        defaultState = state;
                    }
                }
                states.Add(state.name, state);
            }
        }

        public void Pause(bool pause)
        {
            this.pause = pause;
        }

        public void ReStart(bool pause = false)
        {
            currentState = defaultState;
            this.pause = pause;
        }

        static int conditionID = 1;
        public static string ConditionID
        {
            get { return GetUniqueConditionName(); }
        }
        public static string GetUniqueConditionName()
        {
            return (conditionID++).ToString();
        }

        private void SwitchState(string stateName)
        {
            State stateTo = states[stateName];
            this.currentState = stateTo;
            this.currentState.OnEnter();
            DebugCurrentState();
        }

        public void DebugCurrentState()
        {
            UnityEngine.Debug.Log("currentState: " + currentState.name);
        }



    }
}
