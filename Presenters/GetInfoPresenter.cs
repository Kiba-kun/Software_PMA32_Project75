using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SoftwarePractice_10.CustomControls;
using SoftwarePractice_10.Models.DataProviders;
using SoftwarePractice_10.CustomControls.QueryPanels;

namespace SoftwarePractice_10.Presenters
{
    internal enum ChoosenSection
    {
        Films,
        Actors,
        Users,
        ContactInfos
    }

    internal class GetInfoPresenter
    {
        private readonly GetInfoPage _infoPage;
        private readonly UnitOfWork _uof = new UnitOfWork();
        private readonly DataGrid _dataGrid;
        private readonly TextBox _searchField;
        private string selectedField = "";

        public GetInfoPresenter(GetInfoPage infoPage)
        {
            _infoPage = infoPage;
            _dataGrid = _infoPage.getData_dataGrid;
            _searchField = _infoPage.searchField_textBox;

            _infoPage.searchButton.Click += SearchButton_Click;
            _infoPage.Exit_Button.Click += Exit_Button_Click;
            _infoPage.ClearGrid_Button.Click += ClearGrid_Button_Click;
            //_infoPage.getData_dataGrid.ItemsSource = _uof.Actors.Get();

            _infoPage.searchIn_comboBox.SelectionChanged += SearchIn_comboBox_SelectionChanged;

            _infoPage.print_Button.Click += Print_Button_Click;
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(_infoPage.getData_dataGrid, "Customized selection");
            }
        }

