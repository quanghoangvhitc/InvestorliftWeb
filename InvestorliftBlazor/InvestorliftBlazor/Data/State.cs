using System;
namespace InvestorliftBlazor.Data
{
	public class State
	{
		public string FIPS_Code { get; set; }
        public string AlphaCode { get; set; }
        public string StateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}

