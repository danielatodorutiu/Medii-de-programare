using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AppMobilaDaniela.DataAccessDataBase;
using AppMobilaDaniela.DataSQLiteTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace AppMobilaDaniela
{
    [Activity(Label = "Manage Honey")]
    public class PUserManageHoney : AppCompatActivity
    {
        public RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        public RecyclerAdapter mAdapter;
        private List<Product> mProducts;
        private List<Product> mProductsCopy;
        private List<Product> mProductsCD;
        private List<Magazin> mMagazin;
        
        public IMenu menu;
        DataBaseClass db;
        private int lastPosition;
        Spinner spinnerMagazine;
        public static DataBaseClass database;
        ArrayAdapter<Magazin> arrayAdapter;
        int idPuser;

        private void Filter()
        {
            List<Product> listaNouaProduseFiltrare = new List<Product>(); //construim o noua lista de produse
            if (spinnerMagazine.SelectedItem.ToString().Equals("Magazine")) //daca selectia din spinner e by default->adica Magazine -> la lista de produse ii dam lista copiata
                mProducts = mProductsCopy;

            else
            {
                foreach (Product product in mProductsCopy) //luam fiecare produs din lista copiata-> daca produsul respectiv are Id-ul magazinului egal cu ceea ce am selectat in spinner-> in lista noua de produse se adauga produsul
                {
                    if (product.magazinId == arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id)
                        listaNouaProduseFiltrare.Add(product);
                }
                mProducts = listaNouaProduseFiltrare; //la lista noastra initiala de produse se adauga si ceea ce am adaugat in lista noua
            }
            mAdapter.mProducts = mProducts;  //se actualizeaza adapterul cu lista noua
            mAdapter.NotifyDataSetChanged();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.honeyProducts);

            db = new DataBaseClass();

            Product newProduct = new Product();

            Magazin newMagazin = new Magazin();
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

               idPuser = Intent.GetIntExtra("idPUser", 0);
               DataBaseClass sq = new DataBaseClass();
               PUser user=sq.GetPUser(idPuser);
               mProducts = db.getAllProductsByUserID(user.PId); 


           // mProducts = db.getAllProducts();

            mProductsCopy = new List<Product>(); //lista copie pt filtrare
            mProductsCopy.AddRange(mProducts);

            mProductsCD = new List<Product>(); //lista copie pt stergere multipla
            mProductsCD.AddRange(mProducts);
            mMagazin = db.getAllMagazin();

            //create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter(mProducts, mRecyclerView, db, this);
            mAdapter.CellClick_ButtonDelete += MAdapter_CellClick_ButtonDelete;
            mAdapter.CellClick_ButtonEdit += MAdapter_CellClick_ButtonEdit;
            //  mAdapter = new RecyclerAdapter(mMagazin);
            mRecyclerView.SetAdapter(mAdapter);

            // MenuInflater.Inflate(Resource.Menu.menu_pop, menu);
            // base.OnCreateOptionsMenu(menu);
            Button btnInsertData = FindViewById<Button>(Resource.Id.btnInsertData);
            btnInsertData.Click += BtnInsertData_Click;

            database = new DataBaseClass();

            spinnerMagazine = FindViewById<Spinner>(Resource.Id.spinnerMagazine);
            arrayAdapter = new ArrayAdapter<Magazin>(this, Resource.Layout.support_simple_spinner_dropdown_item);
            List<Magazin> magazine = PUserManageHoney.database.GetMagazins();
            Magazin magazin = new Magazin();
            magazin.Name = "Magazine";
            arrayAdapter.Add(magazin);
            arrayAdapter.AddAll(magazine);
            arrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinnerMagazine.Adapter = arrayAdapter;

            spinnerMagazine.ItemSelected += SpinnerMagazine_ItemSelected; 

           





        }
   
        private void SpinnerMagazine_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            // product.magazinId = arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id;
            Filter();

        }

        // Alert Dialog box-if you want to delete something press Yes - then the product is deleted
        private void MAdapter_CellClick_ButtonDelete(object sender, Product e)
        {

            // throw new NotImplementedException();
            this.RunOnUiThread(() =>
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

                alertDialog.SetTitle("Are you sure?");
                alertDialog.SetMessage("Do you want to delete this item?");
                alertDialog.SetPositiveButton("yes", delegate
                {
                    alertDialog.Dispose();

                    //e.Position = mAdapter.mProducts.IndexOf(e);
                    db.deleteProduct(e);
                    mAdapter.mProducts.Remove(e);
                    mProductsCopy.Remove(e);
                    mAdapter.NotifyItemRemoved(e.Position);

                    Toast.MakeText(this, " The product " + e.ToString() + " was deleted! ", ToastLength.Long).Show();
                });
                alertDialog.SetNegativeButton("NO", (IDialogInterfaceOnClickListener)null);
                alertDialog.Create();
                alertDialog.Show();
            });
        }

        //public void mEditProduct_Click(int position)
        //{
        //    Intent nextActivity = new Intent(this, typeof(Activity2EditPage));
        //    lastPosition = position;
        //    StartActivityForResult(nextActivity, 222);
        //}



        // Insert and Edit product in DataBase
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            //Edit/Update
            if (requestCode == 222 && resultCode == Result.Ok)
            {
                Product product;
                string jsonProduct = data.GetStringExtra("product");
                product = JsonSerializer.Deserialize<Product>(jsonProduct); //deserializez

                product.Id = mAdapter.mProducts[lastPosition].Id;
                mProductsCopy[mProductsCopy.IndexOf(mProducts[lastPosition])] = product;
                mAdapter.mProducts[lastPosition] = product;


                db.updateProduct(product);
                //       SpinnerMagazine_ItemSelected(null, new AdapterView.ItemSelectedEventArgs(null,null,lastPosition,lastPosition));
                Filter();

            }

            //Insert
            else if (requestCode == 333 && resultCode == Result.Ok)
            {
                Product product;
                string jsonProduct = data.GetStringExtra("product");
                product = JsonSerializer.Deserialize<Product>(jsonProduct);
                //mAdapter.mProducts.Add(product);

                if (!spinnerMagazine.SelectedItem.ToString().Equals("Magazine")) //daca item-ul ales este diferit de cuvantul default Magazine -> produsul este pus in lista copie
                    mProductsCopy.Add(product);

                mAdapter.mProducts.Add(product); //produsul este adaugat in adapter, pe interfata
                db.addProduct(product); //adauga produsul in baza de date

                Filter();

            }

            else if (requestCode == 444 && resultCode == Result.Ok)
            {
             /*   Depozit depozit;
                string jsonDepozit = data.GetStringExtra("depozit");
                depozit = JsonSerializer.Deserialize<Depozit>(jsonDepozit);
                db.addDepozit(depozit); */
            }
        }

        private void BtnDepozit_Click(object sender, EventArgs e)
        {
          /*  // throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(InsertDepozitActivity));
            StartActivityForResult(nextActivity, 444); */
        }
        private void MMultipleDelete_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetTitle("Are you sure?");
            alertDialog.SetMessage("Do you want to delete this item?");
            alertDialog.SetPositiveButton("yes", delegate
            {

                // throw new NotImplementedException();
                List<Product> listaNouaProduse = new List<Product>(); //construim o noua lista de produse

                listaNouaProduse.AddRange(mProducts); //populez lista noua
                foreach (Product product in mProductsCD) //luam fiecare produs din lista copiata-> daca produsul respectiv are Id-ul magazinului egal cu ceea ce am selectat in spinner-> in lista noua de produse se adauga produsul

                    if (product.Checked)  //am inclus Checked ca si coloana[Ignore] in clasa Produse
                    {
                        listaNouaProduse.Remove(product);
                        db.deleteProduct(product);
                        mProductsCopy.Remove(product); //ca sa sterg produsele din lista copie de la filtrare
                        Toast.MakeText(this, "The selected products were deleted!", ToastLength.Long).Show();
                    }

                mProducts = listaNouaProduse; //la lista noastra initiala de produse se adauga listaNouaProduse(am sters din ea elemente)

                mAdapter.mProducts = mProducts;  //se actualizeaza adapterul cu lista noua
                mAdapter.NotifyDataSetChanged();
            });

            alertDialog.SetNegativeButton("NO", (IDialogInterfaceOnClickListener)null);
            alertDialog.Create();
            alertDialog.Show();
        }

     


        //When the EditButton is pressed -> will apear another page with a specific interface to edit a product
        private void MAdapter_CellClick_ButtonEdit(object sender, Product e)
        {
            //throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(EditHoney));
            // lastPosition = position;

            e.Position = mAdapter.mProducts.IndexOf(e); //here is the position
            lastPosition = e.Position;
            //ca sa imi apara datele produsului pe care vreau sa il editez in EditText
            string jsonProduct = JsonSerializer.Serialize<Product>(mProducts[lastPosition]); //Seriallizez produsul selecat-ii iau toate datele
            nextActivity.PutExtra("product", jsonProduct); //trimit eticheta produsului "Key" si data care vreau sa fie transferata, adica toate datele despre produs
            StartActivityForResult(nextActivity, 222); //merge in 222

        }

        private void BtnInsertData_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(InsertHoney));
            int idPUser = Intent.GetIntExtra("idPUser", 0);
            nextActivity.PutExtra("idPUser", idPUser);
            StartActivityForResult(nextActivity, 333);
        }
    }

    public class RecyclerAdapter : RecyclerView.Adapter, View.IOnClickListener, IFilterable
    {
        public List<Product> mProducts;
        private IMenu menu;
        public int itemPosition;
        private RecyclerView recyclerView;
        private DataBaseClass db;
        PUserManageHoney pUserManageHoney;

        //Open Activity2EditPage
        public event EventHandler<int> ItemClick;
        public event EventHandler<Product> CellClick_ButtonDelete;
        public event EventHandler<Product> CellClick_ButtonEdit;

        //  private List<Magazin> mMagazin;

        public RecyclerAdapter(List<Product> mProducts, RecyclerView recyclerView, DataBaseClass db, PUserManageHoney pUserManageHoney)
        {
            this.mProducts = mProducts;
            this.recyclerView = recyclerView;
            this.db = db;
            this.pUserManageHoney = pUserManageHoney;


        }


        public override int ItemCount => mProducts.Count;

        public Filter Filter => throw new NotImplementedException();

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // throw new System.NotImplementedException();

            //all the items which appear on the Itemi.axml
            View Itemi = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.honeyItem, parent, false);
            TextView txtName = Itemi.FindViewById<TextView>(Resource.Id.txtName);
            TextView txtSubject = Itemi.FindViewById<TextView>(Resource.Id.txtSubject);
            TextView txtMessage = Itemi.FindViewById<TextView>(Resource.Id.txtMessage);
            CheckBox mDeleteProduct = Itemi.FindViewById<CheckBox>(Resource.Id.btn_delete);
            CheckBox mEditProduct = Itemi.FindViewById<CheckBox>(Resource.Id.btn_edit);

            CheckBox mDeleteAllProduct = Itemi.FindViewById<CheckBox>(Resource.Id.btn_multipleDelete); //checkBoxul pentru stergere multipla-> pus in MyView
            // LinearLayout hiddenLayout=Itemi.FindViewById<LinearLayout>(Resource.Id.showMoreLayout);
            //mEditProduct.Click += MEditProduct_Click;


            //Tot ce apare pe un card-pe Itemi-trebuie sa fie aici
            MyView view = new MyView(Itemi, OnCellClick_ButtonDelete, OnCellClick_ButtonEdit) { mName = txtName, mSubject = txtSubject, mMessage = txtMessage, mDeleteProduct = mDeleteProduct, mEditProduct = mEditProduct, mDeleteAllProduct = mDeleteAllProduct };
            view.Update(mDeleteProduct);
            view.Update2(mEditProduct);
            return view;

        }







        //private void MEditProduct_Click(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    mainActivity.mEditProduct_Click(itemPosition);
        //}

        private void OnCellClick_ButtonEdit(Product obj)
        {
            if (CellClick_ButtonEdit != null)
                CellClick_ButtonEdit(this, obj);
        }

        private void OnCellClick_ButtonDelete(Product obj)
        {
            if (CellClick_ButtonDelete != null)
                CellClick_ButtonDelete(this, obj);
        }



        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            myHolder.mName.Text = mProducts[position].Name;
            myHolder.mSubject.Text = mProducts[position].Cantity;
            myHolder.mMessage.Text = (mProducts[position].Id).ToString();

            myHolder.product = mProducts[position];

            //pentru stergere multipla-ca sa imi tina minte checkboxul care a fost selectat
            //trebuie folosit AdapterPosition ca sa imi ia pozitia buna a produsului selectat
            myHolder.mDeleteAllProduct.Checked = mProducts[myHolder.AdapterPosition].Checked; //la checkbox-ul selectat ii dau pozitia produsului din adapter
            if (!myHolder.mDeleteAllProduct.HasOnClickListeners) //daca nu este selectat checkbox-ul
            {
                myHolder.mDeleteAllProduct.Click += (sender, e) => //si se apasa pe checkBox
                {
                    mProducts[myHolder.AdapterPosition].Checked = !mProducts[myHolder.AdapterPosition].Checked;
                };
            }

        }



        public void OnClick(View v)
        {
            //throw new NotImplementedException();
            //  itemPosition = recyclerView.GetChildAdapterPosition(v);
            //  if (hiddenLayout.Visibility == ViewStates.Gone)
            //     hiddenLayout.Visibility = ViewStates.Visible;
            //  else
            //     hiddenLayout.Visibility = ViewStates.Gone;
        }

        public void setProduct(JavaList<Product> product)
        {
            // throw new NotImplementedException();
            // this.mProducts = product;
        }
    }

    public class MyView : RecyclerView.ViewHolder
    {
        public View mMainView { get; set; }
        public TextView mName { get; set; }
        public TextView mSubject { get; set; }
        public TextView mMessage { get; set; }
        public object menuPopUp { get; internal set; }

        public CheckBox mDeleteProduct { get; set; }

        private Action<Product> _cellClick_ButtonDelete;

        public CheckBox mEditProduct { get; set; }

        private Action<Product> _cellClick_ButtonEdit;

        public Product product { get; set; }

        public CheckBox mDeleteAllProduct { get; set; } //folosit pentru stergere multipla
                                                        //  public LinearLayout hiddenLayout { get; private set; }

        public MyView(View view, Action<Product> buttonDelete, Action<Product> buttonEdit) : base(view)
        {
            mMainView = view;
            _cellClick_ButtonDelete = buttonDelete;
            _cellClick_ButtonEdit = buttonEdit;


        }

        public void Update(CheckBox delete)
        {
            mDeleteProduct = delete;
            mDeleteProduct.Click += MDeleteProduct_Click;
        }


        private void MDeleteProduct_Click(object sender, EventArgs e)
        {
            product.Position = this.AdapterPosition;
            _cellClick_ButtonDelete(product);
        }

        public void Update2(CheckBox edit)
        {
            mEditProduct = edit;
            mEditProduct.Click += MEditProduct_Click;
        }

        private void MEditProduct_Click(object sender, EventArgs e)
        {
            product.Position = this.AdapterPosition;
            _cellClick_ButtonEdit(product);

        }
    }

}