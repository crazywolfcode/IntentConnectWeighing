using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:5
	 /// 数据大小:16KB
	 /// </summary>


	  public  class SendCarBill
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:编号
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String number{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String carId{ get; set; }

	 /// <summary>
	 /// 注释:车牌号
	 /// 可空:NO
	 /// </summary>

	 public String plateNumber{ get; set; }

	 /// <summary>
	 /// 注释:驾驶员
	 /// 可空:YES
	 /// </summary>

	 public String driver{ get; set; }

	 /// <summary>
	 /// 注释:电话
	 /// 可空:YES
	 /// </summary>

	 public String driverMobile{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String addTime{ get; set; }

	 /// <summary>
	 /// 注释:生效时间
	 /// 可空:YES
	 /// </summary>

	 public String effectiveDate{ get; set; }

	 /// <summary>
	 /// 注释:失效时间
	 /// 可空:YES
	 /// </summary>

	 public String expiryDate{ get; set; }

	 /// <summary>
	 /// 注释:入场时间
	 /// 可空:YES
	 /// </summary>

	 public String inFactoryTime{ get; set; }

	 /// <summary>
	 /// 注释:出场时间
	 /// 可空:YES
	 /// </summary>

	 public String outFactoryTime{ get; set; }

	 /// <summary>
	 /// 注释:0未生效 1 已经生效 2 已经失效
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 effectiveStatus{ get; set; }

	 /// <summary>
	 /// 注释:过磅状态 0未入场 1 已入场 2 已出场
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 weightStatus{ get; set; }

	 /// <summary>
	 /// 注释:过磅单ID
	 /// 可空:YES
	 /// </summary>

	 public String weighingBillId{ get; set; }

	 /// <summary>
	 /// 注释:过磅编号
	 /// 可空:YES
	 /// </summary>

	 public String weighingNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendCompanyId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendCompanyName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendYardId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendYardName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveCompanyId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveCompanyName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveYardId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveYardName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String materialId{ get; set; }

	 /// <summary>
	 /// 注释:物资名称
	 /// 可空:YES
	 /// </summary>

	 public String materialName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String userId{ get; set; }

	 /// <summary>
	 /// 注释:派车员
	 /// 可空:YES
	 /// </summary>

	 public String userName{ get; set; }

	 /// <summary>
	 /// 注释:同步时间戳
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 注释:是否删除 0 未 1删除
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 注释:删除时间
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 /// <summary>
	 /// 注释:备注
	 /// 可空:YES
	 /// </summary>

	 public String remark{ get; set; }

	 }
}
