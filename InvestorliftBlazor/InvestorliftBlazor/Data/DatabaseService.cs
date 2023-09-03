using System;
using System.Data;
using System.Reflection;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace InvestorliftBlazor.Data
{
	public class DatabaseService
	{
        private readonly IConfiguration configuration;
		private SqlConnection con;
		private string strConnectionString = string.Empty;
        private int TimeOut = 60000;
        public DatabaseService(IConfiguration configuration)
		{
			this.configuration = configuration;
			strConnectionString = configuration.GetConnectionString("DefaultDB");
            con = new SqlConnection(strConnectionString);
        }

		public void Open()
		{
			if(con.State != ConnectionState.Open)
			    con.Open();
		}

		public void Close()
		{
			if(con.State == System.Data.ConnectionState.Open)
			{
				con.Close();
				SqlConnection.ClearPool(con);
			}	
		}

        public List<T> GetList<T>(string text, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParameters) where T : class, new()
        {
            List<T> ret = null;
            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandTimeout = TimeOut;
                cmd.CommandType = cmdType;

                foreach (SqlParameter parameter in sqlParameters)
                    cmd.Parameters.Add(parameter);

                SqlDataReader reader = cmd.ExecuteReader();
                ret = new List<T>();
                while (reader.Read())
                {
                    T t = new T();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Type type = t.GetType();
                        PropertyInfo prop = type.GetProperty(reader.GetName(i));
                        if(prop != null)
                            prop.SetValue(t, reader.GetValue(i));
                    }

                    ret.Add(t);
                }
                reader.Close();
            }
            catch
            {
                ret = null;
            }

            return ret;
        }

        public async Task<List<T>> GetListAsync<T>(string text, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParameters) where T : class, new()
        {
            string name = string.Empty;
            List<T> ret = null;
            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandTimeout = TimeOut;
                cmd.CommandType = cmdType;

                foreach (SqlParameter parameter in sqlParameters)
                    cmd.Parameters.Add(parameter);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                ret = new List<T>();
                while (reader.Read())
                {
                    T t = new T();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Type type = t.GetType();
                        string pName = reader.GetName(i);
                        PropertyInfo prop = type.GetProperty(pName);
                        if(prop != null)
                        {
                            name = prop.Name;
                            var val = reader.GetValue(i);
                            if(val.GetType() != typeof(DBNull))
                                prop.SetValue(t, reader.GetValue(i));

                        }
                    }

                    ret.Add(t);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ret = null;
            }

            return ret;
        }

        public DataTable GetDataTable(string text, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
		{
			DataTable tbl = null;

            try
			{
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
				cmd.CommandType = commandType;

				if(sqlParameters != null)
					cmd.Parameters.AddRange(sqlParameters);

				tbl = new DataTable();
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(tbl);
            }
			catch { }

            return tbl;
        }

        public async Task<DataTable> GetDataTableAsync(string text, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            DataTable tbl = null;

            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandType = commandType;

                if (sqlParameters != null)
                    cmd.Parameters.AddRange(sqlParameters);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                tbl = new DataTable();
                tbl.Load(reader);
                reader.Close();
            }
            catch { }

            return tbl;
        }

        public DataSet GetDataSet(string text, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            DataSet ds = null;

            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandType = commandType;

                if (sqlParameters != null)
                    cmd.Parameters.AddRange(sqlParameters);

                ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch { }

            return ds;
        }

        public int ExecuteNonQuery(string text, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandTimeout = TimeOut;
                cmd.CommandType = cmdType;

                foreach (SqlParameter parameter in sqlParameters)
                    cmd.Parameters.Add(parameter);

                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string text, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            try
            {
                Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandTimeout = TimeOut;
                cmd.CommandType = cmdType;

                foreach (SqlParameter parameter in sqlParameters)
                    cmd.Parameters.Add(parameter);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                return -1;
            }
        }
    }
}

