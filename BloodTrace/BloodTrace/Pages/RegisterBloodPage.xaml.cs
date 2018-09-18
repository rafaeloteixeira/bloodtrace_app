using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodTrace.Helper;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using BloodTrace.Models;
using BloodTrace.Services;

namespace BloodTrace.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterBloodPage : ContentPage
	{

        public MediaFile file;

		public RegisterBloodPage ()
		{
			InitializeComponent ();
		}

        private async void TapOpenCamera_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            ImgDonor.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            var imageArray = FilesHelper.ReadFully(file.GetStream());
            file.Dispose();
            var country = PickerCountry.Items[PickerCountry.SelectedIndex];
            var bloodGroup = PickerBloodGroup.Items[PickerBloodGroup.SelectedIndex];

            DateTime dateTime = DateTime.Now;
            int d = Convert.ToInt32(dateTime.ToOADate());

            var bloodUser = new BloodUser()
            {
                UserName = EntName.Text,
                Email = EntEmail.Text,
                Phone = EntPhone.Text,
                BloodGroup = bloodGroup,
                City = country,
                State = country,
                ImageArray = imageArray,
                Date = d
            };

            ApiServices apiServies = new ApiServices();
            var response = await apiServies.RegisterDonor(bloodUser);
            if (!response)
            {
                await DisplayAlert("Alert", "Somthing wrong...", "Cancel");
            }
            else
            {
                await DisplayAlert("Hi", "Your record has been added successfully", "OK");
            }
        }
    }
}