using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Tesseract.Droid;
using Android.Content;
using Android.Graphics;
using Java.IO;
using static Android.Provider.MediaStore;
using Plugin.Media;
using Plugin.CurrentActivity;
using Android.Support.V4.App;
using static Android.Support.V4.App.ActivityCompat;
using Android.Content.PM;
using Android.App; 

namespace Tesseract.DroidNew
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnRequestPermissionsResultCallback
    {
        TesseractApi api;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            var button = FindViewById<Button>(Resource.Id.button);
            api = new TesseractApi(this, AssetsDeployment.OncePerVersion);
            initialize();
            button.Click += delegate
            {
                ExtractText();
            };
            CrossCurrentActivity.Current.Init(this, bundle); 
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        async void initialize()
        {
            try
            {
                await api.Init("eng");
            }
            catch(Exception e)
            {

            }
        }

        private ProgressDialog dialog;
        async void ExtractText()
        {
            try
            {
                //await CrossMedia.Current.Initialize();
                var xFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = "test.jpg"
                });
                //svar filepath = "/storage/emulated/0/Android/data/Tesseract.Android.Tesseract.Android/files/Pictures/Sample/test_3.jpg";
                dialog = new ProgressDialog(this);
                dialog.SetMessage("Extracting Text");
                dialog.Show(); 
                await api.SetImage(xFile.GetStream());
                dialog.Hide(); 
                Android.App.AlertDialog.Builder builder1 = new Android.App.AlertDialog.Builder(this);
                builder1.SetMessage(api.Text);
                builder1.SetCancelable(true); 
                Android.App.AlertDialog alert11 = builder1.Create();
                alert11.Show();
                string text = api.Text;
            }
            catch (Exception e)
            {

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
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
    }
}

