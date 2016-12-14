using SoftwarePractice_10.CustomControls;
using SoftwarePractice_10.CustomControls.DialogWindows;
using SoftwarePractice_10.Models;
using SoftwarePractice_10.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SoftwarePractice_10.Presenters
{
    class UpdatePagePresenter
    {
        private string _currentTabHeader;
        private readonly UpdatePage _updatePage;
        private readonly UnitOfWork _uof;
        private readonly decimal _priceCoef = 15M;

        public UpdatePagePresenter(UpdatePage postpage)
        {
            _uof = new UnitOfWork();
            _updatePage = postpage;


            _updatePage.updateActor_Submit_Button.Click += UpdateActor_Submit_Button_Click;
            _updatePage.updateFilm_Submit_Button.Click += UpdateFilm_Submit_Button_Click;
            _updatePage.updateUser_Submit_Button.Click += UpdateUser_Submit_Button_Click;

            _updatePage.UpdateTabControl.SelectionChanged += UpdateTabControl_SelectionChanged;

            #region EvenHandlerCorrection
            //handler correctors
            _updatePage.updateActor_Films_ListBox.SelectionChanged += UpdateActor_Films_ListBox_SelectionChanged;
            _updatePage.updateFilm_MainActors_ListBox.SelectionChanged += UpdateFilm_MainActors_ListBox_SelectionChanged;
            _updatePage.updateUser_TakenFilms_ListBox.SelectionChanged += UpdateUser_TakenFilms_ListBox_SelectionChanged;
            _updatePage.updateFilm_ratingCBox.SelectionChanged += UpdateFilm_ratingCBox_SelectionChanged;
            #endregion

            //context menu
            MenuItem chooseMI = new MenuItem { Header = "Choose", Icon = new Image { Source = new BitmapImage(new Uri(@"D:\PROGRAMMING\Програмне забезпечення\SoftwarePractice_10\SoftwarePractice_10\AddtitionalContent\push-512.png", UriKind.Relative)) } };
            chooseMI.Click += ChooseMenuItem_Click;

            MenuItem clearValues = new MenuItem { Header = "Exit" };
            clearValues.Click += ClearValuesMenuItem_Click;

            _updatePage.ContextMenu = new ContextMenu();
            _updatePage.ContextMenu.Items.Add(chooseMI);
            _updatePage.ContextMenu.Items.Add(clearValues);

        }

        private void ClearValuesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(_updatePage).Close();
        }

        private void ChooseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _updatePage.updateActor_Films_ListBox.Items.Clear();
            _updatePage.updateUser_TakenFilms_ListBox.Items.Clear();
            _updatePage.updateFilm_MainActors_ListBox.Items.Clear();

            try
            {
                SetPreviousValuesByName(_currentTabHeader);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void UpdateTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string TabHeader = ((sender as TabControl).SelectedItem as TabItem).Header.ToString();
            _currentTabHeader = TabHeader;
            _updatePage.updateActor_Films_ListBox.Items.Clear();
            _updatePage.updateUser_TakenFilms_ListBox.Items.Clear();
            _updatePage.updateFilm_MainActors_ListBox.Items.Clear();
            _updatePage.choosen_unitId.Text = "";

            try
            {
                SetPreviousValuesByName(TabHeader);
            }
            catch (Exception exc)
            {
                Debug.Write(exc.Message);
            }

        }

        #region Fixes
        private void UpdateFilm_ratingCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void UpdateUser_TakenFilms_ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void UpdateFilm_MainActors_ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void UpdateActor_Films_ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        private void UpdateUser_Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var queryUser = _uof.Users.Get(x => x.FirstName == _updatePage.updateUser_FirstName_TextBox.Text
                //                           && x.LastName == _updatePage.updateUser_LastName_TextBox.Text).First();
                //var queryUser = _uof.Users.GetById(int.Parse(_updatePage.choosen_unitId.Text));
                int id = int.Parse(_updatePage.choosen_unitId.Text);
                var queryUser = _uof.Users.Get(x => x.Id == id, null, "TakenFilms").First();

                var queryContactInfo = _uof.ContactInfos.GetById(queryUser.Id);


                if (!string.IsNullOrEmpty(_updatePage.updateUser_FirstName_TextBox.Text))
                {
                    queryUser.FirstName = _updatePage.updateUser_FirstName_TextBox.Text;
                }

                if (!string.IsNullOrEmpty(_updatePage.updateUser_LastName_TextBox.Text))
                {
                    queryUser.LastName = _updatePage.updateUser_LastName_TextBox.Text;
                }

                if (!string.IsNullOrEmpty(_updatePage.updateUser_Email_TextBox.Text))
                {
                    queryContactInfo.Email = _updatePage.updateUser_Email_TextBox.Text;
                }

                if (!string.IsNullOrEmpty(_updatePage.updateUser_Adress_TextBox.Text))
                {
                    queryContactInfo.Adress = _updatePage.updateUser_Adress_TextBox.Text;
                }
                if (!string.IsNullOrEmpty(_updatePage.updateUser_Phone_TextBox.Text))
                {
                    queryContactInfo.Phone = _updatePage.updateUser_Phone_TextBox.Text;
                }

                if (_updatePage.updateUser_TakenFilms_ListBox.SelectedItems.Count == 0)
                {
                    queryUser.TakenFilms = new HashSet<Film>();
                }
                else
                {
                    var resSet = new HashSet<Film>();
                    var selectedFilmNames = _updatePage.updateUser_TakenFilms_ListBox.SelectedItems.OfType<string>().ToList();

                    foreach (var item in selectedFilmNames)
                    {
                        resSet.Add(_uof.Films.Get(x => x.Name == item).First());
                    }
                    queryUser.TakenFilms = resSet;
                }


                if (_updatePage.updateUser_returnDate_DatePicker.SelectedDate != null)
                {
                    queryUser.ReturnDate = _updatePage.updateUser_returnDate_DatePicker.SelectedDate;
                }
                queryUser.MoneyToPay = queryUser.TakenFilms.Count * _priceCoef;
                queryContactInfo.User = queryUser;

                if (Helpers.PostHelper.IsModelValid(queryUser) && Helpers.PostHelper.IsModelValid(queryContactInfo))
                {
                    _uof.ContactInfos.Update(queryContactInfo);
                    _uof.Users.Update(queryUser);
                    _uof.Commit();

                    MessageBox.Show("Succes!");
                    return;
                }

                throw new Exception();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                MessageBox.Show("Error! Make sure, that you are filling correct values to fields!");
            }

        }

        private void UpdateFilm_Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var queryFilm = _uof.Films.GetById(int.Parse(_updatePage.choosen_unitId.Text));

                if (!string.IsNullOrEmpty(_updatePage.updateFilm_Name_TextBox.Text))
                {
                    queryFilm.Name = _updatePage.updateFilm_Name_TextBox.Text;
                }
                if (!string.IsNullOrEmpty(_updatePage.updateFilm_Studio_TextBox.Text))
                {
                    queryFilm.Studio = _updatePage.updateFilm_Studio_TextBox.Text;
                }
                if (_updatePage.updateFilm_SetDOR_DatePicker.SelectedDate != null)
                {
                    queryFilm.DateOfRelease = _updatePage.updateFilm_SetDOR_DatePicker.SelectedDate ?? DateTime.Now;
                }
                if (!string.IsNullOrEmpty(_updatePage.updateFilm_AvailbleAmount_TextBox.Text))
                {
                    queryFilm.AmountOfAvailableExemplars = int.Parse(_updatePage.updateFilm_AvailbleAmount_TextBox.Text);
                }
                if (!string.IsNullOrEmpty(_updatePage.updateFilm_Director_TextBox.Text))
                {
                    queryFilm.AmountOfAvailableExemplars = int.Parse(_updatePage.updateFilm_AvailbleAmount_TextBox.Text);
                }
                if (!string.IsNullOrEmpty(_updatePage.updateFilm_NumOfReleased_TextBox.Text))
                {
                    queryFilm.AmountOfReleasedExemplars = int.Parse(_updatePage.updateFilm_NumOfReleased_TextBox.Text);
                }


                if (_updatePage.updateFilm_MainActors_ListBox.SelectedItems.Count == 0)
                {
                    queryFilm.MainActors = new HashSet<MainActor>();
                }
                else
                {
                    var newCol = new HashSet<MainActor>();
                    var selectedActors = _updatePage.updateFilm_MainActors_ListBox.SelectedItems.OfType<string>().ToList();

                    foreach (var item in selectedActors)
                    {
                        string [] fName_lName = item.Split(' ');
                        string fName = fName_lName[0];
                        string lName = fName_lName[1];
                        newCol.Add(_uof.Actors.Get(x => (x.FirstName == fName) && (x.LastName == lName)).First());
                    }
                    queryFilm.MainActors = newCol;
                }

                var rTb = _updatePage.updateFilm_Summary_RichTextBox;
                if (!string.IsNullOrEmpty(new TextRange(rTb.Document.ContentStart, rTb.Document.ContentEnd).Text))
                {
                    //rTb.Document.Blocks.Add(new Paragraph(new Run(new TextRange(rTb.Document.ContentStart, rTb.Document.ContentEnd).Text)));
                    queryFilm.Summary = new TextRange(rTb.Document.ContentStart, rTb.Document.ContentEnd).Text;
                }

                queryFilm.Rating = byte.Parse((_updatePage.updateFilm_ratingCBox.SelectedIndex + 1).ToString());
                if (Helpers.PostHelper.IsModelValid(queryFilm))
                {
                    _uof.Films.Update(queryFilm);
                    _uof.Commit();

                    MessageBox.Show("Succes!");
                    return;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error occured! Please, check whether data is valid!", "Error :(", MessageBoxButton.OK);
            }

            MessageBox.Show("Error occured! Please, check whether data is valid!", "Error :(", MessageBoxButton.OK);
        }

        private void UpdateActor_Submit_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var queryActor = _uof.Actors.GetById(int.Parse(_updatePage.choosen_unitId.Text));

            if (!string.IsNullOrEmpty(_updatePage.updateActor_FirstName_TextBox.Text))
            {
                queryActor.FirstName = _updatePage.updateActor_FirstName_TextBox.Text;
            }
            if (!string.IsNullOrEmpty(_updatePage.updateActor_LastName_TextBox.Text))
            {
                queryActor.LastName = _updatePage.updateActor_LastName_TextBox.Text;
            }
            if (_updatePage.updateActor_SetDOB_DatePicker.SelectedDate != null)
            {
                queryActor.Age = (byte)(((DateTime.Now - _updatePage.updateActor_SetDOB_DatePicker.SelectedDate).Value.Days) / 365);
            }
            if (_updatePage.updateActor_Films_ListBox.SelectedItems.Count == 0)
            {
                queryActor.Films = new HashSet<Film>();
            }
            else
            {
                var choosen = _updatePage.updateActor_Films_ListBox.SelectedItems.OfType<string>();
                var newSet = new HashSet<Film>();
                foreach (var item in choosen)
                {
                    newSet.Add(_uof.Films.Get(x => x.Name == item).First());
                }
                queryActor.Films = newSet;
            }


            if (Helpers.PostHelper.IsModelValid(queryActor))
            {
                _uof.Actors.Update(queryActor);
                _uof.Commit();
                MessageBox.Show("Succeded!");
            }
        }

        private async Task<string> GetNameByPrompt(IEnumerable<string> list)
        {
            //here we choose which item we will update
            var prompt = new Prompt(list);

            var task = Dispatcher.CurrentDispatcher.BeginInvoke(new Func<bool?>(prompt.ShowDialog));
            await task;

            if (task.Result != null && (bool)task.Result)
            {
                return prompt.Value;
            }

            throw new Exception();
        }


        private async void SetPreviousValuesByName(string EntityType)
        {
            string stringifiedItem;
            string stringifiedActor;
            string userStringified;

            switch (EntityType)
            {
                case "Film":
                    try
                    {
                        stringifiedItem = await GetNameByPrompt(_uof.Films.Get().Select(x => x.Name).ToList());
                        if (stringifiedItem == "Exception")
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please, choose item to be changed:\n You can call context menu or chage tab for this purpose.", "Warning!");
                        return;
                    }

                    var entity = _uof.Films.Get(x => x.Name == stringifiedItem, null, "MainActors").First();

                    foreach (var item in _uof.Actors.Get().ToList())
                    {
                        if (entity.MainActors.Contains(item))
                        {
                            _updatePage.updateFilm_MainActors_ListBox.SelectedItems.Add(item.FirstName + " " + item.LastName);
                        }

                        _updatePage.updateFilm_MainActors_ListBox.Items.Add(item.FirstName + " " + item.LastName);
                    }

                    #region Here we set values to controls
                    _updatePage.choosen_unitId.Text = entity.Id.ToString();
                    _updatePage.updateFilm_Name_TextBox.Text = entity.Name;
                    _updatePage.updateFilm_Director_TextBox.Text = entity.Director;
                    _updatePage.updateFilm_Studio_TextBox.Text = entity.Studio;
                    _updatePage.updateFilm_SetDOR_DatePicker.DisplayDate = entity.DateOfRelease;
                    _updatePage.updateFilm_AvailbleAmount_TextBox.Text = entity.AmountOfAvailableExemplars.ToString();
                    _updatePage.updateFilm_NumOfReleased_TextBox.Text = entity.AmountOfReleasedExemplars.ToString();
                    _updatePage.updateFilm_ratingCBox.SelectedIndex = entity.Rating ?? 0;


                    //TODO solve proble with appending text
                    Helpers.PostHelper.ClearTextInRichTextBox(_updatePage.updateFilm_Summary_RichTextBox);
                    _updatePage.updateFilm_Summary_RichTextBox.AppendText(entity.Summary);
                    #endregion

                    break;

                case "Actor":

                    try
                    {
                        stringifiedActor = await GetNameByPrompt(_uof.Actors.Get().Select(x => x.FirstName + " " + x.LastName).ToList());
                        if (stringifiedActor == "Exception")
                        {
                            throw new NullReferenceException();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please, choose item to be changed:\n You can call context menu or chage tab for this purpose.", "Warning!");
                        return;
                    }

                    var choosenActor = _uof.Actors.Get(x => (x.FirstName + " " + x.LastName) == stringifiedActor, null, "Films").First();

                    foreach (var item in _uof.Films.Get().ToList())
                    {
                        if (choosenActor.Films.Contains(item))
                        {
                            _updatePage.updateActor_Films_ListBox.SelectedItems.Add(item.Name);
                        }

                        _updatePage.updateActor_Films_ListBox.Items.Add(item.Name);
                    }

                    #region Here we set values to controls
                    _updatePage.choosen_unitId.Text = choosenActor.Id.ToString();
                    _updatePage.updateActor_FirstName_TextBox.Text = choosenActor.FirstName;
                    _updatePage.updateActor_LastName_TextBox.Text = choosenActor.LastName;
                    _updatePage.updateActor_SetDOB_DatePicker.DisplayDate =
                        DateTime.Parse("01/01/" + (DateTime.Now.Year - choosenActor.Age).ToString());
                    #endregion

                    break;
                case "User":
                    try
                    {
                        userStringified = await GetNameByPrompt(_uof.Users.Get()
                            .Select(x => x.FirstName + " " + x.LastName).ToList());
                        if (userStringified == "Exception")
                        {
                            MessageBox.Show("Please, choose item to be changed:\n You can call context menu or chage tab for this purpose.", "Warning!");
                            throw new NullReferenceException();
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    string[] fname_lname = userStringified.Split(' ');

                    string fName = fname_lname[0];
                    string lName = fname_lname[1];

                    var user = _uof.Users.Get(x => x.FirstName == fName && x.LastName == lName, null, "TakenFilms").First();
                    var userContacts = _uof.ContactInfos.GetById(user.Id);

                    foreach (var item in _uof.Films.Get().ToList())
                    {
                        if (user.TakenFilms.Contains(item))
                        {
                            _updatePage.updateUser_TakenFilms_ListBox.SelectedItems.Add(item.Name);
                        }
                        _updatePage.updateUser_TakenFilms_ListBox.Items.Add(item.Name);
                    }

                    #region Here we set values to controls
                    _updatePage.choosen_unitId.Text = userContacts.Id.ToString();
                    _updatePage.updateUser_Adress_TextBox.Text = userContacts.Adress;
                    _updatePage.updateUser_Email_TextBox.Text = userContacts.Email;
                    _updatePage.updateUser_Phone_TextBox.Text = userContacts.Phone;

                    _updatePage.updateUser_FirstName_TextBox.Text = user.FirstName;
                    _updatePage.updateUser_LastName_TextBox.Text = user.LastName;
                    _updatePage.updateUser_returnDate_DatePicker.DisplayDate = user.ReturnDate ?? DateTime.Now;

                    userContacts.User = user;
                    #endregion
                    break;
                default:
                    break;
            }
        }

    }
}
