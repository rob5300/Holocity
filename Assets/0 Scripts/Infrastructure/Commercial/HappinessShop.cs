
using Infrastructure.Residents;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class HappinessShop : Shop
    {
        public override void OnEntityProduced(GridSystem grid)
        {
            base.OnEntityProduced(grid);

            Happiness.BonusHappiness = 0.5f;
            Happiness.BonusProviders++;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Happiness.BonusProviders--;
            if(Happiness.BonusProviders <= 0)
            {
                Happiness.BonusHappiness = 0;
            }
        }
    }
}
