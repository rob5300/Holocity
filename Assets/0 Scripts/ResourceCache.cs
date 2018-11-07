using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceCache
{
    public GameObject TileBorder;

    public ResourceCache()
    {
        TileBorder = Resources.Load<GameObject>("Tile Border");
    }
}
