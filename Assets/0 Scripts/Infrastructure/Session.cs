using Infrastructure;
using Infrastructure.Tick;
using System;
using System.Threading;

public class Session {

    public City City { get; private set; }
    public AssetCache Cache;

    private Thread _thread;
    public TickManager TickManager;

    public double Version;
    public string Name;
    public DateTime CreationDateTime;

    public Session() : this(DateTime.Now.ToShortDateString(), DateTime.Now){}

    public Session(string name) : this(name, DateTime.Now){}

    public Session(string name, DateTime dateTime)
    {
        Name = name != string.Empty ? name : System.DateTime.Now.ToShortTimeString() + ":" + System.DateTime.Now.ToShortDateString();
        CreationDateTime = dateTime;
        City = new City(Game.PlayerName, this);

        Version = Convert.ToDouble(UnityEngine.Application.version);

        Cache = new AssetCache();

        //Create a new thread for the tick manager.
        _thread = new Thread(new ThreadStart(ThreadStart));
#if UNITY_EDITOR
        UnityEngine.Debug.Log("Thread ID: " + _thread.Name);
#endif
        _thread.Start();
    }

    private void ThreadStart()
    {
        //Create a tickmanager and start it off on a new thread.
        TickManager = new TickManager(City);

        City.PostSetup();
    }
}
