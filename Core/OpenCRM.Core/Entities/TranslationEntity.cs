using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCRM.Core.Data
{
    public class TranslationEntity : BaseEntity
    {
        public required string Key { get; set; }
        public required string Translation { get; set; }

        public Guid LanguageId { get; set; } // Required foreign key property
        public LanguageEntity Language { get; set; } = null!; // Required reference navigation to principal
    }
}
