using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практическая_3.Models
{
    public class StaffDB
    {
        public int ID_staff { get; set; }
        public int speciality { get; set; }
        public char work_experience {  get; set; }
        public int ID_departament { get; set; }
        public int ID_login { get; set; }
        public string firstname { get; set; }
        public string secondname {get; set;}

    }
}
