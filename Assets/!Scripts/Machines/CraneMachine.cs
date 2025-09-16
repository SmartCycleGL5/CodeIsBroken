using System.Threading.Tasks;
using UnityEngine;

public class CraneMachine : Machine
{
    [Space(10), SerializeField] Transform piviot;
    protected override void Start()
    {
        AddMethodsAsIntegrated(typeof(CraneMachine));

        base.Start();
    }

    public async void Rotate(int degrees)
    {
        float timer = 0;

        while (isRunning && timer < 1)
        {
            piviot.Rotate(0, degrees * Time.deltaTime, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
            timer += Time.deltaTime;
        }
    }
}
