﻿using Luqmit3ish.Models;
using Luqmit3ish.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodDetailPage : ContentPage
    {
        public FoodDetailPage(DishCard dish)
        {
            InitializeComponent();
            this.BindingContext = new FoodDetailViewModel(dish, Navigation);
        }
    }
}