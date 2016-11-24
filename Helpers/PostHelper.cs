using SoftwarePractice_10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SoftwarePractice_10.Helpers
{
    public static class PostHelper
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

            if (model as Film != null)
            {
                var film = model as Film;
                if (film.Name != null && film.Name.Length <= 20
                    && film.AmountOfAvailableExemplars >= 1 && film.AmountOfReleasedExemplars >= 1
                    && film.AmountOfAvailableExemplars <= film.AmountOfReleasedExemplars)
                {
                    return true;
                }
            }

            if(model as User != null)
            {
                var user = model as User;
                if (user.FirstName != null & user.LastName != null
                    && user.FirstName.Length <= 20 && user.LastName.Length <= 20)
                {
                    return true;
                }
            }

            if (model as ContactInfo != null)
            {
                Regex phoneRegex = new Regex(@"^\d{7, 15}$");
                Regex mailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                var conInf = model as ContactInfo;
                if (phoneRegex.IsMatch(conInf.Email)
                    && conInf.User != null && conInf.Adress.Length <= 25)
                {
                    return true;
                }
            }


            return false;
        }

        public static string GetTextFromRichTextBox(RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
        }

        public static void ClearTextInRichTextBox(RichTextBox rtb)
        {
            rtb.Document.Blocks.Clear();
        }

        public static void ShowSuccesMessage(string customizedMessage = "")
        {
            if (string.IsNullOrEmpty(customizedMessage))
            {
                MessageBox.Show("Operation succeded!");
                return;
            }

            MessageBox.Show(customizedMessage);
        }

    }
}
