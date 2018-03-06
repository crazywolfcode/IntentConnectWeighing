using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 银行卡
	 /// 数据条数:0
	 /// 数据大小:16KB
	 /// </summary>


	  public  class BankCard
	 {

	 /// <summary>
	 /// 可空:NO
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String affiliatedUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String affiliatedUserName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String brankName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String address{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String number{ get; set; }

	 /// <summary>
	 /// 注释: 0 信用卡 1 储蓄卡
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 type{ get; set; }

	 }
}
