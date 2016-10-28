using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BIAdvisor.Web.Helpers
{
	public static class DataTableHelper
	{
		/// <summary>
		/// Converts a DataTable to a list with generic objects
		/// </summary>
		/// <typeparam name="T">Generic object</typeparam>
		/// <param name="table">DataTable</param>
		/// <returns>List with generic objects</returns>
		public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
		{
			try
			{
				List<T> list = new List<T>();

				foreach (var row in table.AsEnumerable())
				{
					T obj = new T();

					foreach (var prop in obj.GetType().GetProperties())
					{
						try
						{
							PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
							propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
						}
						catch
						{
							continue;
						}
					}

					list.Add(obj);
				}

				return list;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Converts a DataTable to a SelectList
		/// </summary>
		/// <param name="table">DataTable</param>
		/// <param name="valueField">Value Field</param>
		/// <param name="textField">Text Field</param>
		/// <returns></returns>
		public static SelectList ToSelectList(this DataTable table, string valueField, string textField)
		{
			return table.ToSelectList(valueField, textField, "");
		}


		/// <summary>
		/// Convert a DataTable to s SelectList with a Selected Value
		/// </summary>
		/// <param name="table">DataTable</param>
		/// <param name="valueField">Value Field</param>
		/// <param name="textField">Text Field</param>
		/// <param name="selectedValue">Selected Value</param>
		/// <returns></returns>
		public static SelectList ToSelectList(this DataTable table, string valueField, string textField, string selectedValue)
		{
			List<SelectListItem> list = new List<SelectListItem>();
			list.Add(new SelectListItem { Text = "", Value = "" });
			if(table == null)
			{
				return new SelectList(list, "Value", "Text");
			}
			foreach (DataRow row in table.Rows)
			{
				list.Add(new SelectListItem()
				{
					Text = row[textField].ToString(),
					Value = row[valueField].ToString(),
				});
			}

			return new SelectList(list, "Value", "Text", selectedValue);
		}
	}
}
