using System.Data;

namespace InvestorliftBlazor.Data
{
    public class HouseService
    {
        private readonly DatabaseService _context;

        public HouseService(DatabaseService context)
        {
            _context = context;
        }

        public async Task<House?> GetById(string Id)
        {
            return await _context.GetItemAsync<House>($"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Index], * FROM House (NOLOCK) WHERE Id = {Id}");
        }

        public async Task<List<House>> GetHousesNearby(int Id)
        {
            return await _context.GetListAsync<House>($"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Index], * FROM House (NOLOCK) \n" +
                $"WHERE States = (SELECT States FROM House WHERE Id = {Id}) \n" +
                $"AND Id <> {Id} AND CrawlDetailStatus = 2 \n" +
                $"ORDER BY PublishedDate DESC");
        }

        public async Task<List<string>> GetImages(string HouseId)
        {
            var dt = await _context.GetDataTableAsync($"SELECT ImageLink FROM HouseImage WHERE HouseId = {HouseId} ORDER BY Id");
            var images = new List<string>();
            foreach ( DataRow item in dt.Rows )
            {
                images.Add(item["ImageLink"].ToString());
            }
            return images;
        }

        public async Task<string?> GetSummary(string HouseId)
        {
            return (string?)await _context.ExecuteScalarAsync($"SELECT [Description] FROM HouseSummary WHERE HouseId = {HouseId}");
        }
    }
}
