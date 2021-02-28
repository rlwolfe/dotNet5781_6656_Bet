using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DO;
using APIDL;
using System.Device.Location;


namespace DS
{
	public static class DataSource
	{

		public static List<BusStation> ListStations;
		public static List<BusLine> ListLines;
		public static List<LineStation> ListLineStations;
		public static List<FollowingStations> ListFollowingStations;
		public static List<LineTrip> ListLineTrips;
		public static int TripId = 1;
		public static int LineId = 1;
		public static int LineStationId = 1;
		public static int FollowingStationsId = 1;

		static DataSource()
		{
			InitAllLists();
		}

		static void InitAllLists()
		{
			ListStations = new List<BusStation>
			{
				new BusStation
				{
					BusStationKey = 38831,
					StationName =" בי''ס בר לב / בן יהודה",
					Address =" רחוב:בן יהודה 76 עיר: כפר סבא רציף: קומה",
					Latitude = 32.183921,
					Longitude = 34.917806,
				},
				new BusStation
				{
					BusStationKey = 38894,
					StationName ="פיינברג/שכביץ",
					Address =" רחוב:פיינברג 4 עיר: גדרה רציף:   קומה",
					Latitude = 31.813285,
					Longitude = 34.775928,
				},
				new BusStation
				{
					BusStationKey = 38903,
					StationName ="קרוננברג/ארגמן",
					Address ="רחוב:יוסף קרוננברג  עיר: רחובות רציף:   קומה",
					Latitude = 31.878667,
					Longitude = 34.81138,
				},
				new BusStation
				{
					BusStationKey = 38912,
					StationName ="השומר/האבות",
					Address ="רחוב:השומר 22 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.959821,
					Longitude = 34.814747,
				},
				new BusStation
				{
					BusStationKey = 38916,
					StationName ="יוסף בורג/משואות יצחק",
					Address ="רחוב:ד''ר יוסף בורג 9 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.968049,
					Longitude = 34.818099,
				},
				new BusStation
				{
					BusStationKey = 38922,
					StationName ="השר חיים שפירא/הרב שלום ג'רופי",
					Address ="רחוב:השר חיים משה שפירא 16 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.990757,
					Longitude = 34.755683,
				},
				new BusStation
				{
					BusStationKey = 39001,
					StationName ="שדרות יעקב/יוסף הנשיא",
					Address ="רחוב:שדרות יעקב 65 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.950254,
					Longitude = 34.819244 ,
				},
				new BusStation
				{
					BusStationKey = 39002,
					StationName ="שדרות יעקב/עזרא",
					Address ="רחוב:שדרות יעקב 59 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.95111,
					Longitude = 34.819766 ,
				},
				new BusStation
				{
					BusStationKey = 39004,
					StationName ="לייב יוספזון/יעקב ברמן",
					Address ="רחוב:יהודה לייב יוספזון  עיר: רחובות רציף:   קומה",
					Latitude = 31.905052,
					Longitude = 34.818909,
				},
				new BusStation
				{
					BusStationKey = 39005,
					StationName ="הרב יעקב ברמן/הרב יהודה צבי מלצר",
					Address =" רחוב:הרב יעקב ברמן 4 עיר: רחובות רציף:   קומה",
					Latitude = 31.901879,
					Longitude = 34.819443,
				},
				new BusStation
				{
					BusStationKey = 39006,
					StationName ="ברמן/מלצר",
					Address ="רחוב:הרב יעקב ברמן  עיר: רחובות רציף:   קומה",
					Latitude = 31.90281,
					Longitude = 34.818922,
				},
				new BusStation
				{
					BusStationKey = 39007,
					StationName ="הנשיא הראשון/מכון ויצמן",
					Address ="רחוב:הנשיא הראשון 55 עיר: רחובות רציף:   קומה",
					Latitude = 31.904567,
					Longitude = 34.815296,
				},
				new BusStation
				{
					BusStationKey = 39008,
					StationName ="הנשיא הראשון/קיפניס",
					Address ="רחוב:הנשיא הראשון 56 עיר: רחובות רציף:   קומה",
					Latitude = 31.904755,
					Longitude = 34.816661,
				},
				new BusStation
				{
					BusStationKey = 39012,
					StationName ="הירדן/הערבה",
					Address ="רחוב:הירדן 23 עיר: באר יעקב רציף:   קומה",
					Latitude = 31.937387,
					Longitude = 34.838609,
				},
				new BusStation
				{
					BusStationKey = 39013,
					StationName ="הירדן/חרוד",
					Address ="רחוב:הירדן 22 עיר: באר יעקב רציף:   קומה",
					Latitude = 31.936925,
					Longitude = 34.838341,
				},
				new BusStation
				{
					BusStationKey = 39014,
					StationName ="האלונים/הדקל",
					Address ="רחוב:שדרות האלונים  עיר: באר יעקב רציף:   קומה",
					Latitude = 31.939037,
					Longitude = 34.831964,
				},
				new BusStation
				{
					BusStationKey = 39017,
					StationName ="האלונים א/הדקל",
					Address ="רחוב:שדרות האלונים  עיר: באר יעקב רציף:   קומה",
					Latitude = 31.939656,
					Longitude = 34.832104,
				},
				new BusStation
				{
					BusStationKey = 39018,
					StationName ="פארק תעשיות שילת",
					Address ="רחוב:דרך הזית  עיר: שילת רציף:   קומה",
					Latitude = 31.914324,
					Longitude = 35.023589,
				},
				new BusStation
				{
					BusStationKey = 39019,
					StationName ="פארק תעשיות שילת",
					Address ="רחוב:דרך הזית  עיר: שילת רציף:   קומה",
					Latitude = 31.914816,
					Longitude = 35.023028,

				},
				new BusStation
				{
					BusStationKey = 39024,
					StationName ="עיריית מודיעין מכבים רעות",
					Address ="רחוב:  עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.908499,
					Longitude = 35.007955,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				},
				new BusStation
				{
					BusStationKey = 39040,
					Address = " רחוב:רבאט  עיר: רמלה רציף:   קומה:  ",
					StationName = "גן חק''ל/רבאט",
					Latitude = 31.931204,
					Longitude = 34.884956
				},
				new BusStation
				{
					BusStationKey = 39041,
					Address = " רחוב:דוכיפת  עיר: רמלה רציף:   קומה:  ",
					StationName = "קניון צ. רמלה לוד/דוכיפת",
					Latitude = 31.933379,
					Longitude = 34.887207
				},
				new BusStation
				{
					BusStationKey = 39042,
					Address = " רחוב:היצירה 2 עיר: רמלה רציף:   קומה:  ",
					StationName = "היצירה/התקווה",
					Latitude = 31.929318,
					Longitude = 34.880069
				},
				new BusStation
				{
					BusStationKey = 39043,
					Address = " רחוב:היצירה  עיר: רמלה רציף:   קומה:  ",
					StationName = "היצירה/התקווה",
					Latitude = 31.929199,
					Longitude = 34.879993
				},
				new BusStation
				{
					BusStationKey = 39044,
					Address = " רחוב:עמל  עיר: רמלה רציף:   קומה:  ",
					StationName = "עמל/היצירה",
					Latitude = 31.932402,
					Longitude = 34.881442
				},
				new BusStation
				{
					BusStationKey = 39049,
					Address = " רחוב:ישראל פרנקל 10 עיר: רמלה רציף:   קומה:  ",
					StationName = "פרנקל/ויתקין",
					Latitude = 31.936159,
					Longitude = 34.864906
				},
				new BusStation
				{
					BusStationKey = 39050,
					Address = " רחוב:ישראל פרנקל 11 עיר: רמלה רציף:   קומה:  ",
					StationName = "פרנקל/ויתקין",
					Latitude = 31.936022,
					Longitude = 34.86495
				},
				new BusStation
				{
					BusStationKey = 39051,
					Address = " רחוב:ישראל פרנקל 17 עיר: רמלה רציף:   קומה:  ",
					StationName = "ישראל פרנקל/דוב הוז",
					Latitude = 31.935488,
					Longitude = 34.863972
				},
				new BusStation
				{
					BusStationKey = 39052,
					Address = " רחוב:גיורא יוספטל 6 עיר: רמלה רציף:   קומה:  ",
					StationName = "יוספטל/הדס",
					Latitude = 31.936109,
					Longitude = 34.857638
				},
				new BusStation
				{
					BusStationKey = 39056,
					Address = " רחוב:שמחה הולצברג  עיר: רמלה רציף:   קומה:  ",
					StationName = "אהרון בוגנים/משה שרת",
					Latitude = 31.933413,
					Longitude = 34.853906
				},
				new BusStation
				{
					BusStationKey = 39057,
					Address = " רחוב:שמחה הולצברג 10 עיר: רמלה רציף:   קומה:  ",
					StationName = "גרשון ש''ץ/שמחה הולצברג",
					Latitude = 31.932532,
					Longitude = 34.853223
				},
				new BusStation
				{
					BusStationKey = 39058,
					Address = " רחוב:שמחה הולצברג 4 עיר: רמלה רציף:   קומה:  ",
					StationName = "הולצברג/שץ",
					Latitude = 31.93166,
					Longitude = 34.853149
				},
				new BusStation
				{
					BusStationKey = 39059,
					Address = " רחוב:לוי אשכול 11 עיר: רמלה רציף:   קומה:  ",
					StationName = "אשכול/הרב שפירא",
					Latitude = 31.929827,
					Longitude = 34.857194
				},
				new BusStation
				{
					BusStationKey = 39060,
					Address = " רחוב:יהודה שטיין  עיר: רמלה רציף:   קומה:  ",
					StationName = "יהודה שטיין/קרן היסוד",
					Latitude = 31.926545,
					Longitude = 34.855866
				},
				new BusStation
				{
					BusStationKey = 39066,
					Address = " רחוב:שמשון הגיבור  עיר: רמלה רציף:   קומה:  ",
					StationName = "שמשון הגיבור/המסגד",
					Latitude = 31.926441,
					Longitude = 34.866014
				},
				new BusStation
				{
					BusStationKey = 39068,
					Address = " רחוב:ח.נ. ביאליק 19 עיר: רמלה רציף:   קומה:",
					StationName = "ביאליק/חניתה",
					Latitude = 31.924484,
					Longitude = 34.870366
				},
				new BusStation
				{
					BusStationKey = 39069,
					Address = " רחוב:ח.נ. ביאליק 43 עיר: רמלה רציף:   קומה:",
					StationName = "ביאליק/ירמיהו",
					Latitude = 31.92055,
					Longitude = 34.868205
				},
				new BusStation
				{
					BusStationKey = 39070,
					Address = " רחוב:א.ס. לוי 1 עיר: רמלה רציף:   קומה:  ",
					StationName = "א.ס לוי/סעדיה מרדכי",
					Latitude = 31.9209,
					Longitude = 34.861221
				},
				new BusStation
				{
					BusStationKey = 39071,
					Address = " רחוב:שדרות דוד רזיאל 5 עיר: רמלה רציף:   קומה:  ",
					StationName = "רזיאל/זכריה",
					Latitude = 31.923666,
					Longitude = 34.862813
				},
				new BusStation
				{
					BusStationKey = 39072,
					Address = " רחוב:חרוד  עיר: ישרש רציף:   קומה:  ",
					StationName = "חרוד א",
					Latitude = 31.912572,
					Longitude = 34.850134
				},
				new BusStation
				{
					BusStationKey = 39073,
					Address = " רחוב:חרוד  עיר: ישרש רציף:   קומה:  ",
					StationName = "חרוד/הירדן",
					Latitude = 31.915977,
					Longitude = 34.848217
				},
				new BusStation
				{
					BusStationKey = 39075,
					Address = " רחוב:הירדן  עיר: ישרש רציף:   קומה:  ",
					StationName = "הירדן/דן",
					Latitude = 31.915489,
					Longitude = 34.852139
				},
				new BusStation
				{
					BusStationKey = 39076,
					Address = " רחוב:עוזי חיטמן 44 עיר: רמלה רציף:   קומה:  ",
					StationName = "עוזי חיטמן/שושנה דמארי",
					Latitude = 31.917022,
					Longitude = 34.868261
				},
				new BusStation
				{
					BusStationKey = 39091,
					Address = "רחוב:דרך החרוב  עיר: שילת רציף:   קומה",
					StationName = "החרוב א",
					Latitude = 31.919207,
					Longitude = 35.0234
				},
				new BusStation
				{
					BusStationKey = 39092,
					Address = "רחוב:  עיר: כפר רות רציף:   קומה",
					StationName ="כפר רות",
					Latitude = 31.910544,
					Longitude = 35.034349
				},
				new BusStation
				{
					BusStationKey = 39093,
					Address = "רחוב:  עיר: כפר רות רציף:   קומה",
					StationName = "כפר רות",
					Latitude = 31.910485,
					Longitude = 35.034441
				}

			};

			ListLines = new List<BusLine>
			{
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 12,
					Area = Areas.Center,
					FirstStationKey = 38831,
					LastStationKey = 39007,
					TotalTime = 20
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 30,
					Area = Areas.Jerusalem,
					FirstStationKey = 38894,
					LastStationKey = 39024,
					TotalTime = 15
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 50,
					Area = Areas.North,
					FirstStationKey = 38903,
					LastStationKey = 39024,
					TotalTime = 30
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 113,
					Area = Areas.Center,
					FirstStationKey = 38831,
					LastStationKey = 39019,
					TotalTime = 40
				}
			};

