//***********************************//
//      有限状态机的基本元素
//      古光人  @right
//**********************************//
namespace FSM
{
    public abstract class FsmObject
    {
        public string name { get; set; }
        public abstract void OnEnter();
        public abstract void Update();
        public abstract void OnExit();
    }
}