        private void SearchIn_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabHeader = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            Debug.WriteLine(tabHeader);
            selectedField = tabHeader;
            switch (tabHeader)
            {
                case "Films":
                    _infoPage.selectedQuery_contentControll.Content = new FilmsSelector();
                    break;
                case "Actors":
                    _infoPage.selectedQuery_contentControll.Content = null;
                    break;
                case "Users":
                    _infoPage.selectedQuery_contentControll.Content = new UserQuery();
                    (_infoPage.selectedQuery_contentControll.Content as UserQuery).asc_rB.IsChecked = true;
                    (_infoPage.selectedQuery_contentControll.Content as UserQuery).quantity_cB.SelectedIndex = 0;
                    break;
            }
        }

        private void ClearGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            ClearDataGrid();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(_infoPage).Close();
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            switch (_infoPage.searchIn_comboBox.SelectedIndex)
            {
                case 0:
                    SetUpDataGridFor(ChoosenSection.Films);
                    break;
                case 1:
                    SetUpDataGridFor(ChoosenSection.Actors);
                    break;
                case 2:
                    SetUpDataGridFor(ChoosenSection.Users);
                    break;
                case 3:
                    SetUpDataGridFor(ChoosenSection.ContactInfos);
                    break;
                default:
                    break;
            }
        }

        private void ClearDataGrid()
        {
            _dataGrid.ItemsSource = null;
            _dataGrid.Items.Clear();
            _dataGrid.Columns.Clear();
        }

        private void SetUpDataGridFor(ChoosenSection choosen)
        {
            try
            {
                var style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                var searchTag = _searchField.Text;

                switch (choosen)
                {
                    case ChoosenSection.Films:
                        this.ClearDataGrid();
                        var filmColumns = new List<DataGridColumn>
                        {
                            new DataGridTextColumn{ Header = "Id", Binding = new Binding("Id"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Name", Binding = new Binding("Name"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Studio", Binding = new Binding("Studio"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Director", Binding = new Binding("Director"), MaxWidth = 200, ElementStyle = style },
                            new DataGridTextColumn{ Header = "DateOfRelease", Binding = new Binding("DateOfRelease"), MaxWidth = 200, ElementStyle = style },
                            new DataGridTextColumn {Header = "Actors", Binding = new Binding("Actors"), MaxWidth = 200, ElementStyle = style },
                            new DataGridTextColumn {Header = "Rating", Binding = new Binding("Rating"), MaxWidth = 200, ElementStyle = style },
                            new DataGridTextColumn {Header = "Amount available", Binding = new Binding("Amount"), MaxWidth = 200, ElementStyle = style }

                        };

                        var control = _infoPage.selectedQuery_contentControll.Content as FilmsSelector;
                        int lowerAm = 0, upperAm = int.MaxValue, lowrRating = 1, upperRatin = 5;

                        if (control != null)
                        {
                            if (!int.TryParse(control.amF_tb.Text, out lowerAm))
                                lowerAm = 0;
                            if (!int.TryParse(control.amTo_tb.Text, out upperAm))
                                upperAm = int.MaxValue;
                            if (!int.TryParse(control.ratingF_tb.Text, out lowrRating))
                                lowrRating = 1;
                            if (!int.TryParse(control.ratingTo_tb.Text, out upperRatin))
                                upperRatin = 5;
                        }


                        var query = _uof.Films
                            .Get(x => (x.Name.Contains(searchTag) || x.Studio.Contains(searchTag)
                            || x.Director.Contains(searchTag)
                            || x.MainActors.Select(a => a.FirstName + a.LastName).FirstOrDefault(a => a.Contains(searchTag)) != null)
                            && (x.AmountOfAvailableExemplars >= lowerAm && x.AmountOfAvailableExemplars <= upperAm
                            && x.Rating >= lowrRating && x.Rating <= upperRatin),
                            null, "MainActors")
                            .Select(y => new
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Studio = y.Studio,
                                Director = y.Director,
                                DateOfRelease = y.DateOfRelease,
                                Actors = (y.MainActors.Select(a => a.FirstName + " " + a.LastName + "\n")),
                                Rating = y.Rating ?? 0,
                                Amount = y.AmountOfAvailableExemplars
                            }).ToList();

                        foreach (var item in filmColumns)
                        {
                            _dataGrid.Columns.Add(item);
                        }
                        foreach (var item in query)
                        {
                            var itemToBeAdded = new
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Studio = item.Studio,
                                Director = item.Director,
                                DateOfRelease = item.DateOfRelease,
                                Actors = string.Join("", item.Actors.Select(a => a).ToList()),
                                Rating = item.Rating,
                                Amount = item.Amount
                            };
                            _dataGrid.Items.Add(itemToBeAdded);
                        }
                        break;

                    case ChoosenSection.Actors:
                        this.ClearDataGrid();
                        var actorColumns = new List<DataGridColumn>
                        {
                            new DataGridTextColumn{ Header = "Id", Binding = new Binding("Id"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "First name", Binding = new Binding("FirstName"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Last name", Binding = new Binding("LastName"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Age", Binding = new Binding("Age"), MaxWidth = 200, ElementStyle = style},
                            new DataGridTextColumn{ Header = "Films", Binding = new Binding("Films"), MaxWidth = 200, ElementStyle = style},
                        };
                        var actorsQuery = _uof.Actors
                            .Get(
                                x =>
                                    x.FirstName.Contains(searchTag) || x.LastName.Contains(searchTag) ||
                                    x.Films.Select(f => f.Name).FirstOrDefault(y => y.Contains(searchTag)) != null,
                                null, "Films")
                            .Select(
                                x => new
                                {
                                    Id = x.Id,
                                    FirsName = x.FirstName,
                                    LastName = x.LastName,
                                    Age = x.Age,
                                    Films = x.Films.Select(y => y.Name + "\n")
                                }).ToList();

                        foreach (var item in actorColumns)
                        {
                            _dataGrid.Columns.Add(item);
                        }
                        foreach (var item in actorsQuery)
                        {
                            var itemToBeAdded = new
                            {
                                Id = item.Id,
                                FirstName = item.FirsName,
                                LastName = item.LastName,
                                Age = item.Age,
                                Films = string.Join("", item.Films.Select(a => a).ToList())
                            };
                            _dataGrid.Items.Add(itemToBeAdded);
                        }
                        break;
                    case ChoosenSection.Users:
                        var userControl = _infoPage.selectedQuery_contentControll.Content as UserQuery;

                        int amountOfQueryItems;
                        if (!int.TryParse(userControl.quantity_cB.Text, out amountOfQueryItems))
                        {
                            amountOfQueryItems = 5;
                        }

                        this.ClearDataGrid();
                        var userColumns = new List<DataGridColumn>
                        {
                            new DataGridTextColumn
                            {
                                Header = "Id",
                                Binding = new Binding("Id"),
                                MaxWidth = 200,
                                ElementStyle = style
                            },
                            new DataGridTextColumn
                            {
                                Header = "First name",
                                Binding = new Binding("FirstName"),
                                MaxWidth = 200,
                                ElementStyle = style
                            },
                            new DataGridTextColumn
                            {
                                Header = "Last name",
                                Binding = new Binding("LastName"),
                                MaxWidth = 200,
                                ElementStyle = style
                            },
                            new DataGridTextColumn
                            {
                                Header = "Contacts",
                                Binding = new Binding("Contacts"),
                                MaxWidth = 200,
                                ElementStyle = style
                            },
                            new DataGridTextColumn
                            {
                                Header = "Taken films",
                                Binding = new Binding("TakenFilms"),
                                MaxWidth = 200,
                                ElementStyle = style
                            },
                            new DataGridTextColumn
                            {
                                Header = "Arrears",
                                Binding = new Binding("Arrears"),
                                MaxWidth = 200,
                                ElementStyle = style
                            }
                        };
                        foreach (var item in userColumns)
                        {
                            _dataGrid.Columns.Add(item);
                        }



                        var userQuery =
                            _uof.Users.Get(x => x.FirstName.Contains(searchTag)
                                                || x.LastName.Contains(searchTag) || x.TakenFilms.FirstOrDefault(f => f.Name.Contains(searchTag)) != null,
                                q => ((bool)userControl.asc_rB.IsChecked) ? q.OrderBy(d => d.TakenFilms.Count) : q.OrderByDescending(d => d.TakenFilms.Count),
                                "TakenFilms,Contacts").Select(z => new
                                {
                                    Id = z.Id,
                                    FirstName = z.FirstName,
                                    LastName = z.LastName,
                                    Contacts = z.Contacts,
                                    //TakenFilms = z.TakenFilms.Where(f => f.UsersWithFilm.Contains(z)).Select(f => f.Name),
                                    TakenFilms = z.TakenFilms.Select(f => f.Name),
                                    Arrears = z.MoneyToPay
                                }).Take(Math.Max(0, amountOfQueryItems)).ToList();
                        foreach (var item in userQuery)
                        {
                            _dataGrid.Items.Add(new
                            {
                                Id = item.Id,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                Contacts = "Adress: " + item.Contacts.Adress + "\nEmail: " + item.Contacts.Email + "\nPhone: " + item.Contacts.Phone,
                                TakenFilms = string.Join("\n", item.TakenFilms),
                                Arrears = item.Arrears
                            });
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(choosen), choosen, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
