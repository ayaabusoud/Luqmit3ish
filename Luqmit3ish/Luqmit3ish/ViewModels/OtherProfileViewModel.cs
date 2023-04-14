using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Luqmit3ish.ViewModels
{
    class OtherProfileViewModel : ViewModelBase
    {

        private UserServices _userServices;
        private User _user;

        public OtherProfileViewModel(int id)
        {
            _userServices = new UserServices();
            _user = new User();
            _ = OnInit(id);
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
                _user = await _userServices.GetUserById(id);
                Name = _user.Name;
                Location = _user.Location;
                PhoneNumber = _user.Phone;
                Email = _user.Email;
                Photo = _user.Photo;
            }
            catch(ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (HttpRequestException )
            {
                Debug.WriteLine("Something went wrong while viewing the profile, Retrying...");
            }
            catch (ConnectionException)
            {
                Debug.WriteLine("There is no internet connection, please check your connection");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (_user == null)
                {
                   Debug.WriteLine($"User with id {id} not found");
                }
            }
        }

    }
}
