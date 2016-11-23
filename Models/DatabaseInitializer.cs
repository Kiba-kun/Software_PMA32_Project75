using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SoftwarePractice_10.Models
{
    class DatabaseInitializer : DropCreateDatabaseIfModelChanges<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            Film testFilm = new Film
            {
                Name = "Zodiac",
                Director = "Scorseze",
                DateOfRelease = DateTime.Now,
                Studio = "20`s century Fox",
                AmountOfAvailableExemplars = 10,
                AmountOfReleasedExemplars = 5000,
                Summary = "Great film bout cruel maniac."
            };
            MainActor testActor = new MainActor
            {
                FirstName = "Jake",
                LastName = "Jylenhaal",
                Age = 40              
            };
            testActor.Films.Add(testFilm);
            User testUser = new User 
            {
                FirstName = "Andrew",
                LastName = "Kibalnikov"
            };
            ContactInfo testUserContactInfo = new ContactInfo
            {
                //User = testUser,
                Adress = "Some str",
                Phone = "2405438",
                Email = "ak13019@gmail.com"
            };
            testUser.Contacts.Add(testUserContactInfo);
            testUser.TakenFilms.Add(testFilm);

            context.Films.Add(testFilm);
            context.Users.Add(testUser);
            context.ContactInfos.Add(testUserContactInfo);
            context.Actors.Add(testActor);

            context.SaveChanges();
        }
    }
}
