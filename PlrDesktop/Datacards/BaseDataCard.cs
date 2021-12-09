using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards
{
    public class BaseDataCard : IApiDataCard
    {
        public int? Id { get; set; }

        public string Name { get; set; }


        public Location AsLocation()
        {
            return new Location()
            {
                Id = Id,
                Name = Name
            };
        }

        public Race AsRace()
        {
            return new Race()
            {
                Id = Id,
                Name = Name
            };
        }

        public SocialFormation AsSocForm()
        {
            return new SocialFormation()
            {
                Id = Id,
                Name = Name
            };
        }

        public Character AsCharacter()
        {
            return new Character()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
