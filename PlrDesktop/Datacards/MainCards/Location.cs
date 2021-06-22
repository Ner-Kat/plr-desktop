using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.AdditionalCards;

namespace PlrDesktop.Datacards.MainCards
{
    public class Location : DataCard
    {
        public string Desc { get; set; }
        public Location ParentLoc { get; set; }
        public List<Location> Children { get; set; }
    }
}
