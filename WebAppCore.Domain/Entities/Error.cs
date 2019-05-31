namespace WebAppCore.Domain.Entities
{
    public class Error<T> : Entity<T>
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}