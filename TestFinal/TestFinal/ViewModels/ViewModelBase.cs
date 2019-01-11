using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace TestFinal.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;

            TakePhotoCommand = new DelegateCommand(async () => await TakePhotoExecute());
            ChoosePhotoCommand = new DelegateCommand(async () => await ChoosePhotoExecute());
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedNewTo(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual async Task ChangeImage(string filePath, byte[] bytes)
        {
            await Task.Factory.StartNew(() => { Debug.WriteLine("Virtual function called: ChangeImage()"); });
        }

        #region Take Photo

        private bool _takePhoto = true;

        public ICommand TakePhotoCommand { get; }

        public async Task TakePhotoExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _takePhoto = false;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Alert", "No Camera", "OK");
            }
            else
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results =
                        await
                            CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        Directory = "Image",
                        Name = DateTime.Now.ToString("MM/dd/yyyy"),
                    });

                    //Small delay
                    await Task.Delay(TimeSpan.FromMilliseconds(200));

                    if (file == null)
                    {
                        _takePhoto = true;
                        return;
                    }

                    var memoryStream = new MemoryStream();
                    file.GetStream().CopyTo(memoryStream);

                    byte[] image = memoryStream.ToArray();
                    await ChangeImage(file.Path, image);

                    //dispose mediafile
                    file.Dispose();
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Alert", "Permission Denied", "OK");
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }

            _takePhoto = true;
            await CrossMedia.Current.Initialize();
        }

        #endregion

        #region Choose Photo

        private bool _choosePhoto = true;

        public ICommand ChoosePhotoCommand { get; }

        public async Task ChoosePhotoExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _choosePhoto = false;

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Alert", "No Pick Image Support", "OK");
            }
            else
            {
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (storageStatus != PermissionStatus.Granted)
                {
                    var results =
                        await
                            CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.PickPhotoAsync();

                    //Small delay
                    await Task.Delay(TimeSpan.FromMilliseconds(150));

                    if (file == null)
                    {
                        _choosePhoto = true;
                        return;
                    }

                    var memoryStream = new MemoryStream();
                    file.GetStream().CopyTo(memoryStream);

                    byte[] image = memoryStream.ToArray();
                    await ChangeImage(file.Path, image);

                    //dispose mediafile
                    file.Dispose();
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Alert", "Permission Denied", "OK");
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }

            _choosePhoto = true;
            await CrossMedia.Current.Initialize();
        }
    }

    #endregion
}
