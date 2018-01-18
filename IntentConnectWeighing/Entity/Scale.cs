using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:0
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Scale
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String com{ get; set; }

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

	 public Int32 syncTime{ get; set; }

	 }
}
