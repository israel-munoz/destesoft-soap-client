using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;

namespace SoapServerTest
{
    /// <summary>
    /// Summary description for SoapService
    /// </summary>
    [WebService(Namespace = "https://destesoft.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class SoapService : WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCurrentDate(bool utc)
        {
            return (utc ? DateTime.UtcNow : DateTime.Now).ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object Fibonacci(int limit)
        {
            int n1 = 0;
            int n2 = 1;
            int n3;
            int[] values = new int[limit];
            values[0] = n1;
            values[1] = n2;
            for (var i = 2; i < limit; i += 1)
            {
                n3 = n1 + n2;
                values[i] = n3;
                n1 = n2;
                n2 = n3;
            }
            var result = new List<string>();
            var line = new StringBuilder();
            for (var i = 0; i < limit; i += 1)
            {
                string str = $"{values[i],7}";
                if (line.Length + str.Length > 70)
                {
                    result.Add(line.ToString());
                    line.Clear();
                }
                line.Append(str);
            }
            result.Add(line.ToString());
            return string.Join(Environment.NewLine, result);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public Person GetLatestPerson()
        {
            var p = Person.People.Last();
            return p;
        }

        [WebMethod]
        public object AddPerson(string name, DateTime birthdate)
        {
            var person = new Person
            {
                Id = Person.People.Max(p => p.Id) + 1,
                Name = name,
                Birthdate = birthdate
            };
            Person.People.Add(person);
            return ToJson(person.Id);
        }

        private object ToJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }

    [Serializable]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }

        public static List<Person> People = new List<Person>
        {
            new Person
            {
                Id = 1,
                Name = "John Smith",
                Birthdate = new DateTime(1970, 5, 24)
            }
        };
    }
}
