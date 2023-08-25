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

            Open();
        }

		public void Open()
		{
			con = new SqlConnection(strConnectionString);
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

                    for (int inc = 0; inc < reader.FieldCount; inc++)
                    {
                        Type type = t.GetType();
                        PropertyInfo prop = type.GetProperty(reader.GetName(inc));
                        prop.SetValue(t, reader.GetValue(inc), null);
                    }

                    ret.Add(t);
                }
            }
            catch
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
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = text;
                cmd.CommandType = commandType;

                if (sqlParameters != null)
                    cmd.Parameters.AddRange(sqlParameters);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                tbl = new DataTable();
                tbl.Load(reader);
            }
            catch { }

            return tbl;
        }

        public DataSet GetDataSet(string text, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            DataSet ds = null;

            try
            {
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

