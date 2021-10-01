using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class ApiMethods
    {
        public CharactersMethods Chars { get; }
        public LocationsMethods Locs { get; }
        public RacesMethods Races { get; }
        public SocialFormationsMethods SocForms { get; }
        public GendersMethods Genders { get; }
        public UsersMethods Users { get; }


        public ApiMethods(ApiServerConnection serverConnection)
        {
            Chars = new CharactersMethods(serverConnection);
            Locs = new LocationsMethods(serverConnection);
            Races = new RacesMethods(serverConnection);
            SocForms = new SocialFormationsMethods(serverConnection);
            Genders = new GendersMethods(serverConnection);
            Users = new UsersMethods(serverConnection);
        }
    }
}
