using SoftwarePractice_10.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SoftwarePractice_10.Presenters
{
    class MenuPresenter
    {
        private readonly MainWindow _mainWindow;

        private readonly UpdatePage _updPage;
        private readonly PostPage _postPage;
        private readonly GetInfoPage _getInfoPage;
        private readonly DeletePage _deletePage;

        public MenuPresenter(MainWindow mw)
        {
            _mainWindow = mw;

            _updPage = new UpdatePage();
            new UpdatePagePresenter(_updPage);

            _postPage = new PostPage();
            new PostPresenter(_postPage);

            _getInfoPage = new GetInfoPage();
            new GetInfoPresenter(_getInfoPage);

            _deletePage = new DeletePage();
            new DeletePresenter(_deletePage);

            _mainWindow.Menu_PostButton.Click += Menu_PostButton_Click;
            _mainWindow.Menu_GetDataButton.Click += Menu_GetDataButton_Click;
            _mainWindow.Menu_UpdateButton.Click += Menu_UpdateButton_Click;
            _mainWindow.Menu_DeleteButton.Click += Menu_DeleteButton_Click;
            _mainWindow.Menu_DeleteButton.Click += Menu_DeleteButton_Click1;

            var globalMenu = new System.Windows.Controls.ContextMenu();
            var exit = new MenuItem { Header = "Exit" };
            
            exit.Click += Exit_Click;
            globalMenu.Items.Add(exit);

        }

        private void Exit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.Close();
        }

        private void Menu_DeleteButton_Click1(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = _deletePage;
        }

        private void Menu_DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = _deletePage;
        }

        private void Menu_UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = _updPage;
        }

        private void Menu_GetDataButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = _getInfoPage;
        }

        private void Menu_PostButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = _postPage;
        }
    }
}
