using System;

namespace WebAppCore.BLL.Models
{
    public class PhotoViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string AlbumId { get; set; }

        public DateTime DateUploaded { get; set; }

        public string FileName { get; set; }

        public byte[] FileBytes { get; set; }
    }
}