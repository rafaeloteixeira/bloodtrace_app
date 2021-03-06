﻿using BloodTrace.Models;
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
        public DonorProfilePage()
        {
            InitializeComponent();
        }
        public DonorProfilePage(BloodUser bloodUser)
        {
            InitializeComponent();

            LblDonorName.Text = bloodUser.UserName;
            LblBloodGroup.Text = bloodUser.BloodGroup;
            lblCidade.Text = bloodUser.City;
            lblUf.Text = bloodUser.State;
            _email = bloodUser.Email;
            _phoneNumber = bloodUser.Phone;
        }

        private void btnLigar_Clicked(object sender, EventArgs e)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(_phoneNumber);

        }

        private void btnEmail_Clicked(object sender, EventArgs e)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                // Send simple e-mail to single receiver without attachments, bcc, cc etc.
                emailMessenger.SendEmail(_email, "[Add a subject]", "[Write email body]");

            }
        }
    }
}