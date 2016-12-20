//***********************************//
//      有限状态机动作的基本元素
//      古光人  @right
//**********************************//
using System.Collections.Generic;

namespace FSM
{
    public abstract class FSMAction : FsmObject
    {
        protected Dictionary<string, object> m_attributes = new Dictionary<string, object>();
        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void Update()
        {
        }


        public virtual object this[string key]
        {
            set
            {
                m_attributes[key] = value;
            }
            get
            {
                if (m_attributes.ContainsKey(key))
                {
                    return m_attributes[key];
                }
                return null;
            }
        }
    }
}