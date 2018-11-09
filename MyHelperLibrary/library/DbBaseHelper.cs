using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data;

namespace MyHelper
{
    public abstract class DbBaseHelper
    {
        public static readonly char splitChar = '`';

        public static readonly string valueSplitChar = "'";
        //软删除的数据字段名
        public static readonly string softDeletedbName = "is_delete";
        //软删除的实体类属性名
        public static readonly string softDeletePropertyName = "isDelete";
        public static readonly string deleteStatusTag = "1"; //删除状态标识
        public static readonly string nomalStatusTag = "0";//正常状态标识
        //软删除的条件
        public static readonly string softDeleteWhere = splitChar + softDeletedbName + splitChar + "=" + valueSplitChar + deleteStatusTag + valueSplitChar;
        public static readonly string softDeleteSet = splitChar + softDeletedbName + splitChar + "=" + valueSplitChar + deleteStatusTag + valueSplitChar;
        public static readonly string notSoftDeleteWhere = splitChar + softDeletedbName + splitChar + "=" + valueSplitChar + nomalStatusTag + valueSplitChar;
        public static readonly string selectSqlTemplqte = "SELECT {0} FROM {1} WHERE {2} ;";
        public static readonly string groupByTemplate = " GROUP BY {0} ";
        public static readonly string orderByTemplate = " ORDER BY {0} ";
        public static readonly string havingTemplate = " HAVING  {0} ";
        public static readonly string LimitTemplate = " LIMIT  {0} ";
        public static readonly string offsetTemplate = " OFFSET  {0} ";


        public static readonly string insertSqlTemplqte = "INSERT INTO {0} ({1}) VALUES ({2});";
        public static readonly string updateSqlTemplqte = "UPDATE {0} SET {1} WHERE {2};";
        public static readonly string deleteSqlTemplqte = "DELETE FROM {0} WHERE {1};";
        public static readonly string buildSqlErrorMessage = "无法获取id或Id的属性名或值，无法对生成SQL的Where条件！";

        /// <summary>
        /// 按主键删除数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>影响行数</returns>
        public abstract int deleteByPrimaryKey(string id);

        /// <summary>
        /// 按主键查找数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>Datatable</returns>
        public abstract DataTable selectByPrimaryKey(string id);

        /// <summary>
        /// 按sql语句查询
        /// </summary>
        /// <param name="sql">sql语句查询</param>
        /// <returns>Datatable</returns>
        public abstract List<T> select<T>(string sql, Dictionary<string, object> parametes = null);

