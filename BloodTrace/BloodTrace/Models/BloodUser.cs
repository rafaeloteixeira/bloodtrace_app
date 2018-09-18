using System;
using System.Collections.Generic;
using System.Text;

namespace BloodTrace.Models
{
    public class BloodUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string BloodGroup { get; set; }
        public string ImagePath { get; set; }
        public string FullLogoPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return string.Empty;
                }
                return string.Format("http://bloodtraceapptest.azurewebsites.net/{0}", ImagePath.Substring(1));
            }
            set
            {
            }
        }
        public int Date { get; set; }
        public object ImageArray { get; set; }
    }
}
