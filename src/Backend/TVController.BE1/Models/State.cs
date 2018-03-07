using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TVController.BE1.Models
{
    public class State
    {
        public string MovieState { get; set; }
        public int Volume { get; set; }
        public int Length { get; set; }
        public int Time { get; set; }
        public string MovieTitle { get; set; }
        public bool Fullscreen { get; set; }
    }
    
}