using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
   public class MysqlTableColumnSchema
    {
        public string columnName { get; set; }
        public string type { get; set; }
        public string defaultValue { get; set; }
        public string isNullable { get; set; }
        public string commentValue { get; set; }
        
    }
}
