using SoftwarePractice_10.CustomControls;
using SoftwarePractice_10.CustomControls.DialogWindows;
using SoftwarePractice_10.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SoftwarePractice_10.Presenters
{
    class UpdatePagePresenter
    {
        private readonly UpdatePage _updatePage;
        private readonly UnitOfWork _uof;

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
            #endregion
        }

        private void UpdateTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string TabHeader = ((sender as TabControl).SelectedItem as TabItem).Header.ToString();

            _updatePage.updateActor_Films_ListBox.Items.Clear();
            _updatePage.updateUser_TakenFilms_ListBox.Items.Clear();
            _updatePage.updateFilm_MainActors_ListBox.Items.Clear();

            SetPreviousValuesByName(TabHeader);
           
        }

        #region Fixes
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
                var queryUser = _uof.Users.Get(x => x.FirstName == _updatePage.updateUser_FirstName_TextBox.Text
                                           && x.LastName == _updatePage.updateUser_LastName_TextBox.Text).First();

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
                throw;
            }

        }

        private void UpdateFilm_Submit_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateActor_Submit_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            switch (EntityType)
            {
                case "Film":

                    stringifiedItem = await GetNameByPrompt(_uof.Films.Get().Select(x => x.Name).ToList());

                    var entity = _uof.Films.Get(x => x.Name == stringifiedItem, null, "MainActors").First();

                    foreach (var item in _uof.Actors.Get().ToList())
                    {
                        if(entity.MainActors.Contains(item))
                        {
                            _updatePage.updateFilm_MainActors_ListBox.SelectedItems.Add(item.FirstName + " " + item.LastName);
                        }

                        _updatePage.updateFilm_MainActors_ListBox.Items.Add(item.FirstName + " " + item.LastName);
                    }

                    #region Here we set values to controls
                    _updatePage.updateFilm_Name_TextBox.Text = entity.Name;
                    _updatePage.updateFilm_Director_TextBox.Text = entity.Director;
                    _updatePage.updateFilm_Studio_TextBox.Text = entity.Studio;
                    _updatePage.updateFilm_SetDOR_DatePicker.DisplayDate = entity.DateOfRelease;
                    _updatePage.updateFilm_AvailbleAmount_TextBox.Text = entity.AmountOfAvailableExemplars.ToString();
                    _updatePage.updateFilm_NumOfReleased_TextBox.Text = entity.AmountOfReleasedExemplars.ToString();


                    //TODO solve proble with appending text
                    Helpers.PostHelper.ClearTextInRichTextBox(_updatePage.updateFilm_Summary_RichTextBox);
                    _updatePage.updateFilm_Summary_RichTextBox.AppendText(entity.Summary);
                    #endregion

                    break;

                case "Actor":
                    var stringified = await GetNameByPrompt(_uof.Actors.Get().Select(x => x.FirstName + " " + x.LastName).ToList());
                    var choosenActor = _uof.Actors.Get(x => (x.FirstName + " " + x.LastName) == stringified, null, "Films").First();

                    foreach (var item in _uof.Films.Get().ToList())
                    {
                        if (choosenActor.Films.Contains(item))
                        {
                            _updatePage.updateActor_Films_ListBox.SelectedItems.Add(item.Name);
                        }

                        _updatePage.updateActor_Films_ListBox.Items.Add(item.Name);
                    }

                    #region Here we set values to controls
                    _updatePage.updateActor_FirstName_TextBox.Text = choosenActor.FirstName;
                    _updatePage.updateActor_LastName_TextBox.Text = choosenActor.LastName;
                    _updatePage.updateActor_SetDOB_DatePicker.DisplayDate = 
                        DateTime.Parse("01/01/" + (DateTime.Now.Year - choosenActor.Age).ToString());
                    #endregion

                    break;
                case "User":
                    string userStringified = await GetNameByPrompt(_uof.Users.Get()
                        .Select(x => x.FirstName + " " + x.LastName).ToList());

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
                    _updatePage.updateUser_Adress_TextBox.Text = userContacts.Adress;
                    _updatePage.updateUser_Email_TextBox.Text = userContacts.Email;
                    _updatePage.updateUser_Phone_TextBox.Text = userContacts.Phone;

                    _updatePage.updateUser_FirstName_TextBox.Text = user.FirstName;
                    _updatePage.updateUser_LastName_TextBox.Text = user.LastName;
                    _updatePage.updateUser_returnDate_DatePicker.DisplayDate = user.ReturnDate??DateTime.Now;

                    userContacts.User = user;
                    #endregion
                    break;
                default:
                    break;
            }
        }

    }
}
