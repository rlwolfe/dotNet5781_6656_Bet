using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class PassengerTravel
    {
        public int TravelId { get; set; }
        public string UserId { get; set; }
        public int LineId { get; set; }
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public DateTime DepartureHour { get; set; }
        public DateTime ArrivalHour { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
