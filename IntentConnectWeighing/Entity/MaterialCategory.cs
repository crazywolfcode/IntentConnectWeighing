using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 物资的分类
	 /// 数据条数:5
	 /// 数据大小:16KB
	 /// </summary>


	  public  class MaterialCategory
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:分类的名称
	 /// 可空:NO
	 /// </summary>

	 public String name{ get; set; }

	 /// <summary>
	 /// 注释:首字母
	 /// 可空:NO
	 /// </summary>

	 public String firstCase{ get; set; }

	 /// <summary>
	 /// 注释:子个数
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 childrenCount{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addtime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserCompany{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 /// <summary>
	 /// 注释:同步时间戳
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 }
}
