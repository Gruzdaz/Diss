using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diss.ViewModels
{
    public class ChatroomView
    {
        public int ChatID { get; set; }
        public int EventID { get; set; }
        public List<string> Users { get; set; }

    }
}