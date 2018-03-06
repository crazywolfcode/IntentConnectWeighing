using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 车牌号的头两个字符
	 /// 数据条数:4
	 /// 数据大小:16KB
	 /// </summary>


	  public  class CarHeader
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:内容
	 /// 可空:YES
	 /// </summary>

	 public String content{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 }
}
