using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyHelper;
using System.Data;
namespace IntentConnectWeighing
{
    class DatabaseOPtionHelper
    {
        private static MySqlHelper mysqlHelper;
        private static SQLiteHelper sqliteHelper;
        private static string dbType;

        static DatabaseOPtionHelper()
        {
            dbType = ConfigurationHelper.GetConfig(ConfigItemName.dbType.ToString());
            if (dbType == DbType.mysql.ToString())
            {
                mysqlHelper = new MySqlHelper();
            }
            else if (dbType == DbType.sqlite.ToString())
            {
                sqliteHelper = new SQLiteHelper();
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("数据为未配置成功,去配制", "错误", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (res == MessageBoxResult.No) { return; }
                InitializationSettingW w = new InitializationSettingW();
                w.ShowDialog();
            }
        }

        /// <summary>
        /// get the all tables in the databaes
        /// 获取数据中所有表
        /// </summary>
        /// <returns></returns>
        public DataTable getAllTable()
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.getAllTable();
            }
            else
            {
                return sqliteHelper.getAllTable();
            }
        }

        /// <summary>
        /// 按SQl语句查询 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>DataTable</returns>
        public DataTable select(string sql)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.select(sql);
            }
            else
            {
                return sqliteHelper.select(sql);
            }
        }

        /// <summary>
        /// 执行删除语句
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <returns>影响行数</returns>
        public int delete(string sql)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.delete(sql);
            }
            else
            {
                return sqliteHelper.delete(sql);
            }
        }
        /// <summary>
        /// 删除对像 ，支持软件删除
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <param name="isTrueDelete">是否真的删除 默认软删除</param>
        /// <returns>影响行数</returns>
        public int delete<T>(T obj, Boolean isTrueDelete = false)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.delete(obj, isTrueDelete);
            }
            else
            {
                return sqliteHelper.delete(obj, isTrueDelete);
            }
        }

        /// <summary>
        /// 执行修改语句
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <returns>影响行数</returns>
        public int update(string sql)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.update(sql);
            }
            else
            {
                return sqliteHelper.update(sql);
            }
        }
        /// <summary>
        /// 修改对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int update<T>(T obj)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.update(obj);
            }
            else
            {
                return sqliteHelper.update(obj);
            }
        }

        /// <summary>
        /// 执行插入语句
        /// </summary>
        /// <param name="sql">插入语句</param>
        /// <returns>影响行数</returns>
        public int insert(string sql)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.insert(sql);
            }
            else
            {
                return sqliteHelper.insert(sql);
            }
        }
        /// <summary>
        /// 插入对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int insert<T>(T obj)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.insert(obj);
            }
            else
            {
                return sqliteHelper.insert(obj);
            }
        }
        /// <summary>
        /// 插入或者更新对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int insertOrUpdate<T>(T obj)
        {
            if (dbType == DbType.mysql.ToString())
            {
                return mysqlHelper.insertOrUpdate(obj);
            }
            else
            {
                return sqliteHelper.insertOrUpdate(obj);
            }
        }
    }
}
