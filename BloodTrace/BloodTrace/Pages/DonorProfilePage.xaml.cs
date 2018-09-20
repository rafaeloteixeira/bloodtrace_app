using BloodTrace.Models;
using Plugin.Messaging;
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
	public partial class DonorProfilePage : ContentPage
	{
        private string _email;
        private string _phoneNumber;

		public DonorProfilePage (BloodUser bloodUser)
		{
			InitializeComponent ();
            //ImgDonor.Source = bloodUser.FullLogoPath;
            LblDonorName.Text = bloodUser.UserName;
            LblBloodGroup.Text = bloodUser.BloodGroup;
            LblCountry.Text = bloodUser.City;
            _email = bloodUser.Email;
            _phoneNumber = bloodUser.Phone;
		}

        private void TapEmail_Tapped(object sender, EventArgs e)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                // Send simple e-mail to single receiver without attachments, bcc, cc etc.
                emailMessenger.SendEmail(_email, "[Add a subject]", "[Write email body]");

            }
        }

            private void TapPhone_Tapped(object sender, EventArgs e)
        {
            // Make Phone Call
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(_phoneNumber);
        }
    }
}