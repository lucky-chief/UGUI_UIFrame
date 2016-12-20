//***********************************//
//      有限状态机条件的基本元素
//      古光人  @right
//**********************************//
using System.Collections.Generic;
namespace FSM
{
    public enum Comparer
    {
        Equal,
        LessThan,
        LessOrEqual,
        GreaterThan,
        GreaterOrEqual
    }

    public abstract class FSMCondition : FsmObject
    {
        protected Dictionary<string, object> m_attributes = new Dictionary<string, object>();
        public bool established { get; private set; }
        public Comparer comparer { get; set; }

        public FSMCondition() { }

        public override void OnEnter()
        {
        }

        public override void Update()
        {
        }

        public override void OnExit()
        {
        }

        public abstract bool OnValidate();

        public virtual void SetEstablish(bool established)
        {
            this.established = established;
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