namespace WebAppCore.Domain.Entities
{
    public class Album<T> : Entity<T>
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}