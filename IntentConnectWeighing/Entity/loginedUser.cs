using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class loginedUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Int64 IsRememberPassword { get; set; }
        public Int64 IsAutoLogin { get; set; }
        public string LastloginTime { get; set; }
    }
}
