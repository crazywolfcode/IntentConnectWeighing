using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:6
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

	 public String carNumber{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String driver{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String driverMobile{ get; set; }

	 /// <summary>
	 /// 注释:驾驶员身份证号码
	 /// 可空:NO
	 /// </summary>

	 public String driverIdnumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String ownerId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String ownerName{ get; set; }

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
	 /// 注释:行驶证ID
	 /// 可空:YES
	 /// </summary>

	 public String vehicleId{ get; set; }

	 /// <summary>
	 /// 注释:0 未启用 1 正常启用
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String remark{ get; set; }

	 }
}
