using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Edge.Applications.PM.Client.Configuration
{
	public class MenuSection  : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired=true, IsDefaultCollection=true)]
		public MenuItemCollection MenuItems
		{
			get { return (MenuItemCollection) base[""]; }
			set { base[""] = value; }
		}
	}
	
	public class MenuItemCollection : ConfigurationElementCollection
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
				return "MenuItem";
			}
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MenuItem)element).Name;
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new MenuItem();
		}
	}

	public class MenuItem : ConfigurationElement
	{
		[ConfigurationProperty ("Name",IsRequired = true)]
		public string Name
		{
			get { return (string)base["Name"]; }
			set { base["Name"] = value; }
		}

		[ConfigurationProperty("Class", IsRequired = true , DefaultValue = "TopMenu")]
		public string Class
		{
			get { return (string)base["Class"]; }
			set { base["Class"] = value; }
		}

		[ConfigurationProperty("Image", IsRequired = false, DefaultValue = "")]
		public string Image
		{
			get { return (string)base["Image"]; }
			set { base["Image"] = value; }
		}

		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public MenuItemCollection MenuItems
		{
			get { return (MenuItemCollection)base[""]; }
			set { base[""] = value; }
		}

	}
}
