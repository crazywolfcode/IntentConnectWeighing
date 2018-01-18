using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 过磅时系统自动截取图片
	 /// 数据条数:0
	 /// 数据大小:16KB
	 /// </summary>


	  public  class BillImage
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:1 前 2 后 3上
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 positon{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String billId{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String billNumber{ get; set; }

	 /// <summary>
	 /// 注释:1 入场 2出场
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 type{ get; set; }

	 /// <summary>
	 /// 注释:0 未上传 1 已经上传
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isUp{ get; set; }

	 /// <summary>
	 /// 注释:上传时间
	 /// 可空:YES
	 /// </summary>

	 public String upTime{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 }
}
