using System.Threading.Tasks;
using UnityEngine;

public class CraneMachine : Machine
{
    [Space(10), SerializeField] Transform piviot;
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(CraneMachine));

        base.Initialize(initialClassName);
    }

    public async void Rotate(int degrees)
    {
        float timer = 0;
        degrees *= 90;
        Vector3 startRot = piviot.eulerAngles;

        float timeToFinish = Tick.tickLength * .5f;

        while (isRunning && timer < timeToFinish)
        {
            piviot.Rotate(0, (degrees * Time.deltaTime) / timeToFinish, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000 * timeToFinish));
            timer += Time.deltaTime;
        }

        piviot.transform.eulerAngles = startRot + Vector3.up * degrees;
    }
}
