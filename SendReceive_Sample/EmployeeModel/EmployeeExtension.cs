using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeeModel
{
    public static class EmployeeExtension
    {
        public static string Serialize(this Employee emp)
        {
            return (JsonConvert.SerializeObject(emp));
        }

        public static Employee Deserialize(this string empJson)
        {
            return ((Employee) JsonConvert.DeserializeObject(empJson));
        }
    }
}
