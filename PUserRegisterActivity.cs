using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppMobilaDaniela.DataAccessDataBase;
using AppMobilaDaniela.DataSQLiteTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMobilaDaniela
{
    [Activity(Label = "PUserRegisterActivity")]
    public class PUserRegisterActivity : Activity
    {
        Button registerPUserButton;
        EditText fullName;
        EditText phone;
        EditText email;
        EditText password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.pUserRegister);
            registerPUserButton = FindViewById<Button>(Resource.Id.registerPUserButton);
            registerPUserButton.Click += RegisterPUserButton_Click;


            fullName = FindViewById<EditText>(Resource.Id.fullNameText);
            phone = FindViewById<EditText>(Resource.Id.phoneText);
            email = FindViewById<EditText>(Resource.Id.emailText);
            password = FindViewById<EditText>(Resource.Id.passwordText);
        }

        private void RegisterPUserButton_Click(object sender, EventArgs e)
        {
            if (email.Text != "" && password.Text != "")
            {
                var sq = new DataBaseClass();
                var user = sq.GetPUser(email.Text);

                if (user == null)
                {
                    var newUser = new PUser();
                    newUser.FullName = fullName.Text;
                    newUser.Password = password.Text;
                    newUser.Phone = phone.Text;
                    newUser.Email = email.Text;

                    sq.RegisterPUser(newUser);
                    Toast.MakeText(this, "Welcome " + fullName.Text + " to our communnity!", ToastLength.Short).Show();
                    Intent nextActivity = new Intent(this, typeof(PUserLoginActivity));
                    StartActivity(nextActivity);
                }
                else
                {
                    Toast.MakeText(this, " Email is found, choose another credentials", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, " Email or Password is empty", ToastLength.Short).Show();
            }
           
        }

    }
}