using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:4
	 /// 数据大小:16KB
	 /// </summary>


	  public  class ScaleBrand
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String brandName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public DateTime addDate{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 注释:类型是 1耀华系 2宁波柯力 3   托利多 4赛多利斯 0其它
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int32 type{ get; set; }

	 }
}
