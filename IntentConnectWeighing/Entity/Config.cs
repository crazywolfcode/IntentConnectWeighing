
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IntentConnectWeighing
{

	/// <summary>
    /// 
    ///</summary>

    public  class Config
    {
        public String id { get; set; }

        public String clientId { get; set; }

        public String clientNumber { get; set; }

        public String configName { get; set; }

        public String configValue { get; set; }

        public Int64 configType { get; set; }

        public String description { get; set; }

        public String addtime { get; set; }

        public String addUserId { get; set; }

        public String addUserName { get; set; }

        public String lastUpdateTime { get; set; }

        public String lastUpdateUserId { get; set; }

        public String lastUpdateUserName { get; set; }

        public Int64 syncTime { get; set; }

        public Int64 isDelete { get; set; }

        public String deleteTime { get; set; }

        public Int64 status { get; set; }

    }
}
