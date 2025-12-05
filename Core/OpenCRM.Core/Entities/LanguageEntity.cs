using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCRM.Core.Data
{
    public class LanguageEntity : BaseEntity
    {
        public required string Code { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "jsonb")]
        public required string Translations { get; set; }
    }
}
