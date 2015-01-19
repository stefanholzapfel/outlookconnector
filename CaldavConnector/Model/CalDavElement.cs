using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.Model
{

    /// <summary>
    /// Representation of a CalDav item.
    /// </summary>
    public class CalDavElement
    {
        /// <summary>
        /// The etag that indicates if changes where made.
        /// </summary>
        public String ETag { get; set; }
        /// <summary>
        /// A global unique identifier of the item.
        /// </summary>
        public String Guid { get; set; }
        /// <summary>
        /// The URL to the item on the server.
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// The "title" of the item.
        /// </summary>
        public String Summary { get; set; }
        /// <summary>
        /// A more detailed description of the item.
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// The location where the event takes place.
        /// </summary>
        public String Location { get; set; }
        /// <summary>
        /// The date when the item was last modified.
        /// </summary>
        public DateTime? LastModified { get; set; }
        /// <summary>
        /// The start date of the event.
        /// </summary>
        public DateTime? Start { get; set; }
        /// <summary>
        /// The end date of the event.
        /// </summary>
        public DateTime? End { get; set; }
        /// <summary>
        /// Indicates whether it is an all day event.
        /// </summary>
        public Boolean AllDayEvent { get; set; }
    }
}
