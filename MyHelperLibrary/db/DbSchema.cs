using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
  public  class DbSchema
    {
        public string TableName { get; set; }
        public string TableComment { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string tableRows { get; set; }
        public string dataLength { get; set; }
    }
}
