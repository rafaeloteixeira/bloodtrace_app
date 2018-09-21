using BloodTrace.Models;
using BloodTrace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BloodTrace.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindBloodGroup : ContentPage
    {
        List<Estado> listaUF = null;
        List<Cidade> listaCidade = null;

        public FindBloodGroup ()
		{
			InitializeComponent ();

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
                await DisplayAlert("Atenção", "Alguma coisa deu errado...", "Cancelar");
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
                await DisplayAlert("Atenção", "Alguma coisa deu errado...", "Cancelar");
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
        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            var bloodGroup = PickerBloodGroup.Items[PickerBloodGroup.SelectedIndex].Trim().Replace("+", "%2B");
            var uf = PickerUF.Items[PickerUF.SelectedIndex];
            var cidade = PickerCity.Items[PickerCity.SelectedIndex].Trim().Replace(" ", "%20");

            Navigation.PushAsync(new DonorsListPage(cidade,uf, bloodGroup));
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