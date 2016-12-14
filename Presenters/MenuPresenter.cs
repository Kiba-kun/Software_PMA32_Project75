using SoftwarePractice_10.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePractice_10.Presenters
{
    class MenuPresenter
    {
        private readonly MainWindow _mainWindow;

        private readonly UpdatePage _updPage;
        private readonly PostPage _postPage;
        private readonly GetInfoPage _getInfoPage;

        public MenuPresenter(MainWindow mw)
        {
            _mainWindow = mw;

            _updPage = new UpdatePage();
            new UpdatePagePresenter(_updPage);

            _postPage = new PostPage();
            new PostPresenter(_postPage);

            _getInfoPage = new GetInfoPage();
            new GetInfoPresenter(_getInfoPage);

            _mainWindow.Menu_PostButton.Click += Menu_PostButton_Click;
            _mainWindow.Menu_GetDataButton.Click += Menu_GetDataButton_Click;
            _mainWindow.Menu_UpdateButton.Click += Menu_UpdateButton_Click;
            _mainWindow.Menu_DeleteButton.Click += Menu_DeleteButton_Click;

        }

        private void Menu_DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw;
            }
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
