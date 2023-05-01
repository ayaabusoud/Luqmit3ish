using Luqmit3ish.Models;
namespace Luqmit3ish.ViewModels
{
    class OtherProfileViewModel : ViewModelBase
    {

        private User _userInfo;
        public User UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }

        public OtherProfileViewModel(User user)
        {
            _userInfo = user;
        }

    }
}
