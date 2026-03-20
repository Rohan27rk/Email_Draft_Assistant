using Email_Assistant.Models;
using System.Collections.Concurrent;

namespace Email_Assistant.Services
{
    /// <summary>
    /// Thread-safe in-memory implementation of email history storage.
    /// Stores entries for the application lifetime.
    /// </summary>
    public class EmailHistoryService
    {
        // Using ConcurrentBag for thread-safe operations
        private readonly ConcurrentBag<EmailHistoryEntry> _history = new();

        /// <summary>
        /// Adds a new entry to the email history with current timestamp.
        /// </summary>
        public void AddEntry(EmailRequest request, EmailResponse response)
        {
            var entry = new EmailHistoryEntry
            {
                Timestamp = DateTime.UtcNow,
                Request = request,
                Response = response
            };

            _history.Add(entry);
        }

        /// <summary>
        /// Retrieves all stored email history entries in reverse chronological order.
        /// Returns empty collection if no history exists.
        /// </summary>
        public IEnumerable<EmailHistoryEntry> GetAllEntries()
        {
            // Return entries in reverse chronological order (newest first)
            return _history.OrderByDescending(e => e.Timestamp);
        }
    }
}
