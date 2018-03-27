using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:34
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Province
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public Int32 id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String provinceName{ get; set; }

	 /// <summary>
	 /// 注释:公司个数
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 childrenCount{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 注释:简称
	 /// 可空:YES
	 /// </summary>

	 public String abbreviation{ get; set; }

	 /// <summary>
	 /// 注释:省会
	 /// 可空:YES
	 /// </summary>

	 public String capital{ get; set; }

	 }
}
