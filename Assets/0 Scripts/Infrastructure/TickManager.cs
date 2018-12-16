using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Infrastructure.Grid;

namespace Infrastructure.Tick
{
    public class TickManager
    {
        public float TickDelay;
        public bool IsRunning { get; private set; }
        public City TargetCity { get; private set; }
        public ConcurrentQueue<SessionAction> SessionActionsQueue;
        public ConcurrentQueue<GridSystem> NewGridSystems;

        public delegate void SessionAction(Session s);

        private Thread _thread;
        private System.Timers.Timer _timer;
        private ElapsedEventHandler _timerHandle;

        /// <summary>
        /// All the tickables that we tick.
        /// </summary>
        private List<Tickable> TickTargets;
        /// <summary>
        /// List of referneces to the queues that all existing grid systems hold. These queues hold new tickables waiting to be added to the master TickTargets list.
        /// </summary>
        private List<ConcurrentQueue<Tickable>> GridNewTickableQueues;
        

        public TickManager(City targetCity, float tickDelay = 100)
        {
            TickDelay = tickDelay;
            TargetCity = targetCity;

            TickTargets = new List<Tickable>();
            GridNewTickableQueues = new List<ConcurrentQueue<Tickable>>();
            SessionActionsQueue = new ConcurrentQueue<SessionAction>();
            NewGridSystems = new ConcurrentQueue<GridSystem>();

            IsRunning = false;

            Start();
        }

        public void Start()
        {
            if (IsRunning) return;
            _thread = new Thread(new ThreadStart(ThreadStart));
            _thread.Start();
            IsRunning = true;
        }

        public void Stop()
        {

        }

        public bool AddSessionAction(SessionAction action)
        {
            if (action == null) return false;
            SessionActionsQueue.Enqueue(action);
            return true;
        }

        private void ThreadStart()
        {
            //The thread is open and we begin setting up the tick manager to tick.
            _timer = new System.Timers.Timer(TickDelay);
            _timerHandle = Tick;
            _timer.Elapsed += _timerHandle;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void Tick(object sender, ElapsedEventArgs args)
        {
            //This is called when we tick.

            //Execute any delegates that are waiting.
            for (int x = 0; x < SessionActionsQueue.Count; x++)
            {
                SessionAction sessionAction;
                while (SessionActionsQueue.TryDequeue(out sessionAction)) sessionAction.Invoke(Game.CurrentSession);
            }

            //Check if there are any new grids
            TryDequeingNewGrids();

            //Test to see if there are any new targets to get.
            TryDequeingNewTargets();

            //Execute all ticks
            for (int i = 0; i < TickTargets.Count; i++)
            {
                TickTargets[i].Tick();
            }
        }

        private void TryDequeingNewGrids()
        {
            for (int i = 0; i < NewGridSystems.Count; i++)
            {
                GridSystem newGrid;
                while (NewGridSystems.TryDequeue(out newGrid)) GridNewTickableQueues.Add(newGrid.TickAddQueue);
            }
        }

        /// <summary>
        /// Try to dequeue any new targets in the grid systems queues. If there are any we add them to the master list.
        /// </summary>
        private void TryDequeingNewTargets()
        {
            for (int i = 0; i < GridNewTickableQueues.Count; i++)
            {
                for (int j = 0; j < GridNewTickableQueues[i].Count; j++)
                {
                    Tickable newTickable;
                    while (GridNewTickableQueues[i].TryDequeue(out newTickable)) TickTargets.Add(newTickable);
                }
            }
        }
    }
}
