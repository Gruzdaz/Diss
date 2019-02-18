using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diss.ViewModels
{
    public class EventView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}