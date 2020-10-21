using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data.Models
{
    /// <summary>
    /// Common model for entities to register anotations
    /// </summary>
    public interface IAssetAnotation
    {
        uint Id { get; set; }

        public string Concept { get; set; }

        decimal Amount { get; set; }

        string UserId { get; set; }

        DateTime AtCreated { get; set; }

        public string TextDate { get; set; }

        public string TextDateAgo { get; set; }
    }
}
