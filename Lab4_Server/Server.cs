using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab4_Server
{
    internal class Server
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static UdpClient udpServer;
        public string message = "";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 2);

        static async Task Main(string[] args)
        {
            udpServer = new UdpClient(8001);
            Server s = new Server();
            Functions f = new Functions();
            Console.WriteLine("Server works!");
            while (true)
            {
                try
                {
                    UdpReceiveResult result = udpServer.ReceiveAsync().Result;
                    await Task.Run(() => ProcessRequest(ref result, ref s, ref f, ref semaphore));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    logger.Error(ex.StackTrace);
                }
            } 
        }

        private static void ProcessRequest(ref UdpReceiveResult result, ref Server s, ref Functions f, ref SemaphoreSlim semaphore)
        {
            semaphore.Wait();
            try
            {
                s.message = Encoding.Unicode.GetString(result.Buffer);
                IPEndPoint clientEndpoint = result.RemoteEndPoint;
                Console.WriteLine($"Message from client: {s.message}");
                logger.Info($"Server got: {s.message}");
                using (StudentContext db = new StudentContext())
                {
                    switch (s.message)
                    {
                        case "Load":
                            List<Student> studentList = db.Set<Student>().ToList();
                            string jsonData = JsonConvert.SerializeObject(studentList);
                            s.SendMessage(jsonData);
                            break;
                        case "Save":
                            s.ReceiveMessage().Wait();
                            List<Student> newStudentsList = JsonConvert.DeserializeObject<List<Student>>(s.message);
                            db.Database.ExecuteSqlCommand("TRUNCATE TABLE STUDENTS");
                            foreach (var student in newStudentsList)
                            {
                                f.AddNoteToFile(student.Surname, student.Name, student.Patronymic, student.Sex.ToString(), student.Age);
                            }
                            break;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void SendMessage(string response)
        {
            try
            {
                string message = response;
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpServer.Send(data, data.Length, "127.0.0.1", 8002);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ReceiveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpServer.Client.LocalEndPoint;
            try
            {
                UdpReceiveResult result = await udpServer.ReceiveAsync();
                message = Encoding.Unicode.GetString(result.Buffer);
                Console.WriteLine("Message from client: {0}", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
