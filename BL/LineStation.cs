namespace BO
{
	public class LineStation
    {
        public int LineId { get; set; }
        public int StationKey { get; set; }
        public int RankInLine { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
