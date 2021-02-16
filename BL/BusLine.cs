using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    public class BusLine
    {
        public int Id { get; set; }
        public int BusLineNumber { get; set; }
        public Areas Area { get; set; }
        public int FirstStationKey { get; set; }
        public int LastStationKey { get; set; }
        public double TotalDistance { get; set; }
        public double TotalTime { get; set; }
        public IEnumerable<int> AllStationsOfLine { get; set; }
        public override string ToString()
        {
            return BusLineNumber.ToString();
            //return this.ToStringProperty();
        }
    }
}
