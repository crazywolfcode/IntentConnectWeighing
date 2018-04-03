using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyHelper
{
    public class MySqlHelper : DbBaseHelper
    {
        private static  string connectionString = ConfigurationHelper.GetConnectionConfig("mysqlConn");
        private static readonly string connectionStringTemplate = "Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port={4};MultipleActiveResultSets=true";
        //"Data Source=192.168.1.64;Initial Catalog=classzone;Persist Security Info=True;User ID=root;Password=root;Pooling=False;charset=utf8;MAX Pool Size=2000;Min Pool Size=1;Connection Lifetime=30;";
        // "Database=weightsys;Data Source=192.168.88.3;User Id=admin;Password=Txmy0071;pooling=false;CharSet=utf8;port=33069";
      
        private MySqlConnection connection;

        private MySqlConnection mConnection
        {
            get
            {
                if (connection == null)
                {
                    connection = new MySqlConnection(connectionString);
                    if (connection.State != ConnectionState.Open)
                    {
                        try
                        {
                            connection.Open();
                        }
                        catch (Exception e)
                        {
                            ConsoleHelper.writeLine("联接数据库失败、/r/n 有可能是Server关机或者mysql没有启动  " + e.Message);
                        }
                    }
                    return connection;
                }
                else
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        try
                        {
                            connection.Open();
                        }
                        catch (Exception e)
                        {
                            ConsoleHelper.writeLine("联接数据库失败、/r/n 有可能是Server关机或者mysql没有启动  " + e.Message);
                        }
                    }
                    return connection;
                }
            }
            set { connection = value; }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public MySqlHelper(string connstring = null)
        {
            if (!string.IsNullOrEmpty(connstring)) {
                connectionString = connstring;
            }
            if (connectionString == null || connectionString.Length <= 0)
            {
                throw new Exception("Mysql 数据没有正常的配制！");
            }
        }
        /// <summary>
        /// 检查连接能否打开
        /// </summary>
        /// <param name="connstring"></param>
        /// <returns></returns>
        public static bool CheckConn(string connstring)
        {
            using (MySqlConnection conn = new MySqlConnection(connstring))
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
        /// get the all tables in the databaes
        /// 获取数据中所有表
        /// </summary>
        /// <returns></returns>
        public DataTable getAllTable(string dbbame)
        {
            string sql =$"SELECT table_name as `name` from information_schema.tables where table_schema='{dbbame}' and table_type='base table';";
            return this.ExcuteDataTable(sql, null);
        }
        /// <summary>
        /// get the all table's name of in the database
        /// </summary>
        /// <returns></returns>
        public string[] getAllTableName(string dbbame)
        {
            DataTable dt = getAllTable(dbbame);
            if (dt.Rows.Count <= 0) { return null; }
            string[] result = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result[i] = dt.Rows[i]["name"].ToString();
            }
            return result;
        }


        public List<DbSchema> getAllTableSchema(string dbname) {
            List<DbSchema> dss = new List<DbSchema>();
            string sql=$"SELECT TABLE_NAME as tableName,TABLE_COMMENT as tableComment,CREATE_TIME as createTime,UPDATE_TIME as updateTime ,TABLE_ROWS as tableRows,DATA_LENGTH as dataLength   from information_schema.tables where table_schema='{dbname}'  and table_type='base table';";
            DataTable dt = this.ExcuteDataTable(sql, null);
            string json= JsonHelper.ObjectToJson(dt);
            dss = (List<DbSchema>) JsonHelper.JsonToObject(json, typeof(List<DbSchema>));
            return dss;
        }
        /// <summary>
        /// judge the table is or not exist
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool ExistTable(string dbName,string table) {
            if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(table)) {
                return false;
            }
            string sql = $"SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{dbName}' and table_name ='{table}' and table_type='base table' ;";
            DataTable dt = this.ExcuteDataTable(sql, null);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get the schema of table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<MysqlTabeSchema> getTableSchema(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
            List<MysqlTabeSchema> ts = null;
            string sql = string.Format(" desc {0};", tableName);
            DataTable dt = this.ExcuteDataTable(sql, null);
            String json = JsonHelper.ObjectToJson(dt);
            ts = (List<MysqlTabeSchema>)JsonHelper.JsonToObject(json, typeof(List<MysqlTabeSchema>));
            return ts;
        }

        public List<MysqlTableColumnSchema> getTableColumnSchema(string dbname,string tablename) {
            List<MysqlTableColumnSchema> list=null;
            if (string.IsNullOrEmpty(dbname) && string.IsNullOrEmpty(tablename)) {
                return null;
            }
            string sqlT = @"select COLUMN_NAME as columnName ,
                                   COLUMN_TYPE as type,
			                       COLUMN_DEFAULT as defaultValue,
			                       IS_NULLABLE as isNullable,
			                       COLUMN_COMMENT as commentValue
                            from information_schema.columns 
                            where table_schema ='{0}'  and table_name = '{1}';";
            string sql = string.Format(sqlT, dbname, tablename);
            DataTable dt = this.ExcuteDataTable(sql, null);
            String json = JsonHelper.ObjectToJson(dt);
            list = (List<MysqlTableColumnSchema>)JsonHelper.JsonToObject(json, typeof(List<MysqlTableColumnSchema>));
            return list;
        }

        public string getCreateSql(string tableName)
        {
            string  sql= $"show create table {tableName};";
            DataTable dt = this.ExcuteDataTable(sql, null);
            if (dt.Rows.Count > 0) {
                return dt.Rows[0][1].ToString();
            }
            return null;
        }

        /// <summary>
        /// 创建Mysql 的连接字符串
        /// </summary>
        /// <param name="dataSource">IP</param>
        /// <param name="db">数据库名称</param>
        /// <param name="userId">登录用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static string BuildConnectionString(string dataSource, string db, string userId, string pwd, string port)
        {
            return string.Format(connectionStringTemplate, db, dataSource, userId, pwd, port);
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
        /// 查询  
        /// </summary>  
        /// <param name="sql">要执行的sql语句</param>  
        /// <param name="parameters">所需参数</param>  
        /// <returns>结果DataTable</returns>  
        public DataTable ExcuteDataTable(string sql, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (MySqlCommand command = new MySqlCommand(sql, mConnection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
        }

        public int ExcuteNoQuery(string sql) {
            using (MySqlCommand command = new MySqlCommand(sql, mConnection))
            {
             return   command.ExecuteNonQuery();              
            }
        }

        #region 增删改  

        /// <summary>  
        /// 增删改  
        /// </summary>  
        /// <param name="sql">要执行的sql语句</param>  
        /// <param name="parameters">所需参数</param>  
        /// <returns>所受影响的行数</returns>  
        private int ExecuteNonQuery(string sql, MySqlParameter[] parametes)
        {
            int affectedRows = 0;
            System.Data.Common.DbTransaction transation = mConnection.BeginTransaction();
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, mConnection))
                {
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
                mConnection.Close();
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
                using (MySqlCommand command = new MySqlCommand())
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
            string Sql = string.Empty;
            Sql = getUpdateSql(obj);
            return ExecuteNonQuery(Sql, null);
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
                return this.update(obj);
            }
            else
            {
                return this.insert(obj);
            }
        }

        public void getSchema()
        {
            foreach (var item in mConnection.GetSchema().Rows)
            {
                ConsoleHelper.writeLine(item.ToString());
            }

        }
        #endregion

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
                string sql = getSelectSqlNoSoftDeleteCondition(getTableName(obj), null, condition);
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
            //put code in up 
        }
    }
}