using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.AdditionalCards;

namespace PlrDesktop.Datacards.MainCards
{
    public class CharacterShort : DataCard
    {
        public List<string> AltNames { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateDeath { get; set; }
        public int GenderId { get; set; }
        public int LocBirthId { get; set; }
        public int LocDeathId { get; set; }
        public int RaceId { get; set; }
        public int Growth { get; set; }
        public int BioFatherId { get; set; }
        public int BioMotherId { get; set; }
        public List<int> ChildrenId { get; set; }
        public List<string> Titles { get; set; }
        public int ColorHair { get; set; }
        public int ColorEyes { get; set; }
        public string Desc { get; set; }
        public string[] Additions { get; set; }
        public List<int> SocialFormationsId { get; set; }
        public List<int> AltCharCardsId { get; set; }
    }
}
