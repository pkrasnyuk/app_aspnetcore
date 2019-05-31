namespace WebAppCore.Domain.Entities
{
    public class UserRole<T> : Entity<T>
    {
        public T UserId { get; set; }

        public T RoleId { get; set; }
    }
}