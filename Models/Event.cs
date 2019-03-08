using System;
using System.Collections.Generic;

namespace Diss.Models
{
    public class Event
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}
