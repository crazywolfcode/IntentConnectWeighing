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


	  public  class CarInfo
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String plateNumber{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String driver{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String driverMobile{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 driverIdnumber{ get; set; }

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

	 public String lastUpdateTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String lastUpdateUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String lastUpdateUserName{ get; set; }

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

	 public Int32 syncTime{ get; set; }

	 /// <summary>
	 /// 注释:0 未启用 1 正常启用
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 }
}
