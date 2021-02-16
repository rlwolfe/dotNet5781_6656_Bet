using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace APIDL

{
    //CRUD Logic:
    // Create - add new instance
    // Request - ask for an instance or for a collection
    // Update - update properties of an instance
    // Delete - delete an instance
    public interface IDAL
    {
        #region Station
        IEnumerable<DO.BusStation> GetAllStations();
        IEnumerable<DO.BusStation> GetAllStationsBy(Predicate<DO.BusStation> predicate);
        DO.BusStation GetStation(int id);
        void AddStation(DO.BusStation station);
        void UpdateStation(DO.BusStation station);
        void UpdateStation(int id, Action<DO.BusStation> update);
        void DeleteStation(int id);
        #endregion

        #region BusLine
        IEnumerable<DO.BusLine> GetAllLines();
        IEnumerable<DO.BusLine> GetAllLinesBy(Predicate<DO.BusLine> predicate);
        DO.BusLine GetLine(int lineNum, Areas area);
        DO.BusLine GetLine(int Id);
        void AddLine(DO.BusLine line);
        void UpdateLine(DO.BusLine line);
        void UpdateLine(BusLine line, Action<DO.BusLine> update);
        void DeleteLine(BusLine line);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStations();
        IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStation(int lineId, int stationKey);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.LineStation station);
        void UpdateLineStation(LineStation lineStation, Action<DO.LineStation> update);
        void DeleteLineStation(LineStation lineStation);
        void DeleteLineStation(Predicate<LineStation> predicate);
        #endregion

        #region FollowingStations
        IEnumerable<DO.FollowingStations> GetAllFollowingStations();
        IEnumerable<DO.FollowingStations> GetAllFollowingStationsBy(Predicate<FollowingStations> predicate);
        DO.FollowingStations GetFollowingStations(BusStation station1, BusStation station2);
        void AddFollowingStations(BusStation station1, BusStation station2);
        void UpdateFollowingStations(BusStation station1, BusStation station2);
        void UpdateFollowingStations(BusStation station1, BusStation station2, Action<FollowingStations> update);
        void DeleteFollowingStations(BusStation station1, BusStation station2);
        #endregion

        #region LineTrip
        DO.LineTrip GetLineTrip(int lineId, int stationKey);
        IEnumerable<DO.LineTrip> GetAllLineTrips();
        void AddLineTrip(LineTrip trip);
        void DeleteLineTrip(LineTrip trip);
        TimeSpan CalculateDistance(LineTrip trip);
        #endregion
    }
}