			ListLineStations = new List<LineStation>
			{
				new LineStation
				{
					LineId = 1,
					StationKey = 38831,
					RankInLine = 1
				},
				new LineStation
				{
					LineId = 1,
					StationKey = 38894,
					RankInLine = 2
				},
				new LineStation
				{
					LineId = 1,
					StationKey = 39002,
					RankInLine = 3
				},
				new LineStation
				{
					LineId = 1,
					StationKey = 39006,
					RankInLine = 4
				},
				new LineStation
				{
					LineId = 1,
					StationKey = 39007,
					RankInLine = 5
				},

				new LineStation
				{

					LineId = 2,
					StationKey = 38894,
					RankInLine = 1
				},
				new LineStation
				{

					LineId = 2,
					StationKey = 39002,
					RankInLine = 2
				},
				new LineStation
				{

					LineId = 2,
					StationKey = 39006,
					RankInLine = 3
				},
				new LineStation
				{

					LineId = 2,
					StationKey = 39024,
					RankInLine = 4
				},

				new LineStation
				{

					LineId = 3,
					StationKey = 38903,
					RankInLine = 1
				},
				new LineStation
				{

					LineId = 3,
					StationKey = 39002,
					RankInLine = 2
				},
				new LineStation
				{

					LineId = 3,
					StationKey = 39024,
					RankInLine = 3
				},

				new LineStation
				{

					LineId = 4,
					StationKey = 38831,
					RankInLine = 1
				},
				new LineStation
				{

					LineId = 4,
					StationKey = 39004,
					RankInLine = 2
				},
				new LineStation
				{

					LineId = 4,
					StationKey = 39019,
					RankInLine = 3
				}
			};

