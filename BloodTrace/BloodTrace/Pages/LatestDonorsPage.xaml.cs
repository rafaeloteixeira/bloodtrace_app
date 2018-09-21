using BloodTrace.Models;
using BloodTrace.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BloodTrace.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LatestDonorsPage : ContentPage
	{
        public ObservableCollection<BloodUser> BloodUsers;

        public LatestDonorsPage ()
		{
			InitializeComponent ();
            BloodUsers = new ObservableCollection<BloodUser>();
            FindBloodDonors();
        }

        private async void FindBloodDonors()
        {
            showLoading(true);

            ApiServices apiServices = new ApiServices();
            var bloodUsers = await apiServices.LatestBloodUser();
            foreach (var bloodUser in bloodUsers)
            {
                BloodUsers.Add(bloodUser);
            }

            LvBloodDonors.ItemsSource = BloodUsers;

            showLoading(false);
        }
        private void showLoading(bool value)
        {
            actInd.IsVisible = value;
            actInd.IsRunning = value;
        }

        private void LvBloodDonors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedDonor = e.SelectedItem as BloodUser;
            Navigation.PushAsync(new DonorProfilePage(selectedDonor));
        }
    }
}