using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

 namespace IntentConnectWeighing
{

	 /// <summary>
	 /// 配置表
	 /// 数据条数:32
	 /// 数据大小:16KB
	 /// </summary>


	  public  class Config
	 {

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String id{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String clientId{ get; set; }

	 /// <summary>
	 /// 可空:NO
	 ///默认值:0
	 /// </summary>

	 public String clientNumber{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String configName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String configValue{ get; set; }

	 /// <summary>
	 /// 注释:0  其它配置 1 用户客户端配置 2应用客户端配置 3 用户平台配置 4应用平台配置 5平台配置
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 configType{ get; set; }

	 /// <summary>
	 /// 注释:说明
	 /// 可空:YES
	 /// </summary>

	 public String description{ get; set; }

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
	 /// 可空:YES
	 /// </summary>

	 public String updateTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String updateUserId{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String updateUserName{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 ///默认值:0
	 /// </summary>

	 public Int64 syncTime{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public Int32 isDelete{ get; set; }

	 /// <summary>
	 /// 可空:YES
	 /// </summary>

	 public String deleteTime{ get; set; }

	 /// <summary>
	 /// 注释:0 未启用 1 正常启用
	 /// 可空:NO
	 ///默认值:1
	 /// </summary>

	 public Int32 status{ get; set; }

	 }
}
