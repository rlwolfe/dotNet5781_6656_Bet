using System;


namespace DO
{

	public class Bus
    {
        public string Immatriculation { get; set; }
        public BusStatus BusStatus { get; set; }
        public DateTime ImmatriculationDate { get; set; }
        public float Kilometrage { get; set; }
        public float KmOfFuel { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public float MaintenanceKm { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
