using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldGrid))]
public class WorldGridTaskManager : MonoBehaviour
{
    public WorldGrid WorldGridTarget;
    public ConcurrentQueue<WorldGridTask> WorldGridTasks;
    public delegate void WorldGridTask(WorldGrid wGrid);

    private void Awake()
    {
        WorldGridTasks = new ConcurrentQueue<WorldGridTask>();
    }

    public void Start()
    {
        WorldGridTarget = GetComponent<WorldGrid>();
    }

    void Update ()
    {
		if(WorldGridTasks.Count != 0)
        {
            WorldGridTask task;
            if(WorldGridTasks.TryDequeue(out task))
            {
                task.Invoke(WorldGridTarget);
            }
        }
	}
}
