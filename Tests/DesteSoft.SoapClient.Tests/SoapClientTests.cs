using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesteSoft.SoapClient.Tests
{
    [TestClass]
    public class SoapClientTests
    {
        public SoapClient GetSoapClient()
        {
            return new SoapClient("https://localhost:44384/SoapService.asmx", "https://destesoft.com/");
        }

        [TestMethod]
        public void WrongServiceUrlTest()
        {
            SoapClient client = new SoapClient("https://loclahost:44384/invalidService.asmx", "https://destesoft.com/");
            Assert.ThrowsException<HttpRequestException>(() => client.Post<string>("HelloWorld"));
        }

        [TestMethod]
        public void WrongServiceNamespaceTest()
        {
            SoapClient client = new SoapClient("https://localhost:44384/SoapService.asmx", "https://wrongnamespace.com/");
            Assert.ThrowsException<HttpRequestException>(() => client.Post<string>("HelloWorld"));
        }

        [TestMethod]
        public void WrongActionTest()
        {
            SoapClient client = GetSoapClient();
            Assert.ThrowsException<HttpRequestException>(() => client.Post<string>("InvalidSoapAction"));
        }

        [TestMethod]
        public void WrongParametersTest()
        {
            SoapClient client = GetSoapClient();
            Assert.ThrowsException<HttpRequestException>(() => client.Post<string>("Fibonacci", new { value = "abc" }));
        }

        [TestMethod]
        public void HelloWorldTest()
        {
            SoapClient client = GetSoapClient();
            var result = client.Post<string>("HelloWorld");
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod]
        public void GetDateTest()
        {
            SoapClient client = GetSoapClient();
            var result = client.Post<string>("GetCurrentDate");
            var date = DateTime.Parse(result);
            Assert.IsTrue(date >= DateTime.Now.AddSeconds(-2) && date <= DateTime.Now.AddSeconds(2));
        }

        [TestMethod]
        public void GetUtcDateTest()
        {
            SoapClient client = GetSoapClient();
            var result = client.Post<string>("GetCurrentDate", new { utc = true });
            var date = DateTime.Parse(result);
            Assert.IsTrue(date >= DateTime.UtcNow.AddSeconds(-2) && date <= DateTime.UtcNow.AddSeconds(2));
        }

        [TestMethod]
        public void FibonacciTest()
        {
            SoapClient client = GetSoapClient();
            var result = client.Post<string>("Fibonacci", new { limit = 5 });
            Assert.AreEqual("      0      1      1      2      3", result);
        }

        [TestMethod]
        public void GetLastPersonTest()
        {
            SoapClient client = GetSoapClient();
            var result = client.Post<Person>("GetLatestPerson");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddNewPersonTest()
        {
            SoapClient client = GetSoapClient();
            var latestId = client.Post<Person>("GetLatestPerson").Id;
            var person = new Person
            {
                Name = "Mary Shelley",
                Birthdate = new DateTime(1970, 10, 21)
            };
            var newId = client.Post<int>("AddPerson", new { name = person.Name, birthdate = person.Birthdate });
            Assert.AreEqual(latestId + 1, newId);

            var addedPerson = client.Post<Person>("GetLatestPerson");
            Assert.AreEqual(person.Name, addedPerson.Name);
        }
    }


    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }

}
