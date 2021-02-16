using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusStation
    {
        public int BusStationKey { get; set; }
        public string StationName { get; set; }
        public string Address { get; set; }
        public IEnumerable<int> LinesThatPass { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
