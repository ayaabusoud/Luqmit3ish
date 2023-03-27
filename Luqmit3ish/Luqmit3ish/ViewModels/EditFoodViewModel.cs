using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class EditFoodViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SubmitCommand { protected set; get; }

        public EditFoodViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SubmitCommand = new Command(async () => await OnSubmitClicked());
        }
        private async Task OnSubmitClicked()
        {
            await Navigation.PopAsync();
        }
    }
}
