using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCRM.Core.Data
{
    public class DataContainerEntity : BaseEntity
    {
        public required string Type { get; set; }
        public ICollection<DataBlockEntity> DataBlocks { get; } = new List<DataBlockEntity>();
    }
}