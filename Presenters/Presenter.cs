using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwarePractice_10.Models;
using System.Data;

namespace SoftwarePractice_10.Presenters
{
    enum DbTable : byte
    {
        User,
        Film,
        ContactInfo,
        Actor
    }
    [Obsolete("First tries")]
    class DataManager
    {
        private MyContext _db;

        public DataManager(MyContext context)
        {
            _db = context;
            UpdateFilm(new Film { Id = 1, Name = "zoidac v2.0", Director = "scorseze", DateOfRelease = DateTime.Now });
        }

        public void AddNewUnit(Unit unit)
        {
            if (unit is Film)
            {
                var temp = unit as Film;
                var query = from item in _db.Films
                            select item.Name;
                if (!query.Contains(temp.Name))
                    _db.Films.Add(temp);
                _db.SaveChanges();
                return;
            }

            if (unit is MainActor)
            {
                var temp = unit as MainActor;
                var query = from item in _db.Actors
                            select item.LastName + item.FirstName;
                if (!query.Contains(temp.LastName + temp.FirstName))
                    _db.Actors.Add(temp);
                _db.SaveChanges();
                return;
            }

            if (unit is User)
            {
                var temp = unit as User;
                var query = from item in _db.Users
                            select item.LastName + item.FirstName;
                if (!query.Contains(temp.LastName + temp.FirstName))
                    _db.Users.Add(temp);
                _db.SaveChanges();
                return;
            }
                

            if (unit is ContactInfo)
            {
                var temp = unit as ContactInfo;
                var query = from item in _db.ContactInfos
                            select item.Adress;
                if (!query.Contains(temp.Adress))
                    _db.ContactInfos.Add(temp);
                _db.SaveChanges();
                return;
            }

            throw new ArgumentException("There is already existing unit with same properties!");
        }

        public Unit GetUnitFromDb(int id, DbTable table)
        {
            Unit result = null;
            switch (table)
            {
                case DbTable.User:
                    result = (from item in _db.Users
                              where item.Id == id
                              select item).First();
                    break;
                case DbTable.Film:
                    result = (from item in _db.Films
                              where item.Id == id
                              select item).First();
                    break;
                case DbTable.ContactInfo:
                    result = (from item in _db.ContactInfos
                              where item.Id == id
                              select item).First();
                    break;
                case DbTable.Actor:
                    result = (from item in _db.Actors
                              where item.Id == id
                              select item).First();
                    break;
                default:
                    break;
            }

            return result;
        }

        public void UpdateFilm(Film modifiedFilm)
        {
            //Some info bout updating entities
            //http://stackoverflow.com/questions/15336248/entity-framework-5-updating-a-record

            //var query = (from item in _context.Films
            //            where item.Id == modifiedFilm.Id
            //            select item).First();

            // New way to get elems
            //var original = _context.Films.Find(modifiedFilm.Id);

            //Chosen this because of 1 query update
            //But with this we cant update separate properties
            _db.Films.Attach(modifiedFilm);
            _db.Entry(modifiedFilm).State = EntityState.Modified;
            _db.SaveChanges();
        }

    }

    class Presenter
    {
        private MyContext _context;
        private DataManager _manager;
        public Presenter(MainWindow mainWindow)
        {
            _context = new MyContext();
            _manager = new DataManager(_context);
        }
        //TODO 1:Think off the functional of this project 
        //TODO 2:start implementing presenter with all it`s logic
        public void FilmConstructor()
        {

        }

        public void AddNewUnit(Unit unit)
        {
            _manager.AddNewUnit(unit);
        }

        ~Presenter()
        {
            _context.Dispose();
        }
    }
}
