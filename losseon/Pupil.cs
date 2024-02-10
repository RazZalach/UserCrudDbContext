using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace losseon
{
    class Pupil
    {
        public int id { get; set; }
        public string name { get; set; }
        public int teacher_id { get; set; }
        public int grade { get; set; }

        public Pupil( int id, string name, int teacher_id, int grade  )
        {
            this.id = id;
            this.name = name; 
            this.teacher_id = teacher_id;
            this.grade = grade;
        }
    }
}
