
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IntentConnectWeighing
{

    public  class BDAccessToken
    {
        public String error { get; set; }
        public String errorDescription { get; set; }
        public String refreshToken { get; set; }
        /// <summary>
        /// last get acesess_token date time 
        /// </summary>
        public String getDate { get; set; }

        /// <summary>
        /// expires date default 30 days
        /// </summary>
        public String expiresIn { get; set; }
        public String scope { get; set; }
        public String sessionKey { get; set; }
        public String sessionSecret { get; set; }
        public String accessToken { get; set; }

    }
}
