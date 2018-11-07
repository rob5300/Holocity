
using System;

public static class Game {

    public static Session CurrentSession { get; private set; }

    public static string PlayerName = "Player";

    static Game(){
        //Initalize Game Data
    }

    /// <summary>
    /// Set the current session to be the provided instance.
    /// </summary>
    /// <param name="session">The new session</param>
    /// <returns>If the operation was successful</returns>
    public static bool SetSession(Session session)
    {
        if(session != null && session.Version <= Convert.ToDouble(UnityEngine.Application.version))
        {
            CurrentSession = session;
            return true;
        }

        return false;
    }
}
