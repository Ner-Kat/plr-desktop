using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards
{
    public interface IApiDataCard
    {
        public int? Id { get; set; }

        public string Name { get; set; }
    }
}
