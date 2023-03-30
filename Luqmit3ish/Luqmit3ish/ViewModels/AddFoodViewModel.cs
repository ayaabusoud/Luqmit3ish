using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class AddFoodViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SubmitCommand { protected set; get; }

        public AddFoodViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SubmitCommand = new Command(async () => await OnSubmitClicked());
        }
        private async Task OnSubmitClicked()
        {
            try
            {
               await Navigation.PopAsync();

            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
