using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Models
{
    class RoommateChore
    {
        public int Id { get; set; }

        public Roommate Roommate { get; set; }

        public Chore Chore { get; set; }
    }
}
