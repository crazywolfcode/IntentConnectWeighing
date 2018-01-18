using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 摄像头信息表
	 /// 数据条数:0
	 /// 数据大小:16KB
	 /// </summary>


	  public  class CameraInfo
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String ip{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String port{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String userName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String password{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 position{ get; set; }

	 /// <summary>
	 /// 注释:磅秤ID
	 /// 可空:YES
	 /// </summary>

	 public String scaleId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String clientId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String companyId{ get; set; }

	 /// <summary>
	 /// 注释:0 未使用 1 正常使用
	 /// 可空:YES
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 syncTime{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isdelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 }
}
