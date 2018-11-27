using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 磅称
	 /// 数据条数:2
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Scale
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:磅秤名称
	 /// 可空:YES
	 /// </summary>

	 public String name{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String com{ get; set; }

	 /// <summary>
	 /// 注释:类型是 1耀华系 2宁波柯力 3   托利多 4赛多利斯 0其它
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int32 brandType{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 baudRate{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 dataByte{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 endByte{ get; set; }

	 /// <summary>
	 /// 注释:品牌
	 /// 可空:YES
	 /// </summary>

	 public String brand{ get; set; }

	 /// <summary>
	 /// 注释:系列
	 /// 可空:YES
	 /// </summary>

	 public String series{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String clientId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String companyId{ get; set; }

	 /// <summary>
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
	 /// 注释:0 示是默认 1 默认为入库 2 默认为出库
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 defaultType{ get; set; }

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

	 public Int32 syncTime{ get; set; }

	 }
}
