using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class FollowingStations
    {
        public int KeyStation1 { get; set; }
        public int KeyStation2 { get; set; }
        public double Distance { get; set; }
        public double AverageJourneyTime { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
