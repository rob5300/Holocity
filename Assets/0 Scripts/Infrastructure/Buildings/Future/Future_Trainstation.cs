
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Trainstation : HappinessShop
    {
        public Future_Trainstation()
        {
            PrefabName = "Future/TrainStation";
            Name = "Train Station";
            Cost = 1050;

            AddJobs(4);
        }
    }
}
