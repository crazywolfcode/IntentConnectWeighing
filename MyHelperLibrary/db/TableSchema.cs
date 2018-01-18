using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class SqliteTableSchema
    {
        public string name  { get; set; }
        public string type { get; set; }
        public string notnull { get; set; }
        public string dflt_value { get; set; }
        public string pk { get; set; }
        
    }
}
