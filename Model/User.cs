namespace MinimalAPI.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public   int Id { get; set; }
        public string  Password{ get; set; }
        public string  PasswordHash{ get; set; }
    }
}
