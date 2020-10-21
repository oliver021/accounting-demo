using OliWorkshop.AccountingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Components
{
    /// <summary>
    /// This a custom logger that system use to create notifications, alert and activity
    /// registration workflow
    /// </summary>
    public class Logger
    {
        public Logger(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ApplicationDbContext Context { get; }

        /// <summary>
        /// Log a simple message with <see cref="LogType"/>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public Task Log(LogType logType, string message)
        {
            return default;
        }

        /// <summary>
        /// Emit an alert
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task LogAlert(string message)
        {
            return Log(LogType.Alert, message);
        }

        /// <summary>
        /// Register an alert
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task LogActivity(string message)
        {
            return Log(LogType.ActiviyLog, message);
        }

        /// <summary>
        /// Create a notification
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task LogNotification(string message)
        {
            return Log(LogType.ActiviyLog, message);
        }
    }
}
