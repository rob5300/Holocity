using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceCache
{
    public GameObject TileBorder;
    public GameObject MoveButton;

    public ResourceCache()
    {
        TileBorder = Resources.Load<GameObject>("Tile Border");
        MoveButton = Resources.Load<GameObject>("Move Button");
    }
}
