using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 显示控制器品牌系列
	 /// 数据条数:6
	 /// 数据大小:16KB
	 /// </summary>


	  public  class ScaleSeries
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String name{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public DateTime addDate{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 brandId{ get; set; }

	 }
}
