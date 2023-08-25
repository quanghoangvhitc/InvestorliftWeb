using System;
using System.Data;

namespace InvestorliftBlazor.Data
{
	public class House
	{
		private readonly DatabaseService DbService;
		public House(DatabaseService DbService)
		{
			this.DbService = DbService;
		}

		public void Get()
		{
			var tbl = DbService.GetDataTable("SELECT * FROM HOUSE");
			
			foreach(DataRow dr in tbl.Rows)
			{
				string str = string.Empty;
				foreach(DataColumn col in tbl.Columns)
					str += $"| {col.ColumnName}: {dr[col.ColumnName]}";
				Console.WriteLine(str);
			}
		}
	}
}

