using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    static class Cloning
    {

        internal static T Clone<T>(this T original)
        {
            T target = (T)Activator.CreateInstance(original.GetType());
            foreach (PropertyInfo item in typeof(T).GetProperties())
                item.SetValue(target, item.GetValue(original, null), null);
            return target;
        }
    }
}