﻿using System;
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
        //private Presenters.PostPresenter _postPresenter;
        private Presenters.MenuPresenter _menuPresenter;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //Database.SetInitializer(new DatabaseInitializer());
                _menuPresenter = new Presenters.MenuPresenter(this);
            }
            catch (Exception)
            {
                MessageBox.Show("Critical error ocured, please reference to you system administrator");
            }
            //_postPresenter = new Presenters.PostPresenter(this);
        }

    }
}
