using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoftwarePractice_10.Models;
using System.Diagnostics;
using System.Data.Entity;

namespace SoftwarePractice_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Database.SetInitializer(new DatabaseInitializer());
            using (var db = new MyContext())
            {
                db.Films.Load();
                db.Actors.Load();
                db.ContactInfos.Load();
                var query = (from item in db.Users
                            select item).First();
                Debug.WriteLine(query.ToString());
            }
        }
    }
}