        /// <summary>  
        /// DataTable转化为List集合  
        /// </summary>  
        /// <typeparam name="T">实体对象</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <returns>返回list集合</returns>  
        public static List<T> DataTableToList<T>(DataTable dataTable)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            List<string> colums = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                //集合属性数组  
                PropertyInfo[] propertyinfos = type.GetProperties();
                //新建对象实例
                T entity = Activator.CreateInstance<T>();
                foreach (PropertyInfo p in propertyinfos)
                {
                    if (!dataTable.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;
                    }
                    try
                    {
                        //类型强转，将table字段类型转为集合字段类型  
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {

                    }
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>  
        /// List集合转DataTable  
        /// </summary>  
        /// <typeparam name="T">实体类型</typeparam>  
        /// <param name="list">传入集合</param>  
        /// <param name="isStoreDB">是否存入数据库DateTime字段，date时间范围没事，取出展示不用设置TRUE</param>  
        /// <returns>返回datatable结果</returns>
        public DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;
            Type type = typeof(T);
            PropertyInfo[] propertyinfos = type.GetProperties();
            foreach (var item in propertyinfos)
            {
                dataTable.Columns.Add(item.Name, item.PropertyType);
            }
            foreach (var item in list)
            {
                dataRow = dataTable.NewRow();
                foreach (var propertyInfo in propertyinfos)
                {
                    object obj = propertyInfo.GetValue(item, null);
                    if (obj == null)
                    {
                        continue;
                    }
                    if (propertyInfo.GetType() == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    dataRow[propertyInfo.Name] = obj;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        /// <summary>
        ///  Table To Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToEntitys<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (row[StringHelper.camelCaseToDBnameing(p.Name)] is DBNull)
                    {
                        p.SetValue(entity, null, null);
                        continue;
                    }
                    p.SetValue(entity, row[StringHelper.camelCaseToDBnameing(p.Name)], null);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>  
        /// 提取DataTable某一行转为指定对象
        /// </summary>  
        /// <typeparam name="T">实体</typeparam>  
        /// <param name="dt">传入的表格</param>  
        /// <param name="rowindex">table行索引，默认为第一行</param>  
        /// <returns>返回实体对象</returns> 

        public static T tableToEntity<T>(DataTable dataTable, int rowIndex)
        {
            Type type = typeof(T);
            T entity = Activator.CreateInstance<T>();
            if (dataTable == null)
            {
                return entity;
            }
            DataRow row = dataTable.Rows[rowIndex];
            PropertyInfo[] pArray = type.GetProperties();
            foreach (PropertyInfo p in pArray)
            {
                if (!dataTable.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                {
                    continue;
                }

                if (p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-02"))
                {
                    continue;
                }
                try
                {
                    //类型强转，将table字段类型转为对象字段类型 
                    var obj = Convert.ChangeType(row[p.Name], p.PropertyType);
                    p.SetValue(entity, obj, null);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return entity;
        }

        #region 拼装通用的SQL语句

        /// <summary>
        /// 获取一个对像所对应的数据表名
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>数据表名</returns>
        public static string getTableName<T>(T obj)
        {
            Type type = typeof(T);
            string name = StringHelper.camelCaseToDBnameing(type.Name);
            return name;
        }




        /// <summary>
        /// 获得查询SQL语句 自动加上软删除的条件
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="conditon"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string getSelectSql(string tableName, string fields = null, string conditon = null, string groupBy = null, string having = null, string orderBy = null, int limit = 0, int offset = -1)
        {
            string sql = string.Empty;
            if (string.IsNullOrEmpty(fields))
            {
                fields = " * ";
            }
            if (string.IsNullOrEmpty(conditon))
            {
                conditon = notSoftDeleteWhere;
            }
            else
            {
                conditon = "(" + conditon + ")" + " and " + notSoftDeleteWhere;
            }
            sql = string.Format(selectSqlTemplqte, fields, tableName, conditon);

            if (!string.IsNullOrEmpty(groupBy))
            {
                sql = sql.Replace(";", string.Format(groupByTemplate, groupBy) + " ;");
            }

            if (!string.IsNullOrEmpty(having))
            {
                sql = sql.Replace(";", string.Format(havingTemplate, having) + " ;");
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sql = sql.Replace(";", string.Format(orderByTemplate, orderBy) + " ;");
            }
            if (limit > 0)
            {
                sql = sql.Replace(";", string.Format(LimitTemplate, limit) + " ;");
            }

            if (offset > -1)
            {
                sql = sql.Replace(";", string.Format(offsetTemplate, offset) + " ;");
            }
            return sql;
        }

        /// <summary>
        ///获得查询SQL语句 去除软删除的条件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>插入SQL语句</returns>
        public static string getSelectSqlNoSoftDeleteCondition(string tableName, string fields = null, string conditon = null, string groupBy = null, string having = null, string orderBy = null, int limit = 0, int offset = -1)
        {
            string sql = string.Empty;
            if (string.IsNullOrEmpty(fields))
            {
                fields = " * ";
            }
            if (string.IsNullOrEmpty(conditon))
            {
                conditon = notSoftDeleteWhere;
            }
            else
            {
                conditon = "(" + conditon + ")";
            }
            sql = string.Format(selectSqlTemplqte, fields, tableName, conditon);

            if (!string.IsNullOrEmpty(groupBy))
            {
                sql = sql.Replace(";", string.Format(groupByTemplate, groupBy) + " ;");
            }

            if (!string.IsNullOrEmpty(having))
            {
                sql = sql.Replace(";", string.Format(havingTemplate, having) + " ;");
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sql = sql.Replace(";", string.Format(orderByTemplate, orderBy) + " ;");
            }
            if (limit > 0)
            {
                sql = sql.Replace(";", string.Format(LimitTemplate, limit) + " ;");
            }

            if (offset > -1)
            {
                sql = sql.Replace(";", string.Format(offsetTemplate, offset) + " ;");
            }
            return sql;
        }
        /// <summary>
        ///拼装通用的插入SQL语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>插入SQL语句</returns>
        public static string getInsertSql<T>(T obj)
        {
            string columNames = string.Empty;
            string values = string.Empty;
            Type type = typeof(T);
            PropertyInfo[] propertyinfos = type.GetProperties();
            object o;
            foreach (var property in propertyinfos)
            {
                o = property.GetValue(obj, null);
                if (o == null)
                {
                    continue;
                }
                if (property.GetType() == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                {
                    continue;
                }
                if (columNames.Length == 0)
                {
                    columNames += splitChar + StringHelper.camelCaseToDBnameing(property.Name) + splitChar;
                }
                else
                {
                    columNames += "," + splitChar + StringHelper.camelCaseToDBnameing(property.Name) + splitChar;
                }
                if (values.Length == 0)
                {
                    values += valueSplitChar + property.GetValue(obj, null).ToString() + valueSplitChar;
                }
                else
                {
                    values += "," + valueSplitChar + o.ToString() + valueSplitChar;
                }
            }
            return string.Format(insertSqlTemplqte, getTableName(obj), columNames, values);
        }

        /// <summary>
        /// 获取修改SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="set"></param>
        /// <param name="condition"></param>
        /// <returns>修改SQL语句 或 null </returns>
        public static string getUpdateSql(string tableName, string set, string condition)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(set) || string.IsNullOrEmpty(condition))
            {
                return null;
            }
            return string.Format(updateSqlTemplqte, tableName, set, condition);
        }

        /// <summary>
        ///拼装通用的修改SQL语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        /// <returns>修改SQL语句</returns>
        public static string getUpdateSql<T>(T obj)
        {
            string set = string.Empty;
            string condition = string.Empty;
            Type type = typeof(T);
            PropertyInfo[] properinfors = type.GetProperties();
            object tempObj;
            foreach (PropertyInfo p in properinfors)
            {
                tempObj = p.GetValue(obj, null);
                if (tempObj == null)
                {
                    continue;
                }
                if (p.GetType() == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                {
                    continue;
                }
                if (p.Name == "id" || p.Name == "Id")
                {
                    if (tempObj != null && tempObj.ToString().Length > 0)
                    {
                        condition = splitChar + "id" + splitChar + " = " + valueSplitChar + tempObj.ToString() + valueSplitChar;
                    }
                }
                else
                {
                    if (set.Length == 0)
                    {
                        set = splitChar + StringHelper.camelCaseToDBnameing(p.Name) + splitChar + " = " + valueSplitChar + tempObj.ToString() + valueSplitChar;
                    }
                    else
                    {
                        set += "," + splitChar + StringHelper.camelCaseToDBnameing(p.Name) + splitChar + " = " + valueSplitChar + tempObj.ToString() + valueSplitChar;
                    }
                }

            }
            if (condition.Length == 0)
            {
                throw new Exception(buildSqlErrorMessage);
            }
            return string.Format(updateSqlTemplqte, getTableName(obj), set, condition);
        }

        /// <summary>
        ///拼装通用的删除SQL语句 判断是否支持软删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">类型的对像</param>
        ///   /// <param name="isTrueDelete">是否真的删除 默认软删除</param>
        /// <returns>删除SQL语句</returns>
        public static string getDeleteSql<T>(T obj, bool isTrueDelete = false)
        {
            string condition = string.Empty;
            string id = string.Empty;
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
                if (isTrueDelete == false)
                {
                    try
                    {
                        PropertyInfo deletePropertyinfo = type.GetProperty(softDeletePropertyName);
                        if (deletePropertyinfo != null)
                        {
                            string where = splitChar + "id" + splitChar + "=" + valueSplitChar + tempObj.ToString() + valueSplitChar;
                            return getUpdateSql(getTableName(obj), softDeleteSet, where);
                        }
                    }
                    catch
                    {
                        //nothing to do 不做什么处理，因为数据库的表不包含“is_delete”字段， 不支持软删除
                    }
                }
                condition = splitChar + "id" + splitChar + "=" + valueSplitChar + tempObj.ToString() + valueSplitChar;
            }
            catch (AmbiguousMatchException e)
            {
                throw new Exception(buildSqlErrorMessage + e.Message);
            }
            catch (ArgumentNullException e)
            {
                throw new Exception(buildSqlErrorMessage + e.Message);
            }
            return string.Format(deleteSqlTemplqte, getTableName(obj), condition);
        }

        /// <summary>
        /// 获取删除SQL语句
        /// </summary>
        /// <param name="tableName">表名：不能为null</param>
        /// <param name="condition">条件：不能为null</param>
        /// <returns>SQL语句 或者 null</returns>
        public static string getDeleteSql(string tableName, string condition)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(condition))
            {
                return null;
            }
            return string.Format(deleteSqlTemplqte, tableName, condition);
        }
        #endregion

        /// <summary>
        /// 检查对像是已经存在于数据库中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool checkExist<T>(T obj);

        /// <summary>
        /// 将DataTable的首行转换为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T DataTableToEntity<T>(System.Data.DataTable table) where T : class, new()
        {
            var entity = new T();
            if (table == null || table.Rows.Count <= 0)
            {
                return entity;
            }
            // 获得此模型的公共属性 
            PropertyInfo[] propertys = entity.GetType().GetProperties();
            //遍历该对象的所有属性  
            foreach (var p in propertys)
            {
                //将属性名称赋值给临时变量
                string tmpName = StringHelper.camelCaseToDBnameing(p.Name);

                //检查DataTable是否包含此列（列名==对象的属性名）    
                if (table.Columns.Contains(tmpName))
                {
                    // 判断此属性是否有Setter
                    if (!p.CanWrite)
                    {
                        continue; //该属性不可写，直接跳出
                    }

                    //取值  
                    var value = table.Rows[0][StringHelper.DBNamingToCamelCase(tmpName)];

                    //如果非空，则赋给对象的属性  
                    if (value != DBNull.Value)
                    {
                        p.SetValue(entity, value, null);
                    }
                    else
                    {
                        p.SetValue(entity, null, null);
                    }
                }
            }
            return entity;
        }

    }
}
