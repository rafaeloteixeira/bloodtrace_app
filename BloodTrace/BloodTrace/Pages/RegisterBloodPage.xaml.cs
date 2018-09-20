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
using System.IO;

namespace BloodTrace.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterBloodPage : ContentPage
    {
        List<Estado> listaUF = null;
        List<Cidade> listaCidade = null;
        public MediaFile file;

        public RegisterBloodPage()
        {
            InitializeComponent();

            loadUFs();
        }

        private async void loadUFs()
        {
            showLoading(true);
            PickerUF.IsEnabled = false;
            ApiServices apiServies = new ApiServices();
            List<Estado> response = await apiServies.ListarUFs();
            if (response == null)
            {
                await DisplayAlert("Alert", "Somthing wrong...", "Cancel");
            }
            else
            {
                listaUF = new List<Estado>(response.OrderBy(x => x.Sigla).ToList());
                PickerUF.ItemsSource = listaUF;
            }
            PickerUF.IsEnabled = true;
            showLoading(false);
        }
        private async void loadCidades(int uf)
        {
            showLoading(true);
            PickerCity.IsEnabled = false;
            ApiServices apiServies = new ApiServices();
            List<Cidade> response = await apiServies.ListarCidades(uf);
            if (response == null)
            {
                await DisplayAlert("Alert", "Somthing wrong...", "Cancel");
            }
            else
            {
                listaCidade = new List<Cidade>(response.OrderBy(x => x.Nome));
                PickerCity.ItemsSource = listaCidade;
            }
            PickerCity.IsEnabled = true;
            showLoading(false);
        }

        private void showLoading(bool value)
        {
            actInd.IsVisible = value;
            actInd.IsRunning = value;
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
            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }
            var uf = PickerUF.Items[PickerUF.SelectedIndex];
            var cidade = PickerCity.Items[PickerCity.SelectedIndex];
            var bloodGroup = PickerBloodGroup.Items[PickerBloodGroup.SelectedIndex];

            DateTime dateTime = DateTime.Now;
            int d = Convert.ToInt32(dateTime.ToOADate());

            var bloodUser = new BloodUser()
            {
                UserName = EntName.Text,
                Email = EntEmail.Text,
                Phone = EntPhone.Text,
                BloodGroup = bloodGroup,
                City = uf,
                State = cidade,
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

        private void PickerUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerUF.SelectedItem is Estado uf)
            {
                loadCidades(uf.Id);
            }
        }
    }
}