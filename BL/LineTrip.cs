using System;

namespace BO
{
	public class LineTrip
    {
        public int TripId { get; set; }
        public int LineNumber { get; set; }
        public int LineIdTrip { get; set; }
        public int StationKey { get; set; }
        public DateTime Departure { get; set; }
        public int Frequency { get; set; }
        public string Destination { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
