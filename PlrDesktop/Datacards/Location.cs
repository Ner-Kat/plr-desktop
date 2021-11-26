using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards
{
    public class Location : IApiDataCard, IChangableApiCard
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; } = null;

        public string Desc { get; set; } = null;

        private int? parentLocId = null;
        public int? ParentLocId { 
            get
            {
                return parentLocId;
            }
            set
            {
                parentLocId = value;
                parentLoc = null;
            }
        }

        private Location parentLoc = null;
        public Location ParentLoc
        {
            get
            {
                return parentLoc;
            }
            set
            {
                parentLoc = value;
                parentLocId = value is not null ? value.Id : null;
            }
        }

        public List<Location> Children { get; set; } = null;


        public object ForChanging()
        {
            return new
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                ParentLocId = ParentLocId
            };
        }

        public object ForAdding()
        {
            return new
            {
                Name = Name,
                Desc = Desc,
                ParentLocId = ParentLocId
            };
        }
    }
}
