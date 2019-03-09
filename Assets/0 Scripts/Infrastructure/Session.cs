using Infrastructure;
using Infrastructure.Tick;
using Settings;
using System;
using System.Threading;

public class Session {

    public GameSettings Settings { get; }
    public City City { get; private set; }
    public uint Funds { get; private set; }
    public readonly string CurrencySymbol;
    public AssetCache Cache;
    public TickManager TickManager;
    public double Version;
    public string Name;
    public DateTime CreationDateTime;

    private const uint _fundsCap = 1000000;
    private Thread _thread;

    public Session(string name, DateTime dateTime)
    {
        DateTime now = System.DateTime.Now;
        Name = name != string.Empty ? name : now.ToShortTimeString() + ":" + now.ToShortDateString();
        CreationDateTime = dateTime;
        City = new City(Game.PlayerName, this);
        Version = Convert.ToDouble(UnityEngine.Application.version);
        Cache = new AssetCache();
        Funds = GameSettings.StartingMoney;

        //Create a new thread for the tick manager.
        _thread = new Thread(new ThreadStart(ThreadStart));
        _thread.Start();
    }

    public Session() : this(DateTime.Now.ToShortDateString(), DateTime.Now) { }

    public Session(string name) : this(name, DateTime.Now) { }

    private void ThreadStart()
    {
        //Create a tickmanager and start it off on a new thread.
        TickManager = new TickManager(City);
        TickManager.Start();
        City.PostSetup();
    }

    /// <summary>
    /// Add the amount to the players funds.
    /// </summary>
    /// <param name="amount">How much to add</param>
    /// <returns></returns>
    public uint AddFunds(uint amount)
    {
        if (amount > _fundsCap) amount = _fundsCap;
        return Funds += amount;
    }

    /// <summary>
    /// Take the requested amount off the players funds. If there is not enough funds, nothing is taken. Returns if the operation was successful or not.
    /// </summary>
    /// <param name="amount">Amount to attempt to take.</param>
    /// <returns>If the operation was successful.</returns>
    public bool TakeFunds(uint amount)
    {
        if (amount > Funds) return false;
        else Funds -= amount;
        return true;
    }
}
