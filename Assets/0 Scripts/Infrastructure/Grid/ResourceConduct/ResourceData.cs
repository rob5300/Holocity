using CityResources;
using Infrastructure.Grid.Entities;
using System.Collections;

public struct ResourceData {

    public Resource resource;
    public TileEntity entityOrigin;

    public ResourceData(Resource resource, TileEntity entityOrigin)
    {
        this.resource = resource;
        this.entityOrigin = entityOrigin;
    }
}
