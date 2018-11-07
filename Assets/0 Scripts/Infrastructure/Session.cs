using Infrastructure;
using System;

public class Session {

    public City City { get; private set; }
    public ResourceCache Cache;

    public double Version;
    public string Name;
    public DateTime CreationDateTime;

    public Session() : this(DateTime.Now.ToShortDateString(), DateTime.Now){}

    public Session(string name) : this(name, DateTime.Now){}

    public Session(string name, DateTime dateTime) : this(name, dateTime, new City(Game.PlayerName)){}

    public Session(string name, DateTime dateTime, City city)
    {
        Name = name != string.Empty ? name : System.DateTime.Now.ToShortTimeString() + ":" + System.DateTime.Now.ToShortDateString();
        CreationDateTime = dateTime;
        City = city;

        Version = Convert.ToDouble(UnityEngine.Application.version);

        Cache = new ResourceCache();
    }

}
