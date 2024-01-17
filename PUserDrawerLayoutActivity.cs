using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using AppMobilaDaniela.DataAccessDataBase;
using AppMobilaDaniela.DataSQLiteTables;
using Newtonsoft.Json;


namespace AppMobilaDaniela
{
    [Activity(Label = "PUserDrawerLayoutActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class PUserDrawerLayoutActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener

    {
        NavigationView navigationView;
        Android.Support.V7.Widget.Toolbar toolbar;
        View pUserAccountInfo;
        View pUserAccountSettings;
        View pUserManageHoney;

        EditText fullName;
        EditText phone;
        EditText email;
        EditText password;

        Button editPUserButton;
        EditText editFullName;
        EditText editPhone;
        EditText editEmail;
        EditText editPassword;

        Button buttonManageHoney;


        LinearLayout background_picture_map;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.pUserDrawerLayout);

            toolbar=FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Honey";
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            ConfigureNavigationViewHeader();
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            //UserAccountInfo
            pUserAccountInfo = FindViewById<View>(Resource.Id.layout_pUserAccountInfo);

            fullName = FindViewById<EditText>(Resource.Id.fullNameText);
            phone = FindViewById<EditText>(Resource.Id.phoneText);
            email = FindViewById<EditText>(Resource.Id.emailText);
            password = FindViewById<EditText>(Resource.Id.passwordText);

            //EditUserAccountSettings
            pUserAccountSettings = FindViewById<View>(Resource.Id.layout_pUserAccountSettings);

            editPUserButton = FindViewById<Button>(Resource.Id.EditPUserButton);

            editFullName = FindViewById<EditText>(Resource.Id.EditfullNameText);
            editPhone = FindViewById<EditText>(Resource.Id.EditphoneText);
            editEmail = FindViewById<EditText>(Resource.Id.EditemailText);
            editPassword = FindViewById<EditText>(Resource.Id.EditpasswordText);

            pUserManageHoney = FindViewById<View>(Resource.Id.layout_pUserManageHoney);
            buttonManageHoney = FindViewById<Button>(Resource.Id.buttonManageHoney);
            buttonManageHoney.Click += ButtonManageHoney_Click;
            //Background 
            background_picture_map = FindViewById<LinearLayout>(Resource.Id.background_picture_map);



        }

        private void ButtonManageHoney_Click(object sender, EventArgs e)
        {
            var activity = new Intent(this, typeof(PUserManageHoney));
            int idPUser = Intent.GetIntExtra("idPUser", 0);
            activity.PutExtra("idPUser", idPUser);
            StartActivity(activity);
        }

        private void ConfigureNavigationViewHeader()
        {
            View viewHeader = LayoutInflater.Inflate(Resource.Layout.p_nav_header_main, null);
            TextView textView = viewHeader.FindViewById<TextView>(Resource.Id.textViewPUser);
            string name = Intent.GetStringExtra("Pusername");
            textView.Text = name;


            navigationView.AddHeaderView(viewHeader);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);

            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_logout)
            {
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
                //return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs e)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Let's manage!", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_account:
                    toolbar.Title = "Account";

                   
                    pUserAccountSettings.Visibility = ViewStates.Gone;
                    background_picture_map.Visibility = ViewStates.Gone;
                    pUserManageHoney.Visibility = ViewStates.Gone;
                    pUserAccountInfo.Visibility = ViewStates.Visible;

                    PUser infoAccountUser = JsonConvert.DeserializeObject<PUser>(Intent.GetStringExtra("PUserAccountInfo"));
                    fullName.Text = infoAccountUser.FullName;
                    phone.Text = infoAccountUser.Phone;
                    email.Text = infoAccountUser.Email;
                    password.Text = infoAccountUser.Password;

                    break;

                case Resource.Id.nav_manage:
                    toolbar.Title = "Manage";


                    pUserAccountSettings.Visibility = ViewStates.Gone;
                    background_picture_map.Visibility = ViewStates.Gone;
                    pUserAccountInfo.Visibility = ViewStates.Gone;
                    pUserManageHoney.Visibility = ViewStates.Visible;

                    

                    break;


                case Resource.Id.nav_account_settings:
                    toolbar.Title = "Settings";

                    pUserAccountInfo.Visibility = ViewStates.Gone;
                    pUserManageHoney.Visibility = ViewStates.Gone;
                    background_picture_map.Visibility = ViewStates.Gone;
                    pUserAccountSettings.Visibility = ViewStates.Visible;

                    PUser infoAccountSettings = JsonConvert.DeserializeObject<PUser>(Intent.GetStringExtra("PUserAccountInfo"));
                    editFullName.Text = infoAccountSettings.FullName;
                    editPhone.Text = infoAccountSettings.Phone;
                    editEmail.Text = infoAccountSettings.Email;
                    editPassword.Text = infoAccountSettings.Password;

                    editPUserButton.Click += EditPUserButton_Click;

                    break;



            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
        }

        private void EditPUserButton_Click(object sender, EventArgs e)
        {
            PUser infoAccountSettings = JsonConvert.DeserializeObject<PUser>(Intent.GetStringExtra("PUserAccountInfo"));
            string email = infoAccountSettings.Email;
            DataBaseClass sq = new DataBaseClass();
            var user = sq.GetPUser(email);
            if (editFullName.Text != "" && editPhone.Text != "" && editEmail.Text != "" && editPassword.Text != "")
            {
                user.FullName = editFullName.Text;
                user.Phone = editPhone.Text;
                user.Email = editEmail.Text;
                user.Password = editPassword.Text;
            }

            sq.UpdatePUser(user);
            OnBackPressed();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.nav_account)
            {

            }
          
            //else if (id == Resource.Id.nav_camera)
            //{
            //    // Handle the camera action
            //}
            //else if (id == Resource.Id.nav_gallery)
            //{

            //}
            //else if (id == Resource.Id.nav_slideshow)
            //{

            //}
            //else if (id == Resource.Id.nav_manage)
            //{

            //}
            //else if (id == Resource.Id.nav_share)
            //{

            //}
            //else if (id == Resource.Id.nav_send)
            //{

            //}

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

    }
}