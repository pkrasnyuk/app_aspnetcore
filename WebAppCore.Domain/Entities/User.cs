namespace WebAppCore.Domain.Entities
{
    public class User<T> : Entity<T>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PassportHash { get; set; }

        public string PassportSalt { get; set; }

        public bool IsLocked { get; set; }
    }
}