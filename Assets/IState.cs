//interface fpr state machine
public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}
public interface IUnitState : IState
{
    void CallbackActionEnd();
}