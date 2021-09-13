namespace Roommates.Models
{
    public class RoommateChore
    {
        public int Id { get; set; }

        public Roommate Roommate { get; set; }

        public Chore Chore { get; set; }
    }
}
