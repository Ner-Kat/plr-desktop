using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.AdditionalCards;

namespace PlrDesktop.Datacards.MainCards
{
    public class Character : DataCard
    {
        public List<string> AltNames { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateDeath { get; set; }
        public Gender Gender { get; set; }
        public Location LocBirth { get; set; }
        public Location LocDeath { get; set; }
        public Race Race { get; set; }
        public int Growth { get; set; }
        public Character BioFather { get; set; }
        public Character BioMother { get; set; }
        public List<Character> Children { get; set; }
        public List<string> Titles { get; set; }
        public int ColorHair { get; set; }
        public int ColorEyes { get; set; }
        public string Desc { get; set; }
        public string[] Additions { get; set; }
        public List<SocialFormation> SocialFormations { get; set; }
        public List<Character> AltCharCards { get; set; }
    }
}
