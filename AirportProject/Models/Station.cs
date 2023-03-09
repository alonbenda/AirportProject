namespace AirportProject.Models
{
    public class Station
    {
        private bool state;


        public bool State
        {
            get { return state; }
        }

        private Airplane plane;

        public Airplane Plane
        {
            get { return plane; }
        }


        public int Id { get; set; }

        readonly SemaphoreSlim sem = new SemaphoreSlim(1);

        public Station(int id)
        {
            Id = id;
        }

        public async Task Enter(Airplane airplane)
        {
            await sem.WaitAsync();
            plane = airplane;
            state = true;
        }

        public bool Exit(Airplane airplane)
        {
            if (plane != null)
            {
                if (plane.Equals(airplane))
                {
                    plane = null;
                    state = false;
                    sem.Release();
                    return true;
                }
            }
            return false;
        }
    }
}
