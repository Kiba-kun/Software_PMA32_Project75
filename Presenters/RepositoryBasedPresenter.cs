using SoftwarePractice_10.Models;
using SoftwarePractice_10.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoftwarePractice_10.Presenters
{
    class PostPresenter
    {
        private MainWindow _mainWindow;
        private UnitOfWork _uof;
        public PostPresenter(MainWindow mainwindow)
        {
            _mainWindow = mainwindow;
            _uof = new UnitOfWork();

            _mainWindow.PostActor_Submit.Click += PostActor_Submit_Click;

        }

        private void PostActor_Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = new MainActor
                {
                    FirstName = _mainWindow.PostActor_FirstName_TextBox.Text,
                    LastName = _mainWindow.PostActor_LastName_TextBox.Text,
                    Age = Byte.Parse(_mainWindow.PostActor_AgeTextBox.Text)
                };
                if (Helper.IsModelValid(model))
                {
                    _uof.Actors.Insert(model);
                    _uof.Commit();
                    return;
                }

                throw new Exception();
            }
            catch (Exception)
            {
                //MessageBox.Show("Enter approptiate values to the fields!");
                throw;
            }
        }
        
    }

    static class Helper
    {
        public static bool IsModelValid(Unit model)
        {
            if (model as MainActor != null)
            {
                var actorModel = model as MainActor;

                if (actorModel.FirstName != null && actorModel.FirstName.Length >= 2
                    && actorModel.LastName != null && actorModel.LastName.Length >= 2
                    && actorModel.Age > 1 && actorModel.Age <= 110)
                {
                    return true;
                }

            }

            return false;
        }

    }
}
