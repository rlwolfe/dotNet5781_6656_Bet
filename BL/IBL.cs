using System;
using System.Collections.Generic;

namespace BLApi
{
	public interface IBL
    {
        #region BusLine
        DO.Areas AreasAdapter(BO.Areas areaBO);
        IEnumerable<BO.BusLine> GetAllBusLines();
        IEnumerable<BO.BusLine> GetAllBusLinesBy(Predicate<BO.BusLine> predicate);
        BO.BusLine GetBusLine(int lineNumber, DO.Areas area);
        BO.BusLine GetBusLine(int id);
        void AddBusLine(BO.BusLine newLine);
        void UpdateBusLine(BO.BusLine line);
        void UpdateBusLine(int id, Action<DO.BusLine> action);
        void DeleteBusLine(BO.BusLine line);
        #endregion

        #region BusStation
        BO.BusStation BusStationDoBoAdapter(DO.BusStation stationDo);
        IEnumerable<BO.BusStation> GetAllBusStations();
        IEnumerable<BO.BusStation> GetAllBusStationsBy(Predicate<BO.BusStation> predicate);
        BO.BusStation GetBusStation(int key);
        void AddStation(BO.BusStation station);
        void UpdateBusStation(BO.BusStation station);
        void UpdateBusStation(int key, Action<BO.BusStation> action);
        void DeleteStation(int key);
        #endregion

        #region LineStation
        IEnumerable<BO.LineStation> GetAllLineStations();
        IEnumerable<BO.LineStation> GetAllLineStationsBy(Predicate<BO.LineStation> predicate);
        BO.LineStation GetLineStation(int id);
        BO.LineStation GetLineStation(int lineId, int stationId);
        void AddLineStation(BO.LineStation lineStation);
        void UpdateLineStation(BO.LineStation lineStation);
        void DeleteLineStation(BO.LineStation lineStation);
        #endregion

        #region FollowingStations
        BO.LineTrip LineTripDoBoAdapter(DO.LineTrip lineTripDo);
        BO.FollowingStations GetFollowingStations(BO.BusStation station1, BO.BusStation station2);
        #endregion

        #region LineTrip
        BO.LineTrip GetLineTrip(int lineId, int stationKey);
        IEnumerable<BO.LineTrip> GetTripsForABus(BO.BusLine line);
        IEnumerable<BO.LineTrip> GetTripsForAStation(BO.BusLine line, BO.BusStation station);
        IEnumerable<BO.LineTrip> GetAllLineTrips();
        void AddLineTrip(BO.LineTrip trip);
        BO.LineTrip AddLineTrip(int lineId, int stationKey);
        void DeleteLineTrip(BO.LineTrip trip);
        TimeSpan CalculateDistance(BO.LineTrip trip);
        #endregion
        
        #region User
        string AddNewUser(string name, string password, bool isAdmin);
        bool UserVerified(string name, string password);
        bool UserAdmin(string name);
        #endregion
    }
}