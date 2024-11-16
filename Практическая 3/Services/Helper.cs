using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Практическая_3.Models;

namespace Практическая_3.Services
{
    internal class Helper
    {
        private static HospitalProEntities1 _context;


        public static HospitalProEntities1 GetContext()
        {
            if (_context == null)
            {
                _context = new HospitalProEntities1();
            }
            return _context;
        }

        public int CreatePatientLogin(Login login)
        {
            var context = GetContext();
            int maxId = context.Login.Any()
                ? context.Login.OrderByDescending(u => u.ID_login).First().ID_login
                : 0;

            _context.Login.Add(login);
            try
            {
                _context.SaveChanges();
                return maxId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка!!! :(");
                return -1;
            }
        }
        public void CreatePatient(Patient patient)
        {
            _context.Patient.Add(patient);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка!!! :(");
            }
        }
    }
}
