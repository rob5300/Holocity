using CityResources;
using Infrastructure.Grid.Entities;
using System;

public class ResourceData {

    public Resource resource;
    public TileEntity entityOrigin;

    public event Action OriginDestroyed;

    public int id;

    public ResourceData(Resource resource, TileEntity entityOrigin)
    {
        this.resource = resource;
        this.entityOrigin = entityOrigin;
    }

    public void Destroy()
    {
        OriginDestroyed?.Invoke();
    }
}
