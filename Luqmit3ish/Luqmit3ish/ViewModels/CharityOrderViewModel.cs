using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CharityOrderViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand EditCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand OrderCommand { protected set; get; }
        private readonly string _deleteAlertTitle = "Delete Order";
        private readonly string _successDeleteMessage = "The order have been deleted successfully.";
        private readonly string _failDeleteMessage = "The Order has not been deleted , please try again.";
        private readonly string _confirmDeleteMessage = "Are you sure that you want to delete this Order?";
        
        private readonly IOrderService _orderService;

        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }
 
        public CharityOrderViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            OrderCommand = new Command<OrderCard>(async (OrderCard order) => await OnFrameClicked(order));
            _orderService = new OrderService();
            OnInit();
        }

        private async Task OnFrameClicked(OrderCard order)
        {
            try
            {
                await _navigation.PushAsync(new  OrderDetailsPage(order));

            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        } 

        private async Task OnDeleteClicked(int restaurantId)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert(_deleteAlertTitle,
                _confirmDeleteMessage, "Confirm", "Cnacel");
            if (deleteConfirm)
            {
                var userId = GetUserId();
                try
                {
                    bool result =  await _orderService.DeleteOrder(userId, restaurantId);
                    if(result == true)
                    {
                        OnInit();
                        await PopNavigationAsync(_successDeleteMessage);

                    }
                    else
                    {
                        await PopNavigationAsync(_failDeleteMessage);
                    }
                }
                catch (ConnectionException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(InternetMessage);
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(HttpRequestMessage);
                }
                catch (NotAuthorizedException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(NotAuthorizedMessage);
                }
                catch (Exception e)
                {

                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(ExceptionMessage);
                }
            }

        }

        private ObservableCollection<OrderCard> _orderCards;

        public ObservableCollection<OrderCard> OrderCards
        {
            get => _orderCards;
            set => SetProperty(ref _orderCards, value);
        }

        private  void OnInit()
        {
            try
            {


                Task.Run(async () => {
                    var userId = GetUserId();
                    try
                    {
                        OrderCards = await _orderService.GetOrders(userId);
                    }
                    catch (ConnectionException e)
                    {
                        Debug.WriteLine(e.Message);
                        await PopNavigationAsync(InternetMessage);
                    }
                    catch (HttpRequestException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    catch (NotAuthorizedException e)
                    {
                        Debug.WriteLine(e.Message);
                        EndSession();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    if (OrderCards.Count > 0)
                    {
                        EmptyResult = false;
                    }
                    else
                    {
                        EmptyResult = true;
                    }

                }).Wait();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
        }


       
    }
}
