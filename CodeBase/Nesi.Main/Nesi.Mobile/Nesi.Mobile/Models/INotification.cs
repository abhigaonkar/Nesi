using System;
using System.Collections.Generic;
using System.Text;

namespace Nesi.Mobile.Models
{
   public interface INotification
    {

        void ShowNotification(string Title, string Text);
        void CancelNotification();
    }
}
