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
        private MainWindow _mainWindow;

        public MenuPresenter(MainWindow mw)
        {
            _mainWindow = mw;

            _mainWindow.Menu_PostButton.Click += Menu_PostButton_Click;
            _mainWindow.Menu_GetDataButton.Click += Menu_GetDataButton_Click;
            _mainWindow.Menu_UpdateButton.Click += Menu_UpdateButton_Click;
            _mainWindow.Menu_DeleteButton.Click += Menu_DeleteButton_Click;

        }

        private void Menu_DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu_UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new UpdatePage();
            new UpdatePagePresenter(_mainWindow.contentControl.Content as UpdatePage);
        }

        private void Menu_GetDataButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu_PostButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new PostPage();
            new PostPresenter(_mainWindow.contentControl.Content as PostPage);
        }
    }
}
