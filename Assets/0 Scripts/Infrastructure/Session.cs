using Infrastructure;
using Infrastructure.Tick;
using Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Session {

    public GameSettings Settings { get; }
    public City City { get; private set; }
    public AssetCache Cache;
    public TickManager TickManager;
    public double Version;
    public string Name;
    public DateTime CreationDateTime;

    public event Action<Session> OnSessionReady;

    public TaskManager TaskManager { get; private set; }

    private const uint _fundsCap = 1000000;
    private Thread _thread;

    public Session(TimePeriod timePeriod) : this(new GameSettings(timePeriod), Game.PlayerName, DateTime.Now)
    {
        
    }

    public Session(GameSettings savedSettings, string name, DateTime time)
    {
        //Restore these game settings
        DateTime now = time;
        Name = name;
        CreationDateTime = DateTime.Now;
        Settings = savedSettings;
        City = new City(Game.PlayerName, this);
        Version = Convert.ToDouble(UnityEngine.Application.version);
        Cache = new AssetCache(savedSettings);

        TaskManager = new UnityEngine.GameObject().AddComponent<TaskManager>();
        TaskManager.gameObject.name = "TaskManager";

        //Create a new thread for the tick manager.
        _thread = new Thread(new ThreadStart(ThreadStart));
        _thread.Start();
    }

    public void DestroySession()
    {
        TickManager.Stop();
        TickManager.Dispose();
        OnSessionReady = null;
    }

    private void ThreadStart()
    {
        try
        {
            //Create a tickmanager and start it off on a new thread.
            TickManager = new TickManager(City);
            TickManager.Initialize();
            City.PostSetup();
            OnSessionReady?.Invoke(this);
        }
        catch (Exception e)
        {
            //Log the exception on the main thread.
            TaskManager.Tasks.Enqueue(() => { UnityEngine.Debug.LogError(e.Message); });
        }
    
    }

    /// <summary>
    /// Add the amount to the players funds.
    /// </summary>
    /// <param name="amount">How much to add</param>
    /// <returns></returns>
    public uint AddFunds(uint amount)
    {
        if (amount > _fundsCap) amount = _fundsCap;
        return Settings.Funds += amount;
    }

    /// <summary>
    /// Take the requested amount off the players funds. If there is not enough funds, nothing is taken. Returns if the operation was successful or not.
    /// </summary>
    /// <param name="amount">Amount to attempt to take.</param>
    /// <returns>If the operation was successful.</returns>
    public bool TakeFunds(uint amount)
    {
        if (amount > Settings.Funds) return false;
        else Settings.Funds -= amount;
        return true;
    }

    /// <summary>
    /// Unload the session
    /// </summary>
    public void Unload()
    {

    }

    public void Initialize()
    {
        BuildingLibrary.Initialize(Settings);
    }
}
