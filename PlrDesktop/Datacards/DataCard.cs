using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.AdditionalCards;

namespace PlrDesktop.Datacards
{
    public abstract class DataCard
    {
        public int Id { get; set;  } = 0;
        public string Name { get; set; } = "";

        public DataCard()
        {
        }

        public DataCard(int id)
        {
            Id = id;
        }

        public DataCard(ShortDataCard dataCard)
        {
            Id = dataCard.Id;
            Name = dataCard.Name;
        }
    }
}