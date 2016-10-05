using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePractice_10.Models
{
    abstract class Unit
    {
        public int Id { get; set; }
    }

    [Table("Films")]
    class Film : Unit
    {
        public Film()
        {
            MainActors = new HashSet<MainActor>();
        }
        public string Name { get; set; }
        public string Studio { get; set; }
        public DateTime DateOfRelease { get; set; }
        public string Director { get; set; }
        public virtual HashSet<MainActor> MainActors { get; set; } 
        public string Summary { get; set; }
        public byte? Rating { get; set; }
        public int AmountOfReleasedExemplars { get; set; }
        public int AmountOfAvailableExemplars { get; set; }
    }

    [Table("Users")]
    class User : Unit
    {
        public User()
        {
            Contacts = new HashSet<ContactInfo>();
            TakenFilms = new HashSet<Film>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual HashSet<ContactInfo> Contacts{ get; set; }
        public virtual HashSet<Film> TakenFilms { get; set; }
        public decimal? MoneyToPay { get; set; }
        public DateTime? ReturnDate { get; set; }
        public override string ToString()
        {
            var res =  this.FirstName + " " + this.LastName + "\t\nContact info:";
            foreach (var item in Contacts)
            {
                res += "\t\n" + item.Adress + "\n" + item.Email + "\n" + item.Phone;                
            }
            res += "\nTaken films:";
            foreach (var item in TakenFilms)
            {
                res += "\n'" + item.Name + "'(" + item.Director + ")";
            }
            return res;
        }
    }

    [Table("MainActors")]
    class MainActor : Unit
    {
        public MainActor()
        {
            Films = new HashSet<Film>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public byte Age { get; set; }
        public virtual HashSet<Film> Films { get; set; }
    }

    [Table("ContactInfo")]
    class ContactInfo : Unit
    {
        [Required]
        public virtual User User{ get; set; }

        [Required]
        [RegularExpression(@"[0-9]+")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(20)]
        public string Adress { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
    }
}

//TODO Config models + set connection string in .config
