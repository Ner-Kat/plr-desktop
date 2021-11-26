using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards
{
    public class SocialFormation : IApiDataCard, IChangableApiCard
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; } = null;

        public string Desc { get; set; } = null;


        public object ForChanging()
        {
            return new
            {
                Id = Id,
                Name = Name,
                Desc = Desc
            };
        }

        public object ForAdding()
        {
            return new
            {
                Name = Name,
                Desc = Desc
            };
        }
    }
}
