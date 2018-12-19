
using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Tick;

namespace Infrastructure.Residents
{
    public class Resident : Tickable
    {
        #region Static Members
        public static System.Random rand;

        static Resident() {
            rand = new System.Random(System.DateTime.Now.Second);
        }

        public static string[] FirstNames = {
            "Jon", "Robert", "Damian", "Jabir", "Jacob", "Gordon", "Alyx", "Barney", "Eli", "Kliener"
        };

        public static string[] SecondNames =
        {
            "Smith", "Charles", "Jones", "Freeman", "Vance", "Breen"
        };

        private static string GetRandomFirstName()
        {
            return FirstNames[rand.Next(FirstNames.Length)];
        }

        private static string GetRandomSecondName()
        {
            return SecondNames[rand.Next(SecondNames.Length)];
        }
        #endregion

        public string FirstName;
        public string SecondName;
        public Residential Home;

        public Resident()
        {
            FirstName = GetRandomFirstName();
            SecondName = GetRandomSecondName();
        }

        public void Tick(float time)
        {
            
        }
    }
}
