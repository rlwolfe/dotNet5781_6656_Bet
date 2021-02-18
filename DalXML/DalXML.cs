using APIDL;
using DO;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Xml.Linq;

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
		string UsersPath = @"UserPassXml.xml";
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
		/// <summary>
		/// get methods for BusStations
		/// get all, get all based on a linq request
		/// and get specific
		/// </summary>
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

				elementId.Element("Value").Value = (Convert.ToInt32(elementId.Element("Value").Value) + 1).ToString();
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
		/// <summary>
		/// get methods for BusLines
		/// get all, get all based on a linq request
		/// and get specific based off internal id of line
		/// or the bus's line number and area
		/// </summary>
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
										  new XElement("RankInLine", GetAllLineStationsBy(ls =>
																		ls.LineId == lineStation.LineId).Count().ToString()));

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

			List<LineStation> myLineStation = (from lineStat in lineStationsRootElem.Elements()
											   let ls = new LineStation()
											   {
												   LineId = Convert.ToInt32(lineStat.Element("LineId").Value),
												   StationKey = Convert.ToInt32(lineStat.Element("StationKey").Value),
												   RankInLine = Convert.ToInt32(lineStat.Element("RankInLine").Value)
											   }
											   where predicate(ls)
											   select ls).ToList();
			//if (myLineStation == null)

			return myLineStation;
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
			UpdateFollowingStations(busStation1, busStation2);
		}
		#endregion

		#region LineTrip
		/// <summary>
		/// gets for the LineTrips
		/// can be sent a trip or the info of the station and lineId
		/// and one get for all trip in file
		/// </summary>
		/// <param name="tripId"></param>
		/// <returns>requested trip/s</returns>
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
									  where Convert.ToInt32(lineTrip.Element("LineIdTrip").Value) == lineId
									  && Convert.ToInt32(lineTrip.Element("StationKey").Value) == stationKey
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
				LineTrip lt = new LineTrip
				{
					LineIdTrip = lineId,
					StationKey = stationKey,
					Destination = GetStation(GetLine(lineId).LastStationKey).Address,
					LineNumber = GetLine(lineId).BusLineNumber
				};

				if (GetLine(lineId).FirstStationKey == stationKey)
				{
					Random rnd = new Random();
					XElement elementId = (from id in idRootElem.Elements()
										  where id.Element("Type").Value == "lineTrip"
										  select id).FirstOrDefault();

					lt.TripId = Convert.ToInt32(elementId.Element("Value").Value) + 1;
					lt.Departure = new DateTime(2020, 1, 1, rnd.Next(5,8), rnd.Next(60), 0);
					lt.Frequency = rnd.Next(2, 121);

					elementId.Element("Value").Value = (Convert.ToInt32(elementId.Element("Value").Value) + 1).ToString();
					XMLTools.SaveListToXMLElement(idRootElem, idPath);
					
					AddLineTrip(lt);
					myNewLineTrip = lt;
				}

				else
				{
					LineTrip first = GetLineTrip(lineId, GetLine(lineId).FirstStationKey);
					
					lt.TripId = first.TripId;
					lt.Frequency = first.Frequency;
					lt.Departure = first.Departure;
					
					AddLineTrip(lt, false);
					myNewLineTrip = lt;
				}
			}
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
						Departure = Convert.ToDateTime((Convert.ToDateTime(lineTrip.Element("Departure").Value).ToShortTimeString())),
						Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
						Destination = lineTrip.Element("Destination").Value
					}).ToList();
		}
		/// <summary>
		/// adds for the LineTrip
		/// each method checks if the trip already exists, if it doesn't it creates it
		/// then it checks if the given trip is from the first station of the bus,
		/// if so, it continues through the bus route and creates trips for every other station
		/// </summary>
		/// <param>trip/trip+if station is first/lineId+ station+ time bus gets to station</param>
		public void AddLineTrip(LineTrip trip, bool firstOnLine)
		{
			if (!firstOnLine)
			{
				double travelTime = 0;
				for (int rank = 2; rank < GetAllLineStationsBy(ls => ls.LineId == trip.LineIdTrip).Count(); rank++)
				{
					BusStation stop1 = GetStation(GetAllLineStationsBy(ls => ls.RankInLine == rank - 1 &&
															ls.LineId == trip.LineIdTrip).FirstOrDefault().StationKey);

					BusStation stop2 = GetStation(GetAllLineStationsBy(ls => ls.RankInLine == rank &&
															ls.LineId == trip.LineIdTrip).FirstOrDefault().StationKey);

					travelTime += GetFollowingStations(stop1, stop2).AverageJourneyTime;
				
					AddLineTrip(trip.LineIdTrip, stop2.BusStationKey, trip.Departure.AddMinutes(travelTime));
				}
			}
			else
			{
				AddLineTrip(trip);
			}
		}
		public void AddLineTrip(LineTrip trip)
		{
			XElement idRootElem = XMLTools.LoadListFromXMLElement(idPath);
			XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

			XElement elementId = (from id in idRootElem.Elements()
								  where id.Element("Type").Value == "lineTrip"
								  select id).FirstOrDefault();

			LineTrip myLineTrip = (from lineTrip in lineTripRootElem.Elements()
								   where Convert.ToInt32(lineTrip.Element("tripId").Value) == trip.TripId
								   select new LineTrip()
								   {
									   TripId = Convert.ToInt32(lineTrip.Element("tripId").Value),
									   LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
									   LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
									   StationKey = Convert.ToInt32(lineTrip.Element("StationKey").Value),
									   Departure = Convert.ToDateTime(Convert.ToDateTime(lineTrip.Element("Departure").Value).ToShortTimeString()),
									   Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
									   Destination = lineTrip.Element("Destination").Value
								   }).FirstOrDefault();

			if (myLineTrip != null)
				return;

			XElement lineTripElem = new XElement("LineTrip",
										  new XElement("tripId", trip.TripId.ToString()),
										  new XElement("LineNumber", trip.LineNumber.ToString()),
										  new XElement("LineIdTrip", trip.LineIdTrip.ToString()),
										  new XElement("StationKey", trip.StationKey.ToString()),
										  new XElement("Departure", trip.Departure.ToShortTimeString()),
										  new XElement("Frequency", trip.Frequency.ToString()),
										  new XElement("Destination", trip.Destination.ToString()));
			if (trip.TripId == 0)
				lineTripElem.SetElementValue("tripId", Convert.ToInt32(elementId.Element("Value").Value) + 1);

			elementId.Element("Value").Value = (Convert.ToInt32(elementId.Element("Value").Value) + 1).ToString();
			XMLTools.SaveListToXMLElement(idRootElem, idPath);

			lineTripRootElem.Add(lineTripElem);
			XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);

			if (trip.StationKey == GetLine(trip.LineIdTrip).FirstStationKey)
			{
				AddLineTrip(trip, true);
			}
		}
		public LineTrip AddLineTrip(int lineId, int stationKey, DateTime time)
		{
			XElement idRootElem = XMLTools.LoadListFromXMLElement(idPath);
			XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

			XElement elementId = (from id in idRootElem.Elements()
								  where id.Element("Type").Value == "lineTrip"
								  select id).FirstOrDefault();

			LineTrip myLineTrip = (from lineTrip in lineTripRootElem.Elements()
								   where Convert.ToInt32(lineTrip.Element("LineIdTrip").Value) == lineId
								   && Convert.ToInt32(lineTrip.Element("StationKey").Value) == stationKey
								   select new LineTrip()
								   {
									   TripId = Convert.ToInt32(lineTrip.Element("tripId").Value),
									   LineNumber = Convert.ToInt32(lineTrip.Element("LineNumber").Value),
									   LineIdTrip = Convert.ToInt32(lineTrip.Element("LineIdTrip").Value),
									   StationKey = Convert.ToInt32(lineTrip.Element("StationKey").Value),
									   Departure = Convert.ToDateTime(Convert.ToDateTime(lineTrip.Element("Departure").Value).ToShortTimeString()),
									   Frequency = Convert.ToInt32(lineTrip.Element("Frequency").Value),
									   Destination = lineTrip.Element("Destination").Value
								   }).FirstOrDefault();

			XElement newLineTripElem = new XElement("LineTrip",
										  new XElement("tripId", Convert.ToInt32(elementId.Element("Value").Value) + 1),
										  new XElement("LineNumber", GetLine(lineId).BusLineNumber),
										  new XElement("LineIdTrip", lineId.ToString()),
										  new XElement("StationKey", stationKey.ToString()),
										  new XElement("Departure", time.ToShortTimeString()),
										  new XElement("Frequency", "120"),
										  new XElement("Destination", GetStation(GetLine(lineId).LastStationKey).Address));

			elementId.Element("Value").Value = (Convert.ToInt32(elementId.Element("Value").Value) + 1).ToString();
			XMLTools.SaveListToXMLElement(idRootElem, idPath);

			lineTripRootElem.Add(newLineTripElem);
			XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);

			if (stationKey == GetLine(lineId).FirstStationKey)
			{
				foreach (LineStation station in GetAllLineStationsBy(ls => ls.LineId == lineId))
				{
					if (station.StationKey != stationKey)
						AddLineTrip(lineId, station.StationKey, time);
				}
			}

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
		/// <summary>
		/// calculates the time it takes from one busStop to the next
		/// so the arrival times of each station are accurate based off the first station's departure time
		/// </summary>
		/// <param name="LineTrip"></param>
		/// <returns>the time calculated between bus stations</returns>
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

		#region User
		/// <summary>
		/// gets username, password and if the user is an employee or passenger
		/// checks if the username is already taken, if not creates a new user
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <returns>if the user was created or an error is not</returns>
		public string AddNewUser(string name, string password, bool isAdmin)
		{
			XElement UsersRootElem = XMLTools.LoadListFromXMLElement(UsersPath);
			XElement user = (from u in UsersRootElem.Elements()
							 where u.Element("Name").Value == name
							 select u).FirstOrDefault();

			if (user != null)
				return "שם משתמש כבר קיים";

			XElement userElem = new XElement("User",
								new XElement("Type", isAdmin ? "Employee" : "Passenger"),
								new XElement("Name", name),
								new XElement("Password", password));

			UsersRootElem.Add(userElem);
			XMLTools.SaveListToXMLElement(UsersRootElem, UsersPath);

			return "המשתמש הוסף בהצלחה";
		}
		/// <summary>
		/// checks if user is in the system and password matches
		/// </summary>
		/// <param name="name"></param>
		/// <returns>if username and password match returns true</returns>
		public bool UserVerified(string name, string password)
		{
			XElement UsersRootElem = XMLTools.LoadListFromXMLElement(UsersPath);
			User user = (from u in UsersRootElem.Elements()
						 where u.Element("Name").Value == name && u.Element("Password").Value == password
						 select new User()
						 {
							 UserName = u.Element("Name").Value,
							 Password = u.Element("Password").Value,
							 UserStatus = (UserStatus)Enum.Parse(typeof(UserStatus), u.Element("Type").Value)
						 }).FirstOrDefault();

			if (user == null)
				return false;

			return true;
		}
		/// <summary>
		/// checks if the user is an employee or a passenger, and accordingly allows them to only go to the part of system they are supposed to use
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool UserAdmin(string name)
		{
			XElement UsersRootElem = XMLTools.LoadListFromXMLElement(UsersPath);
			User tempUser = (from u in UsersRootElem.Elements()
							 where u.Element("Name").Value == name
							 select new User()
							 {
								 UserName = u.Element("Name").Value,
								 Password = u.Element("Password").Value,
								 UserStatus = (UserStatus)Enum.Parse(typeof(UserStatus), u.Element("Type").Value)
							 }).FirstOrDefault();

			return tempUser.UserStatus == (UserStatus)Enum.Parse(typeof(UserStatus), "Employee");
		}
		#endregion
	}
}
