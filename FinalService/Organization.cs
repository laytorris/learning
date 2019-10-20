using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
  public  class Organization
    {
        public string Name;
        public string Phone;
        public int ID;
        public Organization()
        {

        }
        public Organization(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }
        public Organization(string name,  string phone, int id)
        {
            Name = name;
            Phone = phone;
            ID = id;
        }

    }
}
