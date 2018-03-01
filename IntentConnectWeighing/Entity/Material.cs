using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:8
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Material
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String name{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String categoryId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String categoryName{ get; set; }

	 /// <summary>
	 /// 注释:物质的首拼
	 /// 可空:YES
	 /// </summary>

	 public String nameFirstCase{ get; set; }

	 /// <summary>
	 /// 注释:说明
	 /// 可空:YES
	 /// </summary>

	 public String description{ get; set; }

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

	 public String updateTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String updateUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String updateUserName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 注释:0 未启用 1 正常启用
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 }
}
