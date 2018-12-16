using Infrastructure;
using Infrastructure.Tick;
using System;
using System.Threading;

public class Session {

    public City City { get; private set; }
    public ResourceCache Cache;

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

        Cache = new ResourceCache();

        //Create a tickmanager and start it off on a new thread.
        TickManager = new TickManager(City);
    }

}
