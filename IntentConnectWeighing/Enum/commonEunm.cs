using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    /// <summary>
    /// 磅单截图的位置
    /// 0 其它 1 前 2 后 3上
    /// </summary>
    enum billImagePositon
    {
        None,//orther position
        Front,
        Behind,
        Up,
    }
    /// <summary>
    /// 数据库类型
    /// </summary>
    enum DbType
    {
        mysql,
        sqlite
    }

    enum ConfigItemName
    {
        //connectionStrings
        appSettings,
        connectionStrings,


        //appSettings
        programVersion,
        coryRight,
        dbType,
        mysqlConn,
        sqliteConn,
        sqliteDbPath,
        sqliteDbName,
        clientId,
        companyId,
        companyName,
        companyRegisterStep,
        usingStatus,
        softwareVersion,
        defaultTryUseTimes,
        tryUseTimes,
        startUseTime,
        expiryDate,
    
        //url
        syncUp,
        syncDown,

    }

    public enum CompanyOrganizationType
    {
       Primary,
        son,
    }

    public enum CompanyRegisterStep {
        RegisterOnePage,
        RegisterPrimaryPage,
        RegisterPrimaryLicencePage,
        RegisterSonPage,
        RegisterSonLicencePage,
        RegisterPayPage,
        RegisterFinishedPage,
    }
    /// <summary>
    /// 软件使用状态
    /// </summary>
    public enum UsingStatus {
        TryUsing,//试用
        PayUsing //购买使用
    }

    public enum SoftwareVersion {
        localSingle,
        localLan,
        netSingleBusiness,
        netConnection
    }
    /// <summary>
    ///0  其它配置 1 用户客户端配置 2应用客户端配置 3 用户平台配置 4应用平台配置 5平台配置
    /// </summary>
    public enum ConfigType
    {
        OtherConfig,
        clientUserConfig,
        ClientAppConfig,
        platformUserConfig,
        platformAppConfig,
        platformConfig
    }

}
