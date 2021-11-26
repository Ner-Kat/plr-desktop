using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.Utils;

namespace PlrDesktop.Datacards
{
    public class Character : IApiDataCard, IChangableApiCard
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; } = null;

        public string[] AltNames { get; set; } = null;

        public string DateBirth { get; set; } = null;

        public string DateDeath { get; set; } = null;

        public int? GenderId { get; set; } = null;
        public Gender Gender { get; set; } = null;

        public int? LocBirthId { get; set; } = null;
        public Location LocBirth { get; set; } = null;

        public int? LocDeathId { get; set; } = null;
        public Location LocDeath { get; set; } = null;

        public int? RaceId { get; set; } = null;
        public Race Race { get; set; } = null;

        public int? Growth { get; set; } = null;

        public int? BioFatherId { get; set; } = null;
        public Character BioFather { get; set; } = null;

        public int? BioMotherId { get; set; } = null;
        public Character BioMother { get; set; } = null;

        public int[] ChildrenIds { get; set; } = null;
        public List<Character> Children { get; set; } = null;

        public string[] Titles { get; set; } = null;

        public string ColorHair { get; set; } = null;

        public string ColorEyes { get; set; } = null;

        public string Desc { get; set; } = null;

        public int[] AltCharsIds { get; set; } = null;
        public List<Character> AltChars { get; set; } = null;

        public List<AdditionalCharData> Additions { get; set; } = null;

        public int[] SocFormsIds { get; set; } = null;
        List<SocialFormation> SocForms { get; set; } = null;


        public object ForChanging()
        {
            return new
            {
                Id = Id,
                Name = Name,
                AltNames = AltNames,
                DateBirth = DateBirth,
                DateDeath = DateDeath,
                GenderId = GenderId,
                LocBirthId = LocBirthId,
                LocDeathId = LocDeathId,
                RaceId = RaceId,
                Growth = Growth,
                BioFatherId = BioFatherId,
                BioMotherId = BioMotherId,
                ChildrenIds = ChildrenIds,
                Titles = Titles,
                ColorHair = ColorHair,
                ColorEyes = ColorEyes,
                Desc = Desc,
                AltCharsIds = AltCharsIds,
                Additions = Additions,
                SocFormsIds = SocFormsIds
            };
        }

        public object ForAdding()
        {
            return new
            {
                Name = Name,
                AltNames = AltNames,
                DateBirth = DateBirth,
                DateDeath = DateDeath,
                GenderId = GenderId,
                LocBirthId = LocBirthId,
                LocDeathId = LocDeathId,
                RaceId = RaceId,
                Growth = Growth,
                BioFatherId = BioFatherId,
                BioMotherId = BioMotherId,
                ChildrenIds = ChildrenIds,
                Titles = Titles,
                ColorHair = ColorHair,
                ColorEyes = ColorEyes,
                Desc = Desc,
                AltCharsIds = AltCharsIds,
                Additions = Additions,
                SocFormsIds = SocFormsIds
            };
        }
    }
}
