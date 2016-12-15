using SoftwarePractice_10.CustomControls;
using SoftwarePractice_10.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoftwarePractice_10.Presenters
{
    class DeletePresenter
    {
        private readonly DeletePage _delPage;
        private readonly UnitOfWork _uof = new UnitOfWork();

        public DeletePresenter(DeletePage deletepage)
        {
            _delPage = deletepage;
            _delPage.delete_button.Click += Delete_button_Click;
        }

        private void Delete_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var id = int.Parse(_delPage.delete_textBox.Text);
                switch (_delPage.comboBox.Text)
                {
                    case "User":
                        _uof.ContactInfos.Delete(id);
                        _uof.Users.Delete(id);
                        break;
                    case "Actor":
                        _uof.Actors.Delete(id);
                        break;
                    case "Film":
                        _uof.Films.Delete(id);
                        break;
                    default:
                        break;
                }
                _uof.Commit();
                MessageBox.Show("Record has been succesfully deleted!");        
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid id! Check value that you are entering!");
            }
        }
    }
}
