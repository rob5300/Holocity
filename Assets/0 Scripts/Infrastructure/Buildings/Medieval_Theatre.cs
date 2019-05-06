using Infrastructure.Residents;

namespace Infrastructure.Grid.Entities.Buildings
{
    [TileEntityMeta("", MultiTileSize.x2)]
    public class Medieval_Theatre : Commercial2x2
    {
        public Medieval_Theatre()
        {
            Name = "Theatre";
            PrefabName = "Medieval/Theatre";
        }

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
            if (Happiness.BonusProviders <= 0)
            {
                Happiness.BonusHappiness = 0;
            }
        }
    }
}