using System;
using System.Collections.Generic;

namespace Diss.Models
{
    public class Chatroom
    {
        public int ID { get; set; }
        public int ChatID { get; set; }
        public int EventID { get; set; }
        public int PeopleCount { get; set; }
        public int AverageELO { get; set; }
        public bool MixedRoom { get; set; }
        public bool HomeTeamRoom { get; set; }
    }
}
