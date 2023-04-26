using System;
using System.Collections.Generic;
using Luqmit3ish.ViewModels;
using Xamarin.Forms;
namespace Luqmit3ish.Views
{	
	public partial class ResetPasswordForgetPage : ContentPage
	{	
		public ResetPasswordForgetPage (string email)
		{
            InitializeComponent();
            this.BindingContext = new ResetPasswordForgetViewModel(Navigation, email);
        }
	}
}
