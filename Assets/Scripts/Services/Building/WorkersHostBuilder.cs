using UnityEngine;

[RequireComponent(typeof(Worker))]
public class WorkersHostBuilder : BuilderBase<WorkersHost>
{
    protected override void UpdateBuildingProps(WorkersHost building)
    {
        base.UpdateBuildingProps(building);

        if (TryGetComponent(out Worker currentWorker))
        {
            building.BindWorker(currentWorker);
        }
    }
}