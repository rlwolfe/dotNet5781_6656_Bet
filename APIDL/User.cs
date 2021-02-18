namespace DO
{
	public class User
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public UserStatus UserStatus { get; set; }

		public override string ToString()
		{
			return this.ToStringProperty();
		}
	}
}
