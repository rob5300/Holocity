using Infrastructure;

namespace Infrastructure.Grid.Entities
{
    public class TileEntity
    {
        public bool CanBeMoved = true;

        public virtual void OnEntityProduced(GridSystem grid)
        {

        }

        public virtual void OnInteract()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void Tick()
        {

        }
    }
}