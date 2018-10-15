using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerClick  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns>If the click was successful.</returns>
    bool OnPlayerClick(object sender, PlayerClickEventData eventData);
	
}
