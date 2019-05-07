
using System;

public static class Game {

    public static Session CurrentSession { get; private set; }

    //Should get the name of the current user. If its broken remove
    public static string PlayerName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

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
            CurrentSession.Initialize();
            return true;
        }

        return false;
    }
    
}
