using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Models
{
    /// <summary>
    /// This class define the basic options to filter records
    /// Nota: The usage is obsolete
    /// </summary>
    [Obsolete]
    public class FilterOptions
    {
        public uint AmountMin {get;set;}

        public uint AmountMax { get;set;}

        public string Category { get;set; }

        public string Term { get;set; }
    }
}
