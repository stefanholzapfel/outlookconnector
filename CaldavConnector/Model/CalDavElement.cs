using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.Model
{
    public class CalDavElement
    {
        public String CTag { get; set; }
        public String Guid { get; set; }
        public String Url { get; set; }

        public String Summary { get; set; }
        public String Description { get; set; }
        public String Location { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Boolean AllDayEvent { get; set; }
    }
}
