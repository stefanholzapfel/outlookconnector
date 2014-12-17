using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.DataLayer
{
    public class LocalStorageProvider
    {
        public void createDatabase() {
            String filepath = "Data/CalDavConnector.sqlite";
            if (File.Exists(filepath))
                File.Delete(filepath);
            SQLiteConnection.CreateFile("Data/CalDavConnector.sqlite");
        }

        public void loadDatabase() { }

        public void searchEntry(String Guid) { }

        public void writeEntry(String Guid, String Etag) { }

        public void deleteEntry(String Guid) { }

        public void editEntry(String Guid, String Etag) { }
    }
}
