using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace api_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "http://172.16.163.128/EasyAutomation/services/api.php?method=listsensors";
            ApiHandler apihandler = new ApiHandler("http://172.16.163.128");
            Console.Title = "Apitester";

            PrintStuffToConsole("**************EASY HOME TESTER**************", ConsoleColor.Green);
            Console.WriteLine("");


            Ping pingSender = new Ping();
            IPAddress address = new IPAddress(new byte[] { 172, 16, 163, 128 }); //172.16.163.255
            PingReply reply = pingSender.Send(address);
            if (reply.Status == IPStatus.Success)
            {
                PrintStuffToConsole(String.Format("Server status: ONLINE [PingStatus: {0}]", reply.Status), ConsoleColor.White);
            }
            else
            {
                PrintStuffToConsole("Server status: OFFLINE", ConsoleColor.Red);
            }

            Console.WriteLine("");
            PrintStuffToConsole("**************EndPoint testing**************", ConsoleColor.Green);
            Console.WriteLine("");

            #region ListSensors




            PrintStuffToConsole(String.Format("#Endpoint: {0}", Endpoints.ListSensors), ConsoleColor.Green);
            var sensorList = apihandler.GetSensorList();

            PrintStuffToConsole(String.Format("Number of sensors returned: {0}", sensorList.Result.Count), ConsoleColor.White);
            PrintStuffToConsole(String.Format("Sensor names: "), ConsoleColor.White);
            foreach (var sensor in sensorList.Result)
            {
                PrintStuffToConsole(String.Format("Sensor name: {0}", sensor.Type), ConsoleColor.Green);
            }
            Console.WriteLine();
            PrintStuffToConsole("Check changeable/ non changeable sensors", ConsoleColor.White);

            foreach (var sensor in sensorList.Result)
            {
                if (sensor.Changeable == Changeable.True)
                {
                    PrintStuffToConsole(String.Format("Changeable sensor: {0}", sensor.Type), ConsoleColor.Green);
                }
                else
                {
                    PrintStuffToConsole(String.Format("NON Changeable sensor: {0}", sensor.Type), ConsoleColor.Green);
                }
            }

            PrintStuffToConsole(String.Format("#Endpoint: {0} testing ends", Endpoints.ListSensors), ConsoleColor.Green);
            Console.WriteLine();
            #endregion
            
            Console.ReadLine();
            #region CreateSensor

            PrintStuffToConsole(String.Format("#Endpoint: {0}", Endpoints.CreateSensor), ConsoleColor.Green);
            Sensor randomSensor = CreateRandomSensor();
            

            PrintStuffToConsole(String.Format("Sensor type:{0}, changeable: {1}", randomSensor.Type, randomSensor.Changeable.ToString()), ConsoleColor.White);

            var result = apihandler.CreateSensor(randomSensor);
            PrintStuffToConsole(string.Format("Response from server: {0}", result.Result.Answer), ConsoleColor.Green);
            Console.WriteLine();
            PrintStuffToConsole(String.Format("#Endpoint: {0} testing ends", Endpoints.CreateSensor), ConsoleColor.Green);

            #endregion
            
            Console.ReadLine();
            
            #region ChangeSensorValue
            Console.WriteLine();
            PrintStuffToConsole(String.Format("#Endpoint: {0}", Endpoints.ChangesensorValue), ConsoleColor.Green);
            Sensor switchSensor = sensorList.Result.Find(x => x.Type == "Switcher");
            Console.WriteLine(String.Format("Changing the following sensor: {0}",switchSensor.Type));
            
            var serverResult = apihandler.ChangeSensorValue(switchSensor.Id,new SensorData("","0","SWITCH",switchSensor.Id));
            PrintStuffToConsole(string.Format("Response from server: {0}", serverResult.Result.Answer), ConsoleColor.Green);
            Console.WriteLine();
            PrintStuffToConsole(String.Format("#Endpoint: {0} testing ends", Endpoints.ChangesensorValue), ConsoleColor.Green);

            #endregion
            
            Console.Read();

            #region ChangeSensorValue
            Console.WriteLine();
            PrintStuffToConsole(String.Format("#Endpoint: {0}", Endpoints.GetlatestSensorData), ConsoleColor.Green);
            Console.WriteLine();
            var serverResult2 = apihandler.GetLatestSensorData(switchSensor.Id);
            PrintStuffToConsole("Response from server:", ConsoleColor.White);
            Console.WriteLine();
            SensorData sd = serverResult2.Result[0];
            PrintStuffToConsole(String.Format("SensorData time: {0}",sd.Time),ConsoleColor.Green);
            PrintStuffToConsole(String.Format("SensorData value: {0}", sd.Value), ConsoleColor.Green);
            PrintStuffToConsole(String.Format("SensorData unit: {0}", sd.Unit), ConsoleColor.Green);
            Console.WriteLine();
            PrintStuffToConsole(String.Format("#Endpoint: {0} testing ends", Endpoints.GetlatestSensorData), ConsoleColor.Green);

            #endregion

            Console.ReadLine();


            PrintStuffToConsole(String.Format("#Endpoint: {0}", Endpoints.DeleteSensor), ConsoleColor.Green);
            Console.WriteLine("*************Delete Sensor************");
            sensorList = apihandler.GetSensorList();
            var deletion = apihandler.DeleteSensor(sensorList.Result.Last());
            Console.WriteLine(deletion.Result.Answer);
            PrintStuffToConsole(String.Format("#Endpoint: {0} testing ends", Endpoints.DeleteSensor), ConsoleColor.Green);

            Console.ReadLine();

        }

        private static void PrintStuffToConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static Sensor CreateRandomSensor()
        {
            Random rnd = new Random();
            string name = "Automation" + rnd.Next(0, 88);
            Changeable changeable = (rnd.Next(0, 100) % 2 == 0) ? Changeable.True : Changeable.False;

            Sensor sensor = new Sensor(1, name, changeable);
            return sensor;
        }

    }
}
