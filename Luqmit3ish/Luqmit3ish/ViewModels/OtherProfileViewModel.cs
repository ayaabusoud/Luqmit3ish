using Luqmit3ish.Models;
using Luqmit3ish.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Luqmit3ish.ViewModels
{
    class OtherProfileViewModel : ViewModelBase
    {

        public UserServices userServices;
        public User user;

        public OtherProfileViewModel(int id)
        {
            userServices = new UserServices();
            user = new User();
            OnInit(id);
        }
        
        public OtherProfileViewModel()
        {
            userServices = new UserServices();
            user = new User();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private string _phone;
        public string PhoneNumber
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private string _photo;
        public string Photo
        {
            get => _photo;
            set => SetProperty(ref _photo, value);
        }


        private async Task OnInit(int id)
        {
            try
            {
                user = await userServices.GetUserById(id);
                Name = user.Name;
                Location = user.Location;
                PhoneNumber = user.Phone;
                Email = user.Email;
                Photo = user.Photo;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
