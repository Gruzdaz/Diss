using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diss.ViewModels
{
    public class PreferencesView
    {
        public bool SupportsHomeTeam { get; set; }
        public bool AllowsMixedRooms { get; set; }
        public int EventID { get; set; }
    }
}