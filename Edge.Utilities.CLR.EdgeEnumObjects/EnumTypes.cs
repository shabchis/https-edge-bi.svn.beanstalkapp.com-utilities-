using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Utilities.CLR.EdgeEnumObjects
{
	public class EnumTypes
	{
		public enum ObjectStatus
		{
			Unknown = 0,
			Active = 1,
			Paused = 2,
			Suspended = 3,
			Ended = 4,
			Deleted = 5,
			Pending = 6
		}
		public enum TextCreativeType
		{
			Title = 1,
			Body = 2,
			DisplayUrl = 3
		}
	}
}
