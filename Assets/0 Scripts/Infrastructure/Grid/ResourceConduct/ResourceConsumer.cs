using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities
{
    public class ResourceConsumer : ResourceConductEntity
    {
        public override void OnNewResourceViaConnection_Event(List<ResourceData> resourceData, HashSet<GridTile> tilesToIgnore)
        {
            //We want to take the resource and store it for use.
        }

    }
}
