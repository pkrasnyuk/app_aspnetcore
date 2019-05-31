namespace WebAppCore.Domain.Entities
{
    public class Photo<T> : Entity<T>
    {
        public string Title { get; set; }

        public T AlbumId { get; set; }

        public T FileId { get; set; }

        public string FileName { get; set; }
    }
}