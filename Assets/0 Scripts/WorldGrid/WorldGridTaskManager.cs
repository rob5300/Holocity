using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldGrid))]
public class WorldGridTaskManager : MonoBehaviour
{
    public WorldGrid WorldGridTarget;
    public Queue<WorldGridTask> WorldGridTasks;
    public delegate void WorldGridTask(WorldGrid wGrid);

    private void Awake()
    {
        WorldGridTasks = new Queue<WorldGridTask>();
    }

    public void Start()
    {
        WorldGridTarget = GetComponent<WorldGrid>();
    }

    void Update ()
    {
		if(WorldGridTasks.Count != 0)
        {
            //This is currently NOT thread safe. Will try to fix later with locks.
            //This is not thread safe because it is possible for either thread to both access this Queue at the same time.
            //I am not using the thread safe ConcurrentQueue due to the reading thread having to wait on other threads and we dont want that on the unity thread.
            //Best solution is one that locks the list and only the tick thread (e.g. House) waits to access the list.
            WorldGridTasks.Dequeue().Invoke(WorldGridTarget);
        }
	}
}
