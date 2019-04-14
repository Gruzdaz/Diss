using System;
using System.Collections.Generic;

namespace Diss.Models
{
    public class Chat
    {
        public int ChatID { get; set; }
        public int EventID { get; set; }
        public int PeopleCount { get; set; }
        public int AverageELO { get; set; }
    }
}