			ListFollowingStations = new List<FollowingStations>
			{
				new FollowingStations
				{

					KeyStation1 = 38831,
					KeyStation2 = 38894,
					Distance = new GeoCoordinate(32.183921, 34.917806).GetDistanceTo(new GeoCoordinate(31.813285, 34.775928)),
					AverageJourneyTime =
						new GeoCoordinate(32.183921, 34.917806).GetDistanceTo(new GeoCoordinate(31.813285, 34.775928))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 38894,
					KeyStation2 = 39002,
					Distance = new GeoCoordinate(31.813285, 34.775928).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766)),
					AverageJourneyTime =
						new GeoCoordinate(31.813285, 34.775928).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39002,
					KeyStation2 = 39006,
					Distance = new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.90281, 34.818922)),
					AverageJourneyTime =
						new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.90281, 34.818922))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39006,
					KeyStation2 = 39007,
					Distance = new GeoCoordinate(31.90281, 34.818922).GetDistanceTo(new GeoCoordinate(31.904567, 34.815296)),
					AverageJourneyTime =
						new GeoCoordinate(31.90281, 34.818922).GetDistanceTo(new GeoCoordinate(31.904567, 34.815296))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 38894,
					KeyStation2 = 39002,
					Distance = new GeoCoordinate(31.813285, 34.775928).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766)),
					AverageJourneyTime =
						new GeoCoordinate(31.813285, 34.775928).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39002,
					KeyStation2 = 39006,
					Distance = new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.90281, 34.818922)),
					AverageJourneyTime =
						new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.90281, 34.818922))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39006,
					KeyStation2 = 39024,
					Distance = new GeoCoordinate(31.90281, 34.818922).GetDistanceTo(new GeoCoordinate(31.908499, 35.007955)),
					AverageJourneyTime =
						new GeoCoordinate(31.90281, 34.818922).GetDistanceTo(new GeoCoordinate(31.908499, 35.007955))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 38903,
					KeyStation2 = 39002,
					Distance = new GeoCoordinate(31.878667, 31.878667).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766)),
					AverageJourneyTime =
						new GeoCoordinate(31.878667, 31.878667).GetDistanceTo(new GeoCoordinate(31.95111, 34.819766))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39002,
					KeyStation2 = 39024,
					Distance = new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.908499, 35.007955)),
					AverageJourneyTime =
						new GeoCoordinate(31.95111, 34.819766).GetDistanceTo(new GeoCoordinate(31.908499, 35.007955))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 38831,
					KeyStation2 = 39004,
					Distance = new GeoCoordinate(32.183921, 34.917806).GetDistanceTo(new GeoCoordinate(31.905052, 34.818909)),
					AverageJourneyTime =
						new GeoCoordinate(32.183921, 34.917806).GetDistanceTo(new GeoCoordinate(31.905052, 34.818909))*0.0012*0.5,
				},
				new FollowingStations
				{

					KeyStation1 = 39004,
					KeyStation2 = 39019,
					Distance = new GeoCoordinate(31.905052, 34.818909).GetDistanceTo(new GeoCoordinate(31.914816, 35.023028)),
					AverageJourneyTime =
						new GeoCoordinate(31.905052, 34.818909).GetDistanceTo(new GeoCoordinate(31.914816, 35.023028))*0.0012*0.5,
				}
			};

			ListLineTrips = new List<LineTrip>
            {
                new LineTrip
                {
                    TripId = 1,
                    LineNumber = 12,
                    LineIdTrip = 1,
                    StationKey = 38831,
                    Departure = new DateTime(2020, 1, 1, 6, 0, 0),
                    Frequency = 10,
                    Destination = "הנשיא הראשון/מכון ויצמן"
                },
                new LineTrip
                {
                    TripId = 2,
                    LineNumber = 30,
                    LineIdTrip = 2,
                    StationKey = 38894,
                    Departure = new DateTime(2020, 1, 1, 6, 30, 0),
                    Frequency = 08,
                    Destination = " רחוב:  עיר: מודיעין מכבים רעות רציף:   קומה:"
                },
                new LineTrip
                {
                    TripId = 3,
                    LineNumber = 50,
                    LineIdTrip = 3,
                    StationKey = 38903,
                    Departure = new DateTime(2020, 1, 1, 6, 20, 0),
                    Frequency = 15,
                    Destination = " רחוב:  עיר: מודיעין מכבים רעות רציף:   קומה:"
                },
                new LineTrip
                {
                    TripId = 4,
                    LineNumber = 113,
                    LineIdTrip = 4,
                    StationKey = 38831,
                    Departure = new DateTime(2020, 1, 1, 6, 12, 0),
                    Frequency = 5,
                    Destination = "רחוב:דרך הזית  עיר: שילת רציף:   קומה"
                },
            };
		}
	}
}