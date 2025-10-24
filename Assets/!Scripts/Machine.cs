using SharpCube;

public class Machine : BaseMachine
{
    protected virtual void Start()
    {
        Tick.OnEndingTick += Reset;
        AddMethodsAsIntegrated(typeof(Machine));
    }

    public void Print(object debug)
    {
        PlayerConsole.Log(debug);
    }

    public virtual void Reset()
    {

    }

    protected virtual void OnDestroy()
    {
        base.OnDestroy();
        Tick.OnEndingTick -= Reset;
    }
}
