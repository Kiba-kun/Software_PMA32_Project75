using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePractice_10.Models
{
    abstract class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    class Film : Unit
    {
        public string Studio { get; set; }
        public DateTime DateOfRelease { get; set; }
        public string Director { get; set; }
        public string  MainActors { get; set; } //TODO create class Actor and replace this prop on List<Actor>
        public string Summary { get; set; }
        public byte Rating { get; set; }
        public int AmountOfReleasedExemplars { get; set; }
        public int AmountOfAvailableExemplars { get; set; }
    }

    class User : Unit
    {
        public string ContactInfo { get; set; } //TODO create class ContactInfo and replace this prop on List<ContactInfo>
        public int MyProperty { get; set; }

    }
}
