using System;
using api_tester;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiTests
{
    [TestClass]
    public class ApiUnitTests
    {
        string baseUrl = "http://172.16.163.128/EasyAutomation/services/api.php?method=listsensors";
        ApiHandler apihandler = new ApiHandler("http://172.16.163.128");

        [TestMethod]
        public  void CreateSensorApiTest()
        {
            Sensor newSensor = CreateRandomSensor();
            var result =  apihandler.CreateSensor(newSensor);
            string expected = "[\"inserted\"]";
            string actual = result.Result.Answer;
            Assert.AreEqual(expected,actual);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllSensorsApiTest()
        {
            var result = apihandler.GetSensorList();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Count>0);
        }


        [TestMethod]
        public void DeleteSensorApiTest()
        {
            var getSensorList = apihandler.GetSensorList().Result;
            var serverAnswer = apihandler.DeleteSensor(getSensorList[0]);
            
            string expected = "[true]";
            string actual = serverAnswer.Result.Answer;
            Assert.AreEqual(expected,actual);
            Assert.IsNotNull(serverAnswer.Result);
        }

        private Sensor CreateRandomSensor()
        {
            Random rnd = new Random();
            string name = "Automation" + rnd.Next(0, 88);
            Changeable changeable = (rnd.Next(0, 100) % 2 == 0) ? Changeable.True : Changeable.False;

            Sensor sensor = new Sensor(1, name, changeable);
            return sensor;
        }
    }
}
