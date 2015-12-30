using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityWebsite.Model.Page;

namespace UniversityWebsite.Model
{
    public class HomeVm
    {
        public HomeVm()
        {
            Tiles = new List<TileViewModel>();
        }
        public List<TileViewModel> Tiles { get; set; }
        public NavMenuVm NavMenu { get; set; } 
    }
}