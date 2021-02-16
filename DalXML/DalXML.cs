using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using APIDL;
using System.Xml.Linq;
using System.Device.Location;

namespace DL
{
	public class DalXML : IDAL
    {
        #region singelton
        static readonly DalXML instance = new DalXML();
        static DalXML() { }// static ctor to ensure instance init is done just before first usage
        DalXML() { } // default => private
        public static DalXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string stationsPath = @"BusStationXml.xml";
        string linesPath = @"BusLineXml.xml";
        string followingStationsPath = @"FollowingStations.xml";
        string lineStationsPath = @"LineStationXml.xml";
        string idPath = @"IdXml.xml";
        string lineTripPath = @"LineTripXml.xml";
        #endregion

        #region BusStation
        public void AddStation(BusStation station)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            XElement myStation = (from s in stationsRootElem.Elements()
                                  where Convert.ToInt32(s.Element("BusStationKey").Value) == station.BusStationKey
                                  select s).FirstOrDefault();

            if (myStation != null)
                throw new ArgumentException("duplicate bus stations");

            //Creation of new XElement
            XElement stationElem = new XElement("BusStation", new XElement("BusStationKey", station.BusStationKey.ToString()),
                                          new XElement("Address", station.Address),
                                          new XElement("StationName", station.StationName),
                                          new XElement("Latitude", station.Latitude.ToString()),
                                          new XElement("Longitude", station.Longitude.ToString()));

            //Adding this XElement to busStations' xml file
            stationsRootElem.Add(stationElem);
            XMLTools.SaveListToXMLElement(stationsRootElem, stationsPath);
        }

        public void DeleteStation(int id)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            XElement myStation = (from s in stationsRootElem.Elements()
                                  where Convert.ToInt32(s.Element("BusStationKey").Value) == id
                                  select s).FirstOrDefault();

            if (myStation == null)
                throw new ArgumentException("It's not exist Bus Station with this key : " + id);

