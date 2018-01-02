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
        NoNe,//orther position
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
        appSettings,
        connectionStrings,
        dbType,
        mysqlConn,
        sqliteConn,
        sqliteDbPath,
        sqliteDbName,

        //qpp
        clientId,
        companyId,

        //url
        syncUp,
        syncDown,

    }

    public enum CompanyOrganizationType
    {
        primary,
        son,
    }
}
