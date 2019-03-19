using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Tick;
using Settings;

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

        public string FirstName { get; protected set; }
        public string SecondName { get; protected set; }
        public Residential Home;
        public Happiness Happiness { get; protected set; }
        public bool ShouldBeRemoved { get; set; }
        public bool Homeless = true;

        private Session sess;
        private float _timeWithLowHappiness = 0;
        private float _timeOffset;

        public Resident()
        {
            FirstName = GetRandomFirstName();
            SecondName = GetRandomSecondName();
            Happiness = new Happiness(this);
            sess = Game.CurrentSession;
            _timeOffset = (float)(new System.Random().NextDouble() * 10 - 5);
        }

        public void Tick(float time)
        {
            sess.AddFunds(1);
            if (Happiness.Level <= 0.2f)
            {
                _timeWithLowHappiness += time;
                if (_timeWithLowHappiness > GameSettings.ResidentTimeWithLowHappiness + _timeOffset)
                {
                    //The resident will move out.
                    MoveOut();
                }
            }
            else _timeWithLowHappiness = 0;
        }

        protected void MoveOut()
        {
            Home.RemoveResident(this);
        }
    }
}
