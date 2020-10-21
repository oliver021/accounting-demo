using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class ConceptsTemplates
    {
        public uint Id { get; set; }

        public string PresetName { get; set; }

        public string Concepts { get; set; }

        public int DefaultAmount { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public TemplateType Type { get; set; }
    }
}
