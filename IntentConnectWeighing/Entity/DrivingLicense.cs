using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 驾驶证
	 /// 数据条数:6
	 /// 数据大小:16KB
	 /// </summary>


	  public  class DrivingLicense
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

	 public String number{ get; set; }

	 /// <summary>
	 /// 注释:0 f 1 m
	 /// 可空:YES
	 ///默认值:1
	 /// </summary>

	 public Int32 sex{ get; set; }

	 /// <summary>
	 /// 注释:国籍
	 /// 可空:NO
	 /// </summary>

	 public String nationality{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 注释:初次领证时间
	 /// 可空:YES
	 /// </summary>

	 public String birthday{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String firstTime{ get; set; }

	 /// <summary>
	 /// 注释:准驾车型
	 /// 可空:YES
	 /// </summary>

	 public String driverClass{ get; set; }

	 /// <summary>
	 /// 注释:本地图片地址
	 /// 可空:YES
	 /// </summary>

	 public String localImg{ get; set; }

	 /// <summary>
	 /// 注释:远程图片地址
	 /// 可空:YES
	 /// </summary>

	 public String remoteImg{ get; set; }

	 /// <summary>
	 /// 注释:期限
	 /// 可空:NO
	 ///默认值:6
	 /// </summary>

	 public Int32 effictYear{ get; set; }

	 /// <summary>
	 /// 注释:起始时间
	 /// 可空:YES
	 /// </summary>

	 public String startTime{ get; set; }

	 /// <summary>
	 /// 注释:失效时间
	 /// 可空:YES
	 /// </summary>

	 public String endTime{ get; set; }

	 /// <summary>
	 /// 注释:所属驾驶员
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String affiliatedUserId{ get; set; }

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
	 /// 可空:NO
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 注释:0 未审核 1 正常使用地磅
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 status{ get; set; }

	 }
}
