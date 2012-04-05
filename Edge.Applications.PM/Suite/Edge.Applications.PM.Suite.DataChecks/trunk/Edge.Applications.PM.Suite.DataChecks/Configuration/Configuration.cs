using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Edge.Applications.PM.Suite.DataChecks.Configuration
{
	public class MetricsValidationsSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public MetricsItemCollection MetricsItems
		{
			get { return (MetricsItemCollection)base[""]; }
			set { base[""] = value; }

		}
	}
	public class MetricsItemCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		protected override string ElementName
		{
			get
			{
				return "MetricValidation";
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new MetricsItem();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MetricsItem)element).Name;
		}
	}
	public class MetricsItem : ConfigurationElement
	{
		[ConfigurationProperty("Name", IsRequired = true)]
		public string Name
		{
			get { return (string)base["Name"]; }
			set { base["Name"] = value; }
		}

		[ConfigurationProperty("ClassName", IsRequired = true)]
		public string ClassName
		{
			get { return (string)base["ClassName"]; }
			set { base["ClassName"] = value; }
		}

		[ConfigurationProperty("ChannelID", IsRequired = true)]
		public string ChannelID
		{
			get { return (string)base["ChannelID"]; }
			set { base["ChannelID"] = value; }
		}

		[ConfigurationProperty("RunHasLocal", IsRequired = true)]
		public string RunHasLocal
		{
			get { return (string)base["RunHasLocal"]; }
			set { base["RunHasLocal"] = value; }
		}
	}


	//=======================================================================================================================
	public class ValidationsTypesSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public ValidationsTypesCollection ValidationTypeItems
		{
			get { return (ValidationsTypesCollection)base[""]; }
			set { base[""] = value; }

		}
	}
	public class ValidationsTypesCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		protected override string ElementName
		{
			get
			{
				return "ValidationType";
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ValidationTypeItem();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ValidationTypeItem)element).Name;
		}
	}
	public class ValidationTypeItem : ConfigurationElement
	{
		[ConfigurationProperty("Name", IsRequired = true)]
		public string Name
		{
			get { return (string)base["Name"]; }
			set { base["Name"] = value; }
		}

		[ConfigurationProperty("Type", IsRequired = true)]
		public string Type
		{
			get { return (string)base["Type"]; }
			set { base["Type"] = value; }
		}

		[ConfigurationProperty("ClassRef", IsRequired = true)]
		public string ClassRef
		{
			get { return (string)base["ClassRef"]; }
			set { base["ClassRef"] = value; }
		}

		[ConfigurationProperty("ServiceToUse", IsRequired = true)]
		public string ServiceToUse
		{
			get { return (string)base["ServiceToUse"]; }
			set { base["ServiceToUse"] = value; }
		}
	}

}
