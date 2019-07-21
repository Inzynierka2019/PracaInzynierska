namespace Models
{
    public interface IDrivable
    {
        void DriveForward();
    }

    public class Car : IDrivable
    {
        public int Name { get; set; }

        public string State { get; set; }

        public void DriveForward()
        {
            this.State = "Forward";
        }
    }
}
