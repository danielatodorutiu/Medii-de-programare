using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AppMobilaDaniela.DataAccessDataBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMobilaDaniela
{
    [Activity(Label = "pUserLoginActivity")]
    public class PUserLoginActivity : Activity
    {
        TextView clickToRegisterTextView;
        EditText email;
        EditText password;
        Button loginPUserButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.pUserLogin);

            clickToRegisterTextView=FindViewById<TextView>(Resource.Id.clickToRegisterTextView);
            clickToRegisterTextView.Click += ClickToRegisterTextView_Click;

            email = FindViewById<EditText>(Resource.Id.emailText);
            password = FindViewById<EditText>(Resource.Id.passwordText);

            loginPUserButton = FindViewById<Button>(Resource.Id.loginPUserButton);
            loginPUserButton.Click += LoginPUserButton_Click;

        }
       

        private void LoginPUserButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(email.Text) && !string.IsNullOrEmpty(password.Text))
            {
                DataBaseClass sq = new DataBaseClass();
                var user = sq.GetPUser(email.Text, password.Text);
                if (user != null)
                {
                    //Intent i = new Intent(this, typeof(ShowActivity));
                    //i.PutExtra("username", user.Username);
                    //StartActivity(i);
                    Toast.MakeText(this, "Good to see you! Let's manage the honey!", ToastLength.Long).Show();
                    Intent i = new Intent(this, typeof(PUserDrawerLayoutActivity));
                    i.PutExtra("Pusername", user.FullName);
                    i.PutExtra("idPUser", user.PId);
                    i.PutExtra("PUserAccountInfo", JsonConvert.SerializeObject(user));
                    StartActivity(i);
                    email.Text = "";
                    password.Text = "";
                }
                else
                    Toast.MakeText(this, "Username or Password is incorect. Not Registred?", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "User not found!", ToastLength.Long).Show();
        }

        private void ClickToRegisterTextView_Click(object sender, EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(PUserRegisterActivity));
            StartActivity(nextActivity);
            email.Text = "";
            password.Text = "";
        }
    }
}