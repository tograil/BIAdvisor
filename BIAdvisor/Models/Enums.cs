using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIAdvisor.Web.Models
{
    public enum AlertType
    {
        Failed,
        Success,
        Warning,
        Info
    }

    public enum UserRole
    {
        ReadOnly,
        ReadWrite,
        Admin,
        SuperUser
    }

    public static class EnumHelper
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static int ToEnumValue<T>(this string value)
        {
            return (int)Enum.Parse(typeof(T), value, true);
        }
    }

}
