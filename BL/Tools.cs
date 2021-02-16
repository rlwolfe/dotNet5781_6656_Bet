using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BO
{
    static class Tools
    {
        public static string ToStringProperty<T>(this T t, string suffix = "")
        {
            string str = "";
            foreach (PropertyInfo prop in t.GetType().GetProperties())
            {
                var value = prop.GetValue(t, null);
                if (!(value is string) && value is IEnumerable)
                {
                    str += "\n" + suffix + prop.Name + ": ";
                    foreach (var item in (IEnumerable)value)
                        str += item.ToStringProperty("   ");
                }
                else
                    str += "\n" + suffix + prop.Name + ": " + value;
            }
            if (t is ValueType)
                return "   " + t;
            return str;
        }
    }
}
