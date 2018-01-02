using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntentConnectWeighing
{
    interface DbInterface
    {
        /// <summary>
        /// 获取一个对像所对应的数据表名
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>数据表名</returns>
         string getTableName<T>(T obj);

        /// <summary>
        ///拼装通用的插入SQL语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>插入SQL语句</returns>
        string getInsertSql<T>(T obj);

        /// <summary>
        ///拼装通用的修改SQL语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>修改SQL语句</returns>
        string getUpdateSql<T>(T ob);

        /// <summary>
        ///拼装通用的删除SQL语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>删除SQL语句</returns>
        string getDeleteSql<T>(T ob);
    }
}
