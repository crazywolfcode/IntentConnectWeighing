
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IntentConnectWeighing
{

	/// <summary>
    /// 
    ///</summary>

    public  class Company
    {
        public String id { get; set; }

        public String name { get; set; }

        public String nameFirstCase { get; set; }

        public String busineseLicenseNumber { get; set; }

        public String abbreviation { get; set; }

        public String abbreviationFirstCase { get; set; }

        public Int64 level { get; set; }

        public String parentId { get; set; }

        public String remark { get; set; }

        public String addtime { get; set; }

        public Int64 type { get; set; }

        public Int64 status { get; set; }

        public Int64 syncTime { get; set; }

        public Int64 isDelete { get; set; }

        public String deleteTime { get; set; }

    }
}
