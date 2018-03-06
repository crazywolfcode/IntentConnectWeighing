using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 行驶证
	 /// 数据条数:6
	 /// 数据大小:16KB
	 /// </summary>


	  public  class VchicleLicense
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String carNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String carType{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String owner{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 注释:使用性质
	 /// 可空:YES
	 /// </summary>

	 public String useCharacter{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String brand{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String vin{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String engineNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String registerDate{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String issueDate{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String affiliatedCarId{ get; set; }

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
