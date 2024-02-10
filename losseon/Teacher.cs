using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace losseon
{
    class Teacher
    {
        public int id { get; set; }
        public string name { get; set; }
        public string subject { get; set; }

        public Teacher(int id,string name,string subject)
        {
            this.name = name;
            this.id = id;
            this.subject = subject;
        }

    
    }
}
