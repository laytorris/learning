using FinalService;

using Newtonsoft.Json;
using System;
using System.ServiceProcess;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //Service1 serv = new Service1();
            //if (Environment.UserInteractive)
            //{
            //    serv.RunAsConsole(args);
            //}

            //else
            //{
            //    ServiceBase[] ServicesToRun;
            //    ServicesToRun = new ServiceBase[]
            //    {
            //    new Service1()
            //        };
            //    ServiceBase.Run(ServicesToRun);

            //}
            ////using (ServiceReference1.Service1Client client = new ServiceReference1.Service1Client())
            ////{


            //////    DateTime date = new DateTime(2000, 12, 03);
            //////    Organization org = new Organization("Org", "89123360082");
            //////    Contact newc = new Contact("Георгий", "Зайцев", "Петрович", 'm', date, "89046700481", "23456789", "manager", org);

            //////    var test = JsonConvert.SerializeObject(newc);



            //////    Contact newcontact = JsonConvert.DeserializeObject<Contact>(test);
            //////    client.Open();
            //////    Console.WriteLine(client.Endpoint.Address.Uri.AbsoluteUri);
            //////    string s = client.GetData(1);

            //////    Console.WriteLine(s);
            //////}
            ////Console.Read();
        }
    }
}
