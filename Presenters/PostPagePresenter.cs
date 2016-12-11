using SoftwarePractice_10.CustomControls;
using SoftwarePractice_10.Helpers;
using SoftwarePractice_10.Models;
using SoftwarePractice_10.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SoftwarePractice_10.Presenters
{
    public class PostPresenter
    {
        private PostPage _postPage;
        private UnitOfWork _uof;

        public PostPresenter(PostPage postpage)
        {
            _postPage = postpage;
            _uof = new UnitOfWork();

            //submits handlers assignment
            _postPage.postActor_Submit_Button.Click += PostActor_Submit_Button_Click;
            _postPage.postFilm_Submit_Button.Click += PostFilm_Submit_Button_Click;
            _postPage.postUser_Submit_Button.Click += PostUser_Submit_Button_Click;

            //additional helper events
            _postPage.PostTabControl.SelectionChanged += PostTabControl_SelectionChanged;

            #region EvenHandlerCorrection
            //handler correctors
            _postPage.postActor_Films_ListBox.SelectionChanged += PostActor_Films_ListBox_SelectionChanged;
            _postPage.postFilm_MainActors_ListBox.SelectionChanged += PostFilm_MainActors_ListBox_SelectionChanged;
            _postPage.postUser_TakenFilms_ListBox.SelectionChanged += PostUser_TakenFilms_ListBox_SelectionChanged;
            #endregion
        }

        private void PostUser_Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            var takenFilmNames = _postPage.postUser_TakenFilms_ListBox.SelectedItems.OfType<string>();
            HashSet<Film> takenFilms = new HashSet<Film>();

            foreach (var item in takenFilmNames)
            {
                takenFilms.Add(_uof.Films.Get().Where(x => x.Name == item).First());
            }

            try
            {
                var user = new User
                {
                    FirstName = _postPage.postUser_FirstName_TextBox.Text,
                    LastName = _postPage.postUser_LastName_TextBox.Text,
                    TakenFilms = takenFilms,

                    //TODO make setter for coef of multiplication for MoneyToPay calculation
                    MoneyToPay = takenFilms.Count * 10,
                    ReturnDate = _postPage.postUser_returnDate_DatePicker.DisplayDate
                };

                var contactInfo = new ContactInfo
                {
                    Adress = _postPage.postUser_Adress_TextBox.Text,
                    Email = _postPage.postUser_Email_TextBox.Text,
                    User = user,
                    Phone = _postPage.postUser_Phone_TextBox.Text
                };

                if (PostHelper.IsModelValid(user) && PostHelper.IsModelValid(contactInfo))
                {
                    _uof.ContactInfos.Insert(contactInfo);
                    _uof.Commit();

                    PostHelper.ShowSuccesMessage("Operation succeded!");
                    return;
                }


                throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Error.Please, check if fields are written up correcrly.");
            }
        }

        private void PostFilm_Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedActorFullNames = _postPage.postFilm_MainActors_ListBox.SelectedItems.OfType<string>().ToList();
            HashSet<MainActor> selectedActors = new HashSet<MainActor>();

            foreach (var item in selectedActorFullNames)
            {
                string[] fName_lName = item.Split(' ');
                string firstName = fName_lName[0];
                string lastName = fName_lName[1];
                selectedActors.Add(_uof.Actors.Get().Where(x => x.FirstName == firstName && x.LastName == lastName).First());
            }

            try
            {
                var model = new Film
                {
                    Name = _postPage.postFilm_Name_TextBox.Text,
                    Studio = _postPage.postFilm_Studio_TextBox.Text,
                    DateOfRelease = _postPage.postFilm_SetDOR_DatePicker.SelectedDate ?? DateTime.Now,
                    Director = _postPage.postFilm_Director_TextBox.Text,
                    Summary = PostHelper.GetTextFromRichTextBox(_postPage.postFilm_Summary_RichTextBox),
                    MainActors = selectedActors,
                    Rating = 0,
                    AmountOfAvailableExemplars = Int32.Parse(_postPage.postFilm_AvailbleAmount_TextBox.Text),
                    AmountOfReleasedExemplars = Int32.Parse(_postPage.postFilm_NumOfReleased_TextBox.Text)
                };

                if (PostHelper.IsModelValid(model))
                {
                    _uof.Films.Insert(model);
                    _uof.Commit();

                    PostHelper.ShowSuccesMessage();
                    return;
                }

                throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Error.Please, check if fields are written up correcrly.");
            }
        }

        private void PostTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is TabControl))
                return;
            //Debug.WriteLine(((sender as TabControl).SelectedItem as TabItem).Header);
            string TabHeader = ((sender as TabControl).SelectedItem as TabItem).Header.ToString();

            _postPage.postActor_Films_ListBox.Items.Clear();
            _postPage.postUser_TakenFilms_ListBox.Items.Clear();
            _postPage.postFilm_MainActors_ListBox.Items.Clear();

            switch (TabHeader)
            {
                case "Film":
                    foreach (var item in _uof.Actors.Get())
                    {
                        _postPage.postFilm_MainActors_ListBox.Items.Add(item.FirstName + " " + item.LastName);
                    }
                    break;

                case "Actor":
                    foreach (var item in _uof.Films.Get())
                    {
                        _postPage.postActor_Films_ListBox.Items.Add(item.Name);
                    }
                    break;
                case "User":
                    foreach (var item in _uof.Films.Get())
                    {
                        _postPage.postUser_TakenFilms_ListBox.Items.Add(item.Name);
                    }
                    break;
                default:
                    break;
            }
        }

        private void PostActor_Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilmNames = _postPage.postActor_Films_ListBox.SelectedItems.OfType<string>().ToList();
            HashSet<Film> selectedFilms = new HashSet<Film>();

            foreach (var item in selectedFilmNames)
            {
                selectedFilms.Add(_uof.Films.Get().Where(x => x.Name == item).First());
            }

            try
            {
                var model = new MainActor()
                {
                    FirstName = _postPage.postActor_FirstName_TextBox.Text,
                    LastName = _postPage.postActor_LastName_TextBox.Text,
                    Age = (byte)(((DateTime.Now - _postPage.postActor_SetDOB_DatePicker.SelectedDate).Value.Days) / 365),
                    Films = selectedFilms
                };

                if (PostHelper.IsModelValid(model))
                {
                    _uof.Actors.Insert(model);
                    _uof.Commit();

                    PostHelper.ShowSuccesMessage();
                    return;
                }

                throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Error.Please, check if fields are written up correcrly.");
            }
        }



        //private void PostActor_Submit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var model = new MainActor
        //        {
        //            FirstName = _mainWindow.PostActor_FirstName_TextBox.Text,
        //            LastName = _mainWindow.PostActor_LastName_TextBox.Text,
        //            Age = Byte.Parse(_mainWindow.PostActor_AgeTextBox.Text)
        //        };
        //        if (Helper.IsModelValid(model))
        //        {
        //            _uof.Actors.Insert(model);
        //            _uof.Commit();
        //            return;
        //        }

        //        throw new Exception();
        //    }
        //    catch (Exception)
        //    {
        //        //MessageBox.Show("Enter approptiate values to the fields!");
        //        throw;
        //    }
        //}

        #region EventHandlerCorrections

        //The TabControl.SelectionChanged is the same event as a ComboBox.SelectionChanged
        //so we need to apply this fixes to let it work correct
        private void PostUser_TakenFilms_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void PostFilm_MainActors_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void PostActor_Films_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion
    }


}
