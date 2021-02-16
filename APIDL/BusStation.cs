using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusStation
    {
        public int BusStationKey { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string StationName { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
