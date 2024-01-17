using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace AppMobilaDaniela
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        Button _buttonCreateUser;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var iv = FindViewById<ImageView>(Resource.Id.imageView1);
            iv?.SetImageResource(Resource.Drawable.display_image);

            _buttonCreateUser = FindViewById<Button>(Resource.Id.buttonCreateUser);
            _buttonCreateUser.Click += _buttonCreateUser_Click;
            var listPermissions = new System.Collections.Generic.List<string>();

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.ReadExternalStorage);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.WriteExternalStorage);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Internet) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.Internet);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadPhoneState) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.ReadPhoneState);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.AccessFineLocation);

            // Make the request with the permissions needed...and then check OnRequestPermissionsResult() for the results
            if (listPermissions.Count > 0)
                ActivityCompat.RequestPermissions(this, listPermissions.ToArray(), 123/*a code in OnRequestPermissionsResult*/);
            else
            {
                DoStartup();
            }
        }
        private void DoStartup()
        {
            throw new NotImplementedException();
        }

        private void _buttonCreateUser_Click(object sender, System.EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(PUserLoginActivity));
            StartActivity(nextActivity);
        }

    }
}