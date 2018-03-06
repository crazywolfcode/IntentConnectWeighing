using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 数据条数:7
	 /// 数据大小:16KB
	 /// </summary>


	  public  class User
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
	 /// 注释:0 女 1 男 3变态
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 sex{ get; set; }

	 /// <summary>
	 /// 注释:民族
	 /// 可空:YES
	 /// </summary>

	 public String nation{ get; set; }

	 /// <summary>
	 /// 注释:登陆的账户名称
	 /// 可空:NO
	 /// </summary>

	 public String loginName{ get; set; }

	 /// <summary>
	 /// 注释:用户密码
	 /// 可空:NO
	 /// </summary>

	 public String passwprd{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String weichat{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String qq{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String email{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String mobilephone{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String headerImgUrl{ get; set; }

	 /// <summary>
	 /// 注释:出生年月日
	 /// 可空:NO
	 /// </summary>

	 public String birthDate{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String idNumber{ get; set; }

	 /// <summary>
	 /// 注释:1 过磅员工 2 过磅管理员工 3公司管理员工 4老总
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 roleLevel{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addtime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserName{ get; set; }

	 /// <summary>
	 /// 注释:所属公司的ID
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String affiliatedCompanyId{ get; set; }

	 /// <summary>
	 /// 注释:所属公司
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String company{ get; set; }

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
	 /// 注释:住址
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 注释:0系统人员1公司人员，2驾驶员 3车主
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int32 userType{ get; set; }

	 /// <summary>
	 /// 注释:0 未审核 1 正常使用地磅
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public Int64 status{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String remark{ get; set; }

	 }
}
