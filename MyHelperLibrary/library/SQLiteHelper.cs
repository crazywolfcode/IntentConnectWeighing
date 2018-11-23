using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;


namespace MyHelper
{

    public sealed class SQLiteHelper : DbBaseHelper
    {
        private static string connStrTemplate = " Data Source={0};Version=3;Pooling=False;Max Pool Size=100;";
        private static string sqliteDbName = ConfigurationHelper.GetConfig("sqliteDbName");
        private static string dbPath = ConfigurationHelper.GetConfig("sqliteDbPath");
        private static string sqliteConnectionString = ConfigurationHelper.GetConnectionConfig("sqliteConn");
        private static string getTableSchemaSql = "SELECT name as tableName FROM sqlite_master WHERE type='table' ORDER BY name; ";
        private SQLiteConnection connection;

        private SQLiteConnection mConnection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SQLiteConnection(sqliteConnectionString);
                }
                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception e)
                    {
                        ConsoleHelper.SvaeErrorToFile("数据打开失败  " + e.Message);
                        throw new Exception("SQLite 数据打开失败！");
                    }
                }
                return connection;
            }
            set { connection = value; }
        }
        /// <summary>
        /// create  connection string 
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public static string createConnString(string dbname)
        {
            return string.Format(connStrTemplate, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data")) + "\\" + dbname;
        }
        /// <summary>
        /// get the save path of the database
        /// </summary>
        /// <returns></returns>
        public static string getDbSavePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        }

        public SQLiteHelper(string connstr = null)
        {
            if (string.IsNullOrEmpty(connstr))
            {
                if (sqliteDbName == null || sqliteDbName.Length <= 0)
                {
                    sqliteDbName = "interConnectionData.db";
                    ConfigurationHelper.SetConfig("sqliteDbName", sqliteDbName);
                }

                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                    ConfigurationHelper.SetConfig("sqliteDbPath", dbPath);
                }

                if (sqliteConnectionString == null || sqliteConnectionString.Length <= 0)
                {
                    sqliteConnectionString = string.Format(connStrTemplate, dbPath + "\\" + sqliteDbName);
                    ConfigurationHelper.SetConnectionConfig("sqliteConn", sqliteConnectionString);
                }

                if (FileHelper.FolderExistsCreater(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data")))
                {
                    if (!File.Exists(dbPath + "/" + sqliteDbName))
                    {
                        using (File.Create(dbPath + "/" + sqliteDbName))
                        {

                        }
                    }
                }
            }
            else
            {
                sqliteConnectionString = connstr;
            }

        }

        #region 没有实现的方法

        override public int deleteByPrimaryKey(string id)
        {
            throw new NotImplementedException();
        }

        override public DataTable selectByPrimaryKey(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 按SQl语句查询 返回一个Generic（泛型） 的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override List<T> select<T>(string sql, Dictionary<string, object> parametes = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 检查连接能否打开
        /// </summary>
        /// <param name="connstring"></param>
        /// <returns></returns>
        public static bool CheckConn(string connstring)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connstring))
            {
                try
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
                catch (Exception)
                {
                    return false;
                }
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
            return this.ExcuteDataTable(sql, null);
        }

        /// <summary>
        /// get the all tables in the databaes
        /// 获取数据中所有表
        /// </summary>
        /// <returns></returns>
        public DataTable getAllTable()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;  ";
            return this.ExcuteDataTable(sql, null);
        }

        public List<DbSchema> getAllTableSchema()
        {
            List<DbSchema> dss = new List<DbSchema>();
            DataTable dt = this.ExcuteDataTable(getTableSchemaSql, null);
            string json = JsonHelper.ObjectToJson(dt);
            dss = (List<DbSchema>)JsonHelper.JsonToObject(json, typeof(List<DbSchema>));
            return dss;
        }

        public string getCreateSql(string tablename)
        {
            string sql = $"SELECT name,sql FROM sqlite_master WHERE type='table' and name = '{tablename}' ORDER BY name  ; ";
            DataTable dt = this.ExcuteDataTable(sql, null);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][1].ToString();
            }
            return null;
        }
        /// <summary>
        /// get the schema of table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<SqliteTableSchema> getTableSchema(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
            List<SqliteTableSchema> ts = null;
            string sql = string.Format(" PRAGMA table_info({0});", tableName);
            DataTable dt = this.ExcuteDataTable(sql, null);
            String json = JsonHelper.ObjectToJson(dt);
            ts = (List<SqliteTableSchema>)JsonHelper.JsonToObject(json, typeof(List<SqliteTableSchema>));
            return ts;
        }
        /// <summary>
        /// get the all table's name of in the database
        /// </summary>
        /// <returns></returns>
        public string[] getAllTableName()
        {
            DataTable dt = getAllTable();
            if (dt.Rows.Count <= 0) { return null; }
            string[] result = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result[i] = dt.Rows[i]["name"].ToString();
            }
            return result;
        }

        /// <summary>  
        /// 获取多行  
        /// </summary>  
        /// <param name="sql">执行sql</param>  
        /// <param name="param">sql参数</param>  
        /// <returns>多行结果</returns>  
        public DataRow[] getRows(string sql, Dictionary<string, object> param = null)
        {
            List<SQLiteParameter> sqlite_param = new List<SQLiteParameter>();

            if (param != null)
            {
                foreach (KeyValuePair<string, object> row in param)
                {
                    sqlite_param.Add(new SQLiteParameter(row.Key, row.Value.ToString()));
                }
            }
            DataTable dt = this.ExcuteDataTable(sql, sqlite_param.ToArray());
            return dt.Select();
        }

        public bool ExistTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            string sql = $"SELECT name as tableName FROM sqlite_master WHERE type='table' and name ='{tableName}'; ";
            DataTable dt = this.ExcuteDataTable(sql, null);
            if (dt.Rows.Count > 0) return true;
            return false;
        }

        /// <summary>  
        /// SQLite查询  
        /// </summary>  
        /// <param name="sql">要执行的sql语句</param>  
        /// <param name="parameters">所需参数</param>  
        /// <returns>结果DataTable</returns>  
        public DataTable ExcuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SQLiteCommand command = mConnection.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
        }


        public int query(string sql, Dictionary<string, object> parametes = null)
        {
            List<SQLiteParameter> sqlite_param = new List<SQLiteParameter>();
            if (parametes != null)
            {
                foreach (KeyValuePair<string, object> row in parametes)
                {
                    sqlite_param.Add(new SQLiteParameter(row.Key, row.Value.ToString()));
                }
            }
            return this.ExecuteNonQuery(sql, sqlite_param.ToArray());
        }

        #region 增删改  

        /// <summary>  
        /// SQLite增删改  
        /// </summary>  
        /// <param name="sql">要执行的sql语句</param>  
        /// <param name="parameters">所需参数</param>  
        /// <returns>所受影响的行数</returns>  
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parametes)
        {
            int affectedRows = 0;
            System.Data.Common.DbTransaction transation;
            transation = mConnection.BeginTransaction();
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(mConnection))
                {
                    command.CommandText = sql;
                    if (parametes != null)
                    {
                        command.Parameters.AddRange(parametes);
                    }
                    affectedRows = command.ExecuteNonQuery();
                    transation.Commit();
                }
            }
            catch (Exception)
            {
                transation.Rollback();
                throw;
            }
            finally
            {
                mConnection.Cancel();
            }
            return affectedRows;
        }
        /// <summary>
        /// 事务处理多条多条操作
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public int TransactionExecute(string[] sqls)
        {
            int affectedRows = 0;
            System.Data.Common.DbTransaction transation = mConnection.BeginTransaction();
            try
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = mConnection;
                    for (int i = 0; i < sqls.Length; i++)
                    {
                        command.CommandText = sqls[i];
                        affectedRows = command.ExecuteNonQuery();
                    }
                    transation.Commit();
                }
            }
            catch (Exception)
            {
                transation.Rollback();
                throw;
            }
            finally
            {
                mConnection.Close();
            }
            return affectedRows;
        }
        public  int ExcuteNoQuery(string sql)
        {
            using (SQLiteCommand command = new  SQLiteCommand(sql, mConnection))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行删除语句
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <returns>影响行数</returns>
        public int delete(string sql)
        {
            return ExecuteNonQuery(sql, null);
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
            string deleteSql = string.Empty;
            deleteSql = DbBaseHelper.getDeleteSql(obj, isTrueDelete);
            return ExecuteNonQuery(deleteSql, null);
        }

        /// <summary>
        /// 执行修改语句
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <returns>影响行数</returns>
        public int update(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }
        /// <summary>
        /// 修改对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int update<T>(T obj)
        {
            string deleteSql = string.Empty;
            deleteSql = DbBaseHelper.getUpdateSql(obj);
            return ExecuteNonQuery(deleteSql, null);
        }

        /// <summary>
        /// 执行插入语句
        /// </summary>
        /// <param name="sql">插入语句</param>
        /// <returns>影响行数</returns>
        public int insert(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }
        /// <summary>
        /// 插入对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int insert<T>(T obj)
        {
            string insertSql = string.Empty;
            insertSql = DbBaseHelper.getInsertSql(obj);
            return ExecuteNonQuery(insertSql, null);
        }
        #endregion

        /// <summary>  
        /// 查询数据库所有表信息  
        /// </summary>  
        /// <returns>数据库表信息DataTable</returns>  
        public DataTable GetSchema()
        {
            return mConnection.GetSchema("TABLES");
        }

        /// <summary>
        /// 插入或者更新对像
        /// </summary>
        /// <typeparam name="T">对像类型</typeparam>
        /// <param name="obj">对像</param>
        /// <returns>影响行数</returns>
        public int insertOrUpdate<T>(T obj)
        {
            if (checkExist(obj))
            {
                return this.insert(obj);
            }
            else
            {
                return this.update(obj);
            }
        }

        /// <summary>
        /// 检查对像是已经存在于数据库中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        override public bool checkExist<T>(T obj)
        {
            Type type = typeof(T);
            try
            {
                PropertyInfo propertyinfo = type.GetProperty("id");
                if (propertyinfo == null)
                {
                    propertyinfo = type.GetProperty("Id");
                    if (propertyinfo == null)
                    {
                        throw new Exception(buildSqlErrorMessage);
                    }
                }
                object tempObj = propertyinfo.GetValue(obj, null);
                if (tempObj == null || tempObj.ToString().Length <= 0)
                {
                    throw new Exception(buildSqlErrorMessage);
                }
                string condition = splitChar + "id" + splitChar + "=" + valueSplitChar + tempObj.ToString() + valueSplitChar;
                string sql = getSelectSql(getTableName(obj), null, condition);
                DataTable dt = this.select(sql);
                if (dt == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

        }
    }
}