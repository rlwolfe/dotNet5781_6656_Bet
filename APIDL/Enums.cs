using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum BusStatus { Ready, Travelling, Refueling, underMaintenance }
    public enum Areas { General, North, South, Center, Jerusalem }
    public enum UserStatus { Director, Employee, Driver, Passenger }
}
