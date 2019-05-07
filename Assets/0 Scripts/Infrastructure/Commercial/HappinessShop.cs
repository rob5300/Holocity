
using Infrastructure.Residents;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class HappinessShop : Shop
    {
        public override void OnEntityProduced(GridSystem grid)
        {
            base.OnEntityProduced(grid);
            Happiness.IncreaseBonusHappiness();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Happiness.ReduceBonusHappiness();
        }
    }
}
