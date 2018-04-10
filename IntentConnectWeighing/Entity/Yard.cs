using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:10
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Yard
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
	 /// 注释:地址
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String affiliatedCompanyId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String affiliatedCompany{ get; set; }

	 /// <summary>
	 /// 注释:终端的ID
	 /// 可空:YES
	 /// </summary>

	 public String clientId{ get; set; }

	 /// <summary>
	 /// 注释:0 未使用 1 正常
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserName{ get; set; }

	 /// <summary>
	 /// 注释:0 否 1 默认
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDefault{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 ///默认值:0
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

	 }
}
