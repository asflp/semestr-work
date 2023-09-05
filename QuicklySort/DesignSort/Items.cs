using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSort
{
    internal class Items
    {
        public static List<string> GetList(string[] strings)
        {
            var result = new List<string>();
            for(int i = 0; i < strings.Length; i++)
                result.Add($"{i + 1}. {strings[i]}");
            return result;
        }
    }
}
