using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ExcelHeaderAttribute : Attribute
	{
		public string HeaderName { get; }

		public ExcelHeaderAttribute(string headerName)
		{
			HeaderName = headerName;
		}
	}
}