            myStation.Remove();
            XMLTools.SaveListToXMLElement(stationsRootElem, stationsPath);
        }

        public IEnumerable<BusStation> GetAllStations()
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            return from station in stationsRootElem.Elements()
                   let s = new BusStation()
                   {
                       BusStationKey = Convert.ToInt32(station.Element("BusStationKey").Value),
                       Address = station.Element("Address").Value,
                       StationName = station.Element("StationName").Value,
                       Latitude = double.Parse(station.Element("Latitude").Value),
                       Longitude = double.Parse(station.Element("Longitude").Value)
                   }
                   orderby s.BusStationKey
                   select s;
        }

        public IEnumerable<BusStation> GetAllStationsBy(Predicate<BusStation> predicate)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            return from station in stationsRootElem.Elements()
                   let s = new BusStation()
                   {
                       BusStationKey = Convert.ToInt32(station.Element("BusStationKey").Value),
                       Address = station.Element("Address").Value,
                       StationName = station.Element("StationName").Value,
                       Latitude = double.Parse(station.Element("Latitude").Value),
                       Longitude = double.Parse(station.Element("Longitude").Value)
                   }
                   where predicate(s)
                   orderby s.BusStationKey
                   select s;
        }

        public BusStation GetStation(int id)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            BusStation myStation = (from station in stationsRootElem.Elements()
                                    where Convert.ToInt32(station.Element("BusStationKey").Value) == id
                                    select new BusStation()
                                    {
                                        BusStationKey = Convert.ToInt32(station.Element("BusStationKey").Value),
                                        Address = station.Element("Address").Value,
                                        StationName = station.Element("StationName").Value,
                                        Latitude = double.Parse(station.Element("Latitude").Value),
                                        Longitude = double.Parse(station.Element("Longitude").Value)
                                    }).FirstOrDefault();

            if (myStation == null)
                throw new ArgumentException("Bus Station doesn't exist");
            return myStation;
        }
        public void UpdateStation(BusStation station)
        {
            DeleteStation(station.BusStationKey);
            AddStation(station);
        }

        public void UpdateStation(int id, Action<BusStation> update)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            BusStation sta = (from station in stationsRootElem.Elements()
                              where Convert.ToInt32(station.Element("BusStationKey").Value) == id
                              select new BusStation()
                              {
                                  BusStationKey = Convert.ToInt32(station.Element("BusStationKey").Value),
                                  Address = station.Element("Address").Value,
                                  StationName = station.Element("StationName").Value,
                                  Latitude = double.Parse(station.Element("Latitude").Value),
                                  Longitude = double.Parse(station.Element("Longitude").Value)
                              }).FirstOrDefault();
            update(sta);
        }
        #endregion

        #region BusLine
        public void AddLine(BusLine line)
        {
            XElement idRootElem = XMLTools.LoadListFromXMLElement(idPath);
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            XElement myLine = (from l in linesRootElem.Elements()
                               where Convert.ToInt32(l.Element("BusLineNumber").Value) == line.BusLineNumber
                               && l.Element("Area").Value == line.Area.ToString()
                               select l).FirstOrDefault();

            if (myLine != null)
                throw new ArgumentException("Duplicate BusLine");

            //Creation of new XElement
            if (line.Id == 0)
            {
                XElement elementId = (from id in idRootElem.Elements()
                                      where id.Element("Type").Value == "busLine"
                                      select id).FirstOrDefault();
                line.Id = Convert.ToInt32(elementId.Element("Value").Value) + 1;
                
                elementId.Element("Value").Value = (Convert.ToInt32(elementId.Value) + 1).ToString();
                XMLTools.SaveListToXMLElement(idRootElem, idPath);
            }

            XElement lineElem = new XElement("BusLine",
                                          new XElement("Id", line.Id.ToString()),
                                          new XElement("BusLineNumber", line.BusLineNumber.ToString()),
                                          new XElement("Area", line.Area.ToString()),
                                          new XElement("FirstStationKey", line.FirstStationKey.ToString()),
                                          new XElement("LastStationKey", line.LastStationKey.ToString()),
                                          new XElement("TotalTime", line.TotalTime.ToString()));

            //Adding this XElement to BusLine xml file
            linesRootElem.Add(lineElem);
            XMLTools.SaveListToXMLElement(linesRootElem, linesPath);
        }

        public void DeleteLine(BusLine line)
        {

            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            XElement lineToDelete = (from l in linesRootElem.Elements()
                                     where Convert.ToInt32(l.Element("BusLineNumber").Value) == line.BusLineNumber
                                     select l).FirstOrDefault();

            if (lineToDelete == null)
                throw new ArgumentException("This line doesn't exist");

            lineToDelete.Remove();
            XMLTools.SaveListToXMLElement(linesRootElem, linesPath);
        }
        public IEnumerable<BusLine> GetAllLines()
        {
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            return from line in linesRootElem.Elements()
                   let l = new BusLine()
                   {
                       Id = Convert.ToInt32(line.Element("Id").Value),
                       BusLineNumber = Convert.ToInt32(line.Element("BusLineNumber").Value),
                       Area = (Areas)Enum.Parse(typeof(Areas), line.Element("Area").Value),
                       FirstStationKey = Convert.ToInt32(line.Element("FirstStationKey").Value),
                       LastStationKey = Convert.ToInt32(line.Element("LastStationKey").Value),
                       TotalTime = double.Parse(line.Element("TotalTime").Value)
                   }
                   orderby l.BusLineNumber
                   select l;
        }

        public IEnumerable<BusLine> GetAllLinesBy(Predicate<BusLine> predicate)
        {
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            return from line in linesRootElem.Elements()
                   let l = new BusLine()
                   {
                       Id = Convert.ToInt32(line.Element("Id").Value),
                       BusLineNumber = Convert.ToInt32(line.Element("BusLineNumber").Value),
                       Area = (Areas)Enum.Parse(typeof(Areas), line.Element("Area").Value),
                       FirstStationKey = Convert.ToInt32(line.Element("FirstStationKey").Value),
                       LastStationKey = Convert.ToInt32(line.Element("LastStationKey").Value),
                       TotalTime = double.Parse(line.Element("TotalTime").Value)
                   }
                   where predicate(l)
                   orderby l.BusLineNumber
                   select l;
        }
        public BusLine GetLine(int lineNum, Areas area)
        {
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            BusLine lineToReturn = (from line in linesRootElem.Elements()
                                    where Convert.ToInt32(line.Element("BusLineNumber").Value) == lineNum
                                    && (Areas)Enum.Parse(typeof(Areas), line.Element("Area").Value) == area
                                    select new BusLine()
                                    {
                                        Id = Convert.ToInt32(line.Element("Id").Value),
                                        BusLineNumber = Convert.ToInt32(line.Element("BusLineNumber").Value),
                                        Area = (Areas)Enum.Parse(typeof(Areas), line.Element("Area").Value),
                                        FirstStationKey = Convert.ToInt32(line.Element("FirstStationKey").Value),
                                        LastStationKey = Convert.ToInt32(line.Element("LastStationKey").Value),
                                        TotalTime = double.Parse(line.Element("TotalTime").Value)
                                    }).FirstOrDefault();
            if (lineToReturn == null)
                throw new ArgumentException("There is no line with this number and area" + lineNum + area);
            return lineToReturn;
        }
        public BusLine GetLine(int Id)
        {
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            BusLine lineToReturn = (from line in linesRootElem.Elements()
                                    where Convert.ToInt32(line.Element("Id").Value) == Id
                                    select new BusLine()
                                    {
                                        Id = Convert.ToInt32(line.Element("Id").Value),
                                        BusLineNumber = Convert.ToInt32(line.Element("BusLineNumber").Value),
                                        Area = (Areas)Enum.Parse(typeof(Areas), line.Element("Area").Value),
                                        FirstStationKey = Convert.ToInt32(line.Element("FirstStationKey").Value),
                                        LastStationKey = Convert.ToInt32(line.Element("LastStationKey").Value),
                                        TotalTime = double.Parse(line.Element("TotalTime").Value)
                                    }).FirstOrDefault();
            if (lineToReturn == null)
                throw new ArgumentException("There is no line with this id" + Id);
            return lineToReturn;
        }
        public void UpdateLine(BusLine line)
        {
            DeleteLine(line);
            AddLine(line);
        }

        public void UpdateLine(BusLine line, Action<BusLine> update)
        {
            BusLine tempLine = GetLine(line.Id);
            if (tempLine != null)
                update(tempLine);
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            XElement myLineStation = (from ls in lineStationsRootElem.Elements()
                                      where Convert.ToInt32(ls.Element("LineId").Value) == lineStation.LineId
                                        && Convert.ToInt32(ls.Element("StationKey").Value) == lineStation.StationKey
                                      select ls).FirstOrDefault();

            if (myLineStation != null)
                throw new ArgumentException("Duplicate Line Stations");

            //Creation of new XElement
            XElement lineStationElem = new XElement("LineStation",
                                          new XElement("LineId", lineStation.LineId.ToString()),
                                          new XElement("StationKey", lineStation.StationKey.ToString()),
                                          new XElement("RankInLine", lineStation.RankInLine.ToString()));

            //Adding this XElement to busStations' xml file
            lineStationsRootElem.Add(lineStationElem);
            XMLTools.SaveListToXMLElement(lineStationsRootElem, lineStationsPath);
        }

        public void DeleteLineStation(LineStation lineStation)
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            XElement myLineStation = (from ls in lineStationsRootElem.Elements()
                                      where Convert.ToInt32(ls.Element("lineId").Value) == lineStation.LineId
                                        && Convert.ToInt32(ls.Element("StationKey").Value) == lineStation.StationKey
                                      select ls).FirstOrDefault();

            if (myLineStation == null)
                throw new ArgumentException("Line Station doesn't exist");


            myLineStation.Remove();
            XMLTools.SaveListToXMLElement(lineStationsRootElem, lineStationsPath);
        }

        public void DeleteLineStation(Predicate<LineStation> predicate)
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            IEnumerable<XElement> stationToDel = (from lineStat in lineStationsRootElem.Elements()
                                                   let ls = new LineStation()
                                                   {
                                                       LineId = Convert.ToInt32(lineStat.Element("LineId").Value),
                                                       StationKey = Convert.ToInt32(lineStat.Element("StationKey").Value),
                                                       RankInLine = Convert.ToInt32(lineStat.Element("RankInLine").Value)
                                                   }
                                                   where predicate(ls)
                                                   select lineStat).ToList();

            if (stationToDel == null)
                throw new ArgumentException("Line Station doesn't exist");

            foreach (var item in stationToDel)
                item.Remove();

            XMLTools.SaveListToXMLElement(lineStationsRootElem, lineStationsPath);
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            return from lineStat in lineStationsRootElem.Elements()
                   select new LineStation()
                   {
                       LineId = Convert.ToInt32(lineStat.Element("LineId").Value),
                       StationKey = Convert.ToInt32(lineStat.Element("StationKey").Value),
                       RankInLine = Convert.ToInt32(lineStat.Element("RankInLine").Value)
                   };
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            return from lineStat in lineStationsRootElem.Elements()
                   let ls = new LineStation()
                   {
                       LineId = Convert.ToInt32(lineStat.Element("LineId").Value),
                       StationKey = Convert.ToInt32(lineStat.Element("StationKey").Value),
                       RankInLine = Convert.ToInt32(lineStat.Element("RankInLine").Value)
                   }
                   where predicate(ls)
                   select ls;
        }

        public LineStation GetLineStation(int lineId, int stationKey)
        {
            XElement lineStationsRootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);
            return (from lineStat in lineStationsRootElem.Elements()
                    where Convert.ToInt32(lineStat.Element("LineId").Value) == lineId
                         && Convert.ToInt32(lineStat.Element("StationKey").Value) == stationKey
                    select new LineStation()
                    {
                        LineId = Convert.ToInt32(lineStat.Element("LineId").Value),
                        StationKey = Convert.ToInt32(lineStat.Element("StationKey").Value),
                        RankInLine = Convert.ToInt32(lineStat.Element("RankInLine").Value)
                    }).FirstOrDefault();
        }

        public void UpdateLineStation(LineStation station)
        {
            DeleteLineStation(station);
            AddLineStation(station);
        }

        public void UpdateLineStation(LineStation station, Action<LineStation> update)
        {
            LineStation tempStat = GetLineStation(station.LineId, station.StationKey);
            update(tempStat);
        }
        #endregion

        #region FollowingStations
        public void AddFollowingStations(BusStation station1, BusStation station2)
        {
            XElement followingStationsRootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);
            XElement folStat = (from fs in followingStationsRootElem.Elements()
                                where Convert.ToInt32(fs.Element("KeyStation1").Value) == station1.BusStationKey
                                && Convert.ToInt32(fs.Element("KeyStation2").Value) == station2.BusStationKey
                                select fs).FirstOrDefault();

            if (folStat != null)
                throw new ArgumentException("duplicate following stations");

            //calculate distance & time in new field to facility using of them in create new XElement
            double distance = new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                        (new GeoCoordinate(station2.Latitude, station2.Longitude));
            double time = new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                    (new GeoCoordinate(station2.Latitude, station2.Longitude)) * 0.0012 * 0.5;
            //Creation of new XElement
            XElement followStationElem = new XElement("FollowingStations",
                                          new XElement("KeyStation1", station1.BusStationKey.ToString()),
                                          new XElement("KeyStation2", station2.BusStationKey.ToString()),
                                          new XElement("Distance", distance.ToString()),
                                          new XElement("AverageJourneyTime", time.ToString()));

            //Adding this XElement to followingStations' xml file
            followingStationsRootElem.Add(followStationElem);
            XMLTools.SaveListToXMLElement(followingStationsRootElem, followingStationsPath);
        }
        public void DeleteFollowingStations(BusStation station1, BusStation station2)
        {
            XElement followingStationsRootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);
            XElement folStat = (from fs in followingStationsRootElem.Elements()
                                where Convert.ToInt32(fs.Element("KeyStation1").Value) == station1.BusStationKey
                                && Convert.ToInt32(fs.Element("KeyStation2").Value) == station2.BusStationKey
                                select fs).FirstOrDefault();
            if (folStat != null)
            {
                folStat.Remove();
                XMLTools.SaveListToXMLElement(followingStationsRootElem, followingStationsPath);
            }
            else
                throw new ArgumentException("Following stations between " + station1 + " and " + station2 + " doesn't exist");
        }
        public IEnumerable<FollowingStations> GetAllFollowingStations()
        {
            XElement followingStationsRootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);
            return from fs in followingStationsRootElem.Elements()
                   select new FollowingStations()
                   {
                       KeyStation1 = Convert.ToInt32(fs.Element("KeyStation1").Value),
                       KeyStation2 = Convert.ToInt32(fs.Element("KeyStation2").Value),
                       Distance = double.Parse(fs.Element("Distance").Value),
                       AverageJourneyTime = double.Parse(fs.Element("AverageJourneyTime").Value)
                   };
        }

        public IEnumerable<FollowingStations> GetAllFollowingStationsBy(Predicate<FollowingStations> predicate)
        {
            XElement followingStationsRootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);
            return from fs in followingStationsRootElem.Elements()
                   let fs1 = new FollowingStations()
                   {
                       KeyStation1 = Convert.ToInt32(fs.Element("KeyStation1").Value),
                       KeyStation2 = Convert.ToInt32(fs.Element("KeyStation2").Value),
                       Distance = double.Parse(fs.Element("Distance").Value),
                       AverageJourneyTime = double.Parse(fs.Element("AverageJourneyTime").Value)
                   }
                   where predicate(fs1)
                   select fs1;
        }
        public FollowingStations GetFollowingStations(BusStation station1, BusStation station2)
        {
            XElement followingStationsRootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);
			FollowingStations followingStations = (from fs in followingStationsRootElem.Elements()
												   where Convert.ToInt32(fs.Element("KeyStation1").Value) == station1.BusStationKey
												   && Convert.ToInt32(fs.Element("KeyStation2").Value) == station2.BusStationKey
												   select new FollowingStations()
												   {
													   KeyStation1 = Convert.ToInt32(fs.Element("KeyStation1").Value),
													   KeyStation2 = Convert.ToInt32(fs.Element("KeyStation2").Value),
													   Distance = double.Parse(fs.Element("Distance").Value),
													   AverageJourneyTime = double.Parse(fs.Element("AverageJourneyTime").Value)
												   }).FirstOrDefault();
            if (followingStations == null)
			{
				AddFollowingStations(station1, station2);
                followingStations = GetFollowingStations(station1, station2);
            }

			return followingStations;
        }
        public void UpdateFollowingStations(BusStation station1, BusStation station2)
        {
            DeleteFollowingStations(station1, station2);
            AddFollowingStations(station1, station2);
        }

        public void UpdateFollowingStations(BusStation busStation1, BusStation busStation2, Action<FollowingStations> update)
        {

        }
        #endregion

        #region LineTrip
        public LineTrip GetLineTrip(int tripId)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

			LineTrip myLineTrip = (from lineTrip in lineTripRootElem.Elements()
                                   where Convert.ToInt32(lineTrip.Element("tripId").Value) == tripId
                                   select new LineTrip()
                                   {
                                       TripId = Convert.ToInt32(lineTrip.Element("tripId").Value),
                                       LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
                                       LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
                                       StationKey = Convert.ToInt32(lineTrip.Element("StationKey").Value),
                                       Departure = Convert.ToDateTime(lineTrip.Element("Departure").Value),
                                       Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
                                       Destination = lineTrip.Element("Destination").Value
                                   }).FirstOrDefault();

            if (myLineTrip == null)
                throw new ArgumentException("LineTrip doesn't exist");

            return myLineTrip;
        }
        public LineTrip GetLineTrip(int lineId, int stationKey)
        {
            XElement idRootElem = XMLTools.LoadListFromXMLElement(idPath);
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            LineTrip myNewLineTrip = (from lineTrip in lineTripRootElem.Elements()
                                      where Convert.ToInt32(lineTrip.Element("LineIdTrip").Value) == lineId //&& lineTrip.Element("Destination").Value
                                      select new LineTrip()
                                      {
                                          TripId = (Convert.ToInt32(lineTrip.Element("tripId").Value)),
                                          LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
                                          LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
                                          StationKey = stationKey,
                                          Departure = Convert.ToDateTime(lineTrip.Element("Departure").Value),
                                          Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
                                          Destination = lineTrip.Element("Destination").Value
                                      }).FirstOrDefault();

            if (myNewLineTrip == null)
            {
                myNewLineTrip = AddLineTrip(lineId, stationKey);
            }
            
            myNewLineTrip.Departure.Add(CalculateDistance(myNewLineTrip));
            return myNewLineTrip;
        }
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            return (from lineTrip in lineTripRootElem.Elements()
                    select new LineTrip()
                    {
                        TripId = Convert.ToInt32(lineTrip.Element("tripId").Value),
                        LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
                        LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
                        StationKey = Convert.ToInt32(lineTrip.Element("StationKey").Value),
                        Departure = Convert.ToDateTime(lineTrip.Element("Departure").Value),
                        Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
                        Destination = lineTrip.Element("Destination").Value
                    }).ToList();
        }
        public void AddLineTrip(LineTrip trip)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            LineTrip myLineTrip = (from lineTrip in lineTripRootElem.Elements()
                                   where Convert.ToInt32(lineTrip.Element("Id").Value) == trip.TripId
                                   select new LineTrip()
                                   {
                                       TripId = Convert.ToInt32(lineTrip.Element("tripId").Value),
                                       LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
                                       LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
                                       StationKey = Convert.ToInt32(lineTrip.Element("StationKey").Value),
                                       Departure = Convert.ToDateTime(lineTrip.Element("Departure").Value),
                                       Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
                                       Destination = lineTrip.Element("Destination").Value
                                   }).FirstOrDefault();

            if (myLineTrip != null)
                throw new ArgumentException("Duplicate trip");

            XElement lineTripElem = new XElement("LineTrip",
                                          new XElement("tripId", trip.TripId.ToString()),
                                          new XElement("LineNumber", trip.LineNumber.ToString()),
                                          new XElement("LineIdTrip", trip.LineIdTrip.ToString()),
                                          new XElement("StationKey", trip.StationKey.ToString()),
                                          new XElement("Departure", trip.Departure.ToString()),
                                          new XElement("Frequency", trip.Frequency.ToString()),
                                          new XElement("Destination", trip.Destination.ToString()));

            lineTripRootElem.Add(lineTripElem);
            XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath); ;

            lineTripRootElem.Add(lineTripElem);
        }
        public LineTrip AddLineTrip(int lineId, int stationKey)
		{
            XElement idRootElem = XMLTools.LoadListFromXMLElement(idPath);
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            XElement elementId = (from id in idRootElem.Elements()
                                  where id.Element("Type").Value == "lineTrip"
                                  select id).FirstOrDefault();
            
            XElement newLineTripElem = new XElement("LineTrip",
                                          new XElement("tripId", Convert.ToInt32(elementId.Element("Value").Value) + 1),
                                          new XElement("LineNumber", GetLine(lineId).BusLineNumber),
                                          new XElement("LineIdTrip", lineId.ToString()),
                                          new XElement("StationKey", stationKey.ToString()),
                                          new XElement("Departure", "5:00"),
                                          new XElement("Frequency", "120"),
                                          new XElement("Destination", GetStation(GetLine(lineId).LastStationKey).Address));

            elementId.Element("Value").Value = (Convert.ToInt32(elementId.Element("Value").Value) + 1).ToString();
            XMLTools.SaveListToXMLElement(idRootElem, idPath);

            return new LineTrip()
			{
				TripId = Convert.ToInt32(newLineTripElem.Element("tripId").Value),
				LineNumber = GetLine(lineId).BusLineNumber,
                LineIdTrip = lineId,
                StationKey = stationKey,
                Departure = Convert.ToDateTime(newLineTripElem.Element("Departure").Value),
                Frequency = 120,
                Destination = GetStation(GetLine(lineId).LastStationKey).Address
            };
		}
        public void DeleteLineTrip(LineTrip trip)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            XElement myLineTrip = (from t in lineTripRootElem.Elements()
                                   where Convert.ToInt32(t.Element("tripId").Value) == trip.TripId
                                   select t).FirstOrDefault();

            if (myLineTrip == null)
                throw new ArgumentException("Line Trip doesn't exist");


            myLineTrip.Remove();
            XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);
        }
        public TimeSpan CalculateDistance(LineTrip trip)
        {
            double totalDist = 0;
            int ID = GetLine(trip.LineIdTrip).Id;
            BusLine line = GetLine(trip.LineIdTrip);
            LineStation stop1, stop2 = GetLineStation(ID, line.FirstStationKey);

            for (int i = 2; i <= GetLineStation(ID, trip.StationKey).RankInLine; i++)
            {
                stop1 = stop2;
                stop2 = GetLineStation(ID, GetAllLineStationsBy(s => s.LineId == ID && s.RankInLine == i).First().StationKey);
                totalDist += GetFollowingStations(GetStation(stop1.StationKey), GetStation(stop2.StationKey)).AverageJourneyTime;
            }
            return TimeSpan.FromMinutes(totalDist);
        }
        #endregion
    }
}
