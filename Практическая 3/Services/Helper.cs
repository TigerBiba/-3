using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Практическая_3.Models;

namespace Практическая_3.Services
{
    /// <summary>
    /// Возвращает контекст базы данных
    /// </summary>
    /// <returns>Контекст базы данных</returns>
    internal class Helper
    {
        private static HospitalProEntities _context;

        public static HospitalProEntities GetContext()
        {
            if (_context == null)
            {
                _context = new HospitalProEntities();
            }
            return _context;
        }
    }
}
