using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:96
	 /// 数据大小:112KB
	 /// </summary>


	  public  class WeighingBill
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:1 入库 2 出库
	 /// 可空:YES
	 ///默认值:1
	 /// </summary>

	 public Int32 type{ get; set; }

	 /// <summary>
	 /// 注释:相关磅单的ID
	 /// 可空:YES
	 /// </summary>

	 public String relativeBillId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendNumber{ get; set; }

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

	 public String sendInTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendOutTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendUserName{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double sendGrossWeight{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double sendTraeWeight{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double sendNetWeight{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendMaterialId{ get; set; }

	 /// <summary>
	 /// 注释:物资名称
	 /// 可空:YES
	 /// </summary>

	 public String sendMaterialName{ get; set; }

	 /// <summary>
	 /// 注释:派车单Id
	 /// 可空:YES
	 /// </summary>

	 public String sendCarBillId{ get; set; }

	 /// <summary>
	 /// 注释:派车单编号
	 /// 可空:YES
	 /// </summary>

	 public String sendCarBillNumber{ get; set; }

	 /// <summary>
	 /// 注释:派单员
	 /// 可空:YES
	 /// </summary>

	 public String sendCarUser{ get; set; }

	 /// <summary>
	 /// 注释:过磅进度 0 no 1 finished
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 sendStatus{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String sendRemark{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveNumber{ get; set; }

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

	 public String receiveInTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveOutTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveUserName{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double receiveGrossWeight{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double receiveTraeWeight{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double receiveNetWeight{ get; set; }

	 /// <summary>
	 /// 注释:磅差
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double differenceWeight{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveMaterialId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveMaterialName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String receiveRemark{ get; set; }

	 /// <summary>
	 /// 注释:扣重说明
	 /// 可空:YES
	 /// </summary>

	 public String decuationDescription{ get; set; }

	 /// <summary>
	 /// 注释:扣重
	 /// 可空:NO
	 ///默认值:0.00
	 /// </summary>

	 public Double decuationWeight{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String carId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String plateNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String driver{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String driverMobile{ get; set; }

	 /// <summary>
	 /// 注释:驾驶员身份证号
	 /// 可空:YES
	 /// </summary>

	 public String driverIdNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String printDateTime{ get; set; }

	 /// <summary>
	 /// 注释:0未上传 1 上传一次 2上传两次
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int32 upStatus{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 注释:打印次数
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 printTimes{ get; set; }

	 /// <summary>
	 /// 注释:本条数据所属的公司，也就是增加公司 
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String affiliatedCompanyId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 /// <summary>
	 /// 注释:过磅进度 0 no 1 finished
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 receiveStatus{ get; set; }

	 }
}
