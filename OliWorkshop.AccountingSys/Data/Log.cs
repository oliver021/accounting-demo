using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class Log
    {
        public uint Id { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime Moment { get; set; } = DateTime.Now;

        public LogType Type { get; set; }
    }
}
