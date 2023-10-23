using Lab4;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4
{
    class Client : INotifyPropertyChanged
    {
        public static UdpClient udpClient = new UdpClient(8002);
        private ObservableCollection<Student> studentsList;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Student> StudentsList
        {
            get { return studentsList; }
            set
            {
                studentsList = value;
                OnPropertyChanged("StudentsList");
            }
        }

        public void FillStudentsList()
        {
            StudentsList = JsonConvert.DeserializeObject<ObservableCollection<Student>>(ReceiveMessage());
        }

        public void SaveStudentListData()
        {
            string jsonData = JsonConvert.SerializeObject(StudentsList);
            SendMessage("Save");
            SendMessage(jsonData);
        }

        public bool CheckStudents()
        {
            foreach(var s in StudentsList)
            {
                if(string.IsNullOrEmpty(s.Name) || string.IsNullOrEmpty(s.Surname) || s.Age == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveStudentNote(int rowId)
        {
            foreach (Student student in StudentsList)
            {
                if(student.Id == rowId)
                {
                    StudentsList.Remove(student);
                    break;
                }
            }
        }

        public Client() { }

        public void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, "127.0.0.1", 8001);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ReceiveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.Unicode.GetString(data);
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
