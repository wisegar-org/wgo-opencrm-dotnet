using OpenCRM.Core.Data;
using OpenCRM.Core.Models;
using System.ComponentModel.DataAnnotations;


namespace OpenCRM.Core
{
    public class MediaEntity : BaseEntity
    {
        public bool IsPublic { get; set; }
        public required string FileName { get; set; }
        public required string Extension { get; set; }
        public byte[]? FileData { get; set; }
        public MediaType FileType { get; set; }
        
    }
}
