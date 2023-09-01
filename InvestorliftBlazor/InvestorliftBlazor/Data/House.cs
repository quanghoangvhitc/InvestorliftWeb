﻿using System;
using System.Data;
using System.Linq;

namespace InvestorliftBlazor.Data
{
	public class House
	{
		public long Index { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string FullAddress { get; set; }
		public int Price { get; set; }

		private readonly DatabaseService DbService;
		public House() { }

		public House(DatabaseService DbService)
		{
			this.DbService = DbService;
		}

		public async Task<List<House>> Get(int offset = 0, int limit = 10)
		{
			return await DbService.GetListAsync<House>("SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [Index], * FROM HOUSE (NOLOCK) ORDER BY PublishedDate DESC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;");
		}
	}
}

