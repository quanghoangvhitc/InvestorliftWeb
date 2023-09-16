using System;
namespace InvestorliftBlazor.Data
{
	public class FilterObject
	{
		public string content { get; set; }
		public string state { get; set; }
		public CancellationToken cancelToken { get; set; }
	}
}

