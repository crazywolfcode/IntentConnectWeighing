using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 地址
	 /// 数据条数:0
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Address
	 {

	 /// <summary>
	 /// 注释:公司地址表
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 注释:公司 ID
	 /// 可空:YES
	 /// </summary>

	 public Int32 companyId{ get; set; }

	 /// <summary>
	 /// 注释:所属公司
	 /// 可空:YES
	 /// </summary>

	 public String company{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 provinceId{ get; set; }

	 /// <summary>
	 /// 注释:省
	 /// 可空:YES
	 /// </summary>

	 public String province{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 cityId{ get; set; }

	 /// <summary>
	 /// 注释:市
	 /// 可空:YES
	 /// </summary>

	 public String city{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 countryId{ get; set; }

	 /// <summary>
	 /// 注释:区县
	 /// 可空:YES
	 /// </summary>

	 public String country{ get; set; }

	 /// <summary>
	 /// 注释:详细
	 /// 可空:YES
	 /// </summary>

	 public String detail{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public DateTime addtime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String addUserName{ get; set; }

	 /// <summary>
	 /// 注释:状态 0不能使用删除之类的，1正常使用
	 /// 可空:YES
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

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
	 /// 可空:YES
	 /// </summary>

	 public Int32 syncTime{ get; set; }

	 }
}
