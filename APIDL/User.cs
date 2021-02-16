namespace DO
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserStatus UserStatus { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
