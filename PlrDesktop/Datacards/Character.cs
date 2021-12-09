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

        private int? genderId = null;
        public int? GenderId 
        {
            get
            {
                return genderId;
            }
            set
            {
                genderId = value;
                gender = null;
            }
        }

        private Gender gender = null;
        public Gender Gender 
        {
            get
            {
                return gender;
            }
            set
            { 
                gender = value;
                genderId = value is not null ? value.Id : null;
            }
        }

        private int? locBirthId = null;
        public int? LocBirthId 
        {
            get
            {
                return locBirthId;
            }
            set
            {
                locBirthId = value;
                locBirth = null;
            }
        }

        private Location locBirth = null;
        public Location LocBirth
        {
            get
            {
                return locBirth;
            }
            set
            {
                locBirth = value;
                locBirthId = value is not null ? value.Id : null;
            }
        }

        private int? locDeathId = null;
        public int? LocDeathId
        {
            get
            {
                return locDeathId;
            }
            set
            {
                locDeathId = value;
                locDeath = null;
            }
        }

        private Location locDeath = null;
        public Location LocDeath
        {
            get
            {
                return locDeath;
            }
            set
            {
                locDeath = value;
                locDeathId = value is not null ? value.Id : null;
            }
        }

        private int? raceId = null;
        public int? RaceId
        {
            get
            {
                return raceId;
            }
            set
            {
                raceId = value;
                race = null;
            }
        }

        private Race race = null;
        public Race Race
        {
            get
            {
                return race;
            }
            set
            {
                race = value;
                raceId = value is not null ? value.Id : null;
            }
        }

        public int? Growth { get; set; } = null;

        private int? bioFatherId = null;
        public int? BioFatherId
        {
            get
            {
                return bioFatherId;
            }
            set
            {
                bioFatherId = value;
                bioFather = null;
            }
        }

        private Character bioFather = null;
        public Character BioFather
        {
            get
            {
                return bioFather;
            }
            set
            {
                bioFather = value;
                bioFatherId = value is not null ? value.Id : null;
            }
        }

        private int? bioMotherId = null;
        public int? BioMotherId
        {
            get
            {
                return bioMotherId;
            }
            set
            {
                bioMotherId = value;
                bioMother = null;
            }
        }

        private Character bioMother = null;
        public Character BioMother
        {
            get
            {
                return bioMother;
            }
            set
            {
                bioMother = value;
                bioMotherId = value is not null ? value.Id : null;
            }
        }

        private int[] childrenIds = null;
        public int[] ChildrenIds 
        { 
            get
            {
                return childrenIds;
            }
            set
            {
                childrenIds = value;
                children = null;
            }
        }

        private List<Character> children = null;
        public List<Character> Children 
        { 
            get
            {
                return children;
            }
            set
            {
                children = value;
                childrenIds = value is not null ? new int[value.Count] : null;
                for (int i = 0; i < children.Count; i++)
                    childrenIds[i] = children[i].Id.Value;
            }
        }

        public string[] Titles { get; set; } = null;

        public string ColorHair { get; set; } = null;

        public string ColorEyes { get; set; } = null;

        //public int? ColorHair { get; set; } = null;

        //public int? ColorEyes { get; set; } = null;

        public string Desc { get; set; } = null;

        private int[] altCharsIds = null;
        public int[] AltCharsIds
        {
            get
            {
                return altCharsIds;
            }
            set
            {
                altCharsIds = value;
                altChars = null;
            }
        }

        private List<Character> altChars = null;
        public List<Character> AltChars
        {
            get
            {
                return altChars;
            }
            set
            {
                altChars = value;
                altCharsIds = value is not null ? new int[value.Count] : null;
                for (int i = 0; i < altChars.Count; i++)
                    altCharsIds[i] = altChars[i].Id.Value;
            }
        }

        public List<AdditionalCharData> Additions { get; set; } = null;

        private int[] socFormsIds = null;
        public int[] SocFormsIds
        { 
            get
            {
                return socFormsIds;
            }
            set
            {
                socFormsIds = value;
                socForms = null;
            }
        }

        private List<SocialFormation> socForms = null;
        public List<SocialFormation> SocForms
        { 
            get
            {
                return socForms;
            }
            set
            {
                socForms = value;
                socFormsIds = value is not null ? new int[value.Count] : null;
                for (int i = 0; i < socForms.Count; i++)
                    socFormsIds[i] = socForms[i].Id.Value;
            }
        }


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
