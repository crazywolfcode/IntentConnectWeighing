﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    /// <summary>
    /// 为界面好显示而创建的虚类
    /// </summary>
   public class MaterialV
    {
        public MaterialCategory Category { get; set; }
        public List<Material> Materials { get; set; }
    }
}
