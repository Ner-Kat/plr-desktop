using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards.InputCards.InputChar
{
    public class CharData
    {
        public int Id { get; }
        public string Name { get; set; }
        public string[] AltNames { get; set; }
        public string DateBirth { get; set; }
        public string DateDeath { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public int LocBirthId { get; set; }
        public string LocBirth { get; set; }
        public int LocDeathId { get; set; }
        public string LocDeath { get; set; }
        public int RaceId { get; set; }
        public string Race { get; set; }
        public int[] SocFormsId { get; set; }
        public int Growth { get; set; }
        public int BioFatherId { get; set; }
        public string BioFather { get; set; }
        public int BioMotherId { get; set; }
        public string BioMother { get; set; }
        public int[] ChildrenId { get; set; }
        public string[] Titles { get; set; }
        public int ColorHair { get; set; }
        public int ColorEyes { get; set; }
        public string Desc { get; set; }
        public int[] AltCharsId { get; set; }
        public string[] Additions { get; set; }
    }
}
