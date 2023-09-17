using System;
using System.Data;
using System.Linq;

namespace InvestorliftBlazor.Data
{
	public class House
	{
		public long Index { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string States { get; set; }
        public string County { get; set; }
        public string LineAddress { get; set; }
        public string Statuses { get; set; }
		public string FullAddress { get; set; }
		public int Price { get; set; }
        public int ARV { get; set; }
        public string SqFootage { get; set; }
		public string HouseType { get; set; }
		public string Parking { get; set; }
		public int YearBuild { get; set; }
		public string LotSize { get; set; }
        public int BedRooms { get; set; }
        public double BathRooms { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ImageLink { get; set; }
        public string WebUrl { get; set; }
		public int Hotness { get; set; }
		public int Views {  get; set; }
		public DateTime PublishedDate { get; set; }

        private readonly DatabaseService DbService;
		public House() { }

		public House(DatabaseService DbService)
		{
			this.DbService = DbService;
		}

		public async Task<List<House>> Get(int offset = 0, int limit = 10)
		{
			return await DbService.GetListAsync<House>($"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [Index], * FROM HOUSE (NOLOCK) ORDER BY PublishedDate DESC OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;");
		}

        public async Task<List<House>> GetAll(CancellationToken cancellationToken)
        {
            return await DbService.GetListObjectAsync<House>($"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [Index], * FROM HOUSE (NOLOCK) ORDER BY PublishedDate DESC", cancellationToken: cancellationToken);
        }

		public async Task<List<State>> GetStates()
		{
			return await DbService.GetListAsync<State>($"SELECT * FROM dbo.States");
		}
    }
}

