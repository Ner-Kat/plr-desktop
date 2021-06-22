using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.AdditionalCards;

namespace PlrDesktop.Datacards.InputCards.InputChar
{
    public class InputCharacter
    {
        public CharData MainData { get; set; }
        public ShortDataCard[] SocialFormations { get; set; }
        public ShortDataCard[] Children { get; set; }
        public ShortDataCard[] AltCharCards { get; set; }
    }
}
