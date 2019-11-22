using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels
{
	public class DynamicSearchResult
	{
		public string connectionString { get; set; }
		public string columnNames { get; set; }
		public string columnValues { get; set; }
		public string tableName { get; set; }
		public string updateCondition { get; set; }
		public string searchCondition { get; set; }
	}
	public class searchParams
	{
		public string tableName { get; set; }
		public string fieldName { get; set; }
		public string fieldId { get; set; }
	}
	public class MPRStatusUpdate
	{
		public int RevisionId { get; set; }
		public string status { get; set; }
		public string typeOfuser { get; set; }
		public string Remarks { get; set; }
	}
	public class mprFilterParams
	{
		public string DocumentNo { get; set; }
		public string DocumentDescription { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Status { get; set; }
		public string CheckedBy { get; set; }
		public string ApprovedBy { get; set; }
	}

}
