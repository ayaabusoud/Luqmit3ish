using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class OnBoardingViewModel :  INotifyPropertyChanged
    {
    public INavigation Navigation { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand ButtonCommand { protected set; get; }

    public OnBoardingViewModel(INavigation navigation)
    {
        this.Navigation = navigation;
        ButtonCommand = new Command(async () => await OnButtonClicked());
    }

    private async Task OnButtonClicked()
    {
        await Navigation.PushModalAsync(new SignupPage());
    }

}
}
