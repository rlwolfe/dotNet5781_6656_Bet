using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLine
    {
        public int Id { get; set; }
        public int BusLineNumber { get; set; }
        public Areas Area { get; set; }
        public int FirstStationKey { get; set; }
        public int LastStationKey { get; set; }
        public double TotalTime { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
