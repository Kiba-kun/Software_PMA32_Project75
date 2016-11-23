using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePractice_10.Models.DataProviders
{
    class UnitOfWork
    {
        private MyContext _context = new MyContext();

        public GenericRepository<User> Users{ get; set; }
        public GenericRepository<Film> Films{ get; set; }
        public GenericRepository<MainActor> Actors{ get; set; }
        public GenericRepository<ContactInfo> ContactInfos{ get; set; }


        public UnitOfWork()
        {
            Users = new GenericRepository<User>(_context);
            Films = new GenericRepository<Film>(_context);
            Actors = new GenericRepository<MainActor>(_context);
            ContactInfos = new GenericRepository<ContactInfo>(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
