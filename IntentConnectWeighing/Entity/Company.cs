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


	  public  class Company
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String name{ get; set; }

	 /// <summary>
	 /// 注释:法人
	 /// 可空:YES
	 /// </summary>

	 public String legalPerson{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String nameFirstCase{ get; set; }

	 /// <summary>
	 /// 注释:社会信用代码
	 /// 可空:YES
	 /// </summary>

	 public String creditNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String busineseLincenseNumber{ get; set; }

	 /// <summary>
	 /// 注释:有效果日期
	 /// 可空:YES
	 /// </summary>

	 public String liceseEspriseTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String abbreviation{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String abbreviationFirstCase{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 level{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String parentId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String remark{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addtime{ get; set; }

	 /// <summary>
	 /// 注释:0 未知 1私营 2 国营 3政府
	 /// 可空:YES
	 ///默认值:1
	 /// </summary>

	 public Int32 type{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 status{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 syncTime{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 注释:地址
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 }
}
