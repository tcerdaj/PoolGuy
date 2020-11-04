using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.Data.Controllers;
using PoolGuy.Mobile.Data.Models;
using PoolGuy.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PoolGuy.Mobile.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        public EquipmentViewModel(EquipmentModel equipment)
        {
            Equipment = equipment;
            Globals.CurrentPage = equipment.Id == Guid.Empty? Enums.ePage.SelectEquipment: Enums.ePage.Equipment;
            Initialize();
        }

        public Page CurrentPage
        {
            get => (Shell.Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;
        }

        private EquipmentTypeModel _selectedEquipmentType;
        public EquipmentTypeModel SelectedEquipmentType
        {
            get { return _selectedEquipmentType; }
            set { _selectedEquipmentType = value; OnPropertyChanged("SelectedEquipmentType"); }
        }

        private ManufactureModel _selectedManufacture;
        public ManufactureModel SelectedManufacture
        {
            get { return _selectedManufacture; }
            set { _selectedManufacture = value; OnPropertyChanged("SelectedManufacture"); }
        }


        public void Initialize()
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (Equipment != null && Equipment.PoolId != Guid.Empty)
                {
                    Pool = await new PoolController().LoadAsync(Equipment.PoolId);
                    EquipmentTypes = new ObservableCollection<EquipmentTypeModel>(){
                          new EquipmentTypeModel{Id = Guid.Parse("34acfaed-3bc6-421d-9f42-9cb762dcef54"), Name = "Pump", ImageUrl = "pumps.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("b744b63b-6735-44bc-8ead-5d472395d777"), Name = "Filter", ImageUrl = "filters.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("7e0f7936-ce55-45aa-b45d-2a039d84235e"), Name = "Replacement Salt Cell", ImageUrl = "replacementcells.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("b8124bf2-1238-47a8-ba07-32b58cb814f1"), Name = "Chlorine Generator", ImageUrl = "ChlorinationSystem.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("7fd61270-8009-4a6d-9ceb-035577fe417b"), Name = "Ozone Generator", ImageUrl = "OzoneSystem.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("50f0649f-194a-416a-9a1c-bbf83474dd5d"), Name = "PH Control", ImageUrl = "PHControls.png"},
                          new EquipmentTypeModel{Id = Guid.Parse("8d4fd11e-0fd1-49ca-b26f-9433df1d1de5"), Name = "Parts Accessories", ImageUrl = "PartsAccessotries.png"},
                   };
                }
            });
        }

        public async void InitEquipment()
        {
            try
            {
                if (Globals.CurrentPage != Enums.ePage.Equipment)
                {
                    return;
                }

                var flexlayout = CurrentPage.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                BindableLayout.SetItemsSource(flexlayout, null);
                BindableLayout.SetItemsSource(flexlayout, new List<EquipmentModel>() { Equipment });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        private ObservableCollection<EquipmentTypeModel> _equipmentTypes;
        public ObservableCollection<EquipmentTypeModel> EquipmentTypes
        {
            get
            {
                return _equipmentTypes;
            }
            set { _equipmentTypes = value; OnPropertyChanged("EquipmentTypes"); }
        }

        public List<ManufactureModel> Manufactures
        {
            get
            {
                return new List<ManufactureModel>{
                      new ManufactureModel{ Id = Guid.Parse("6c590b52-0243-428b-aeb3-554c851da47a"), Name = "AutoPilot", ImageUrl = "AutoPilotLogo.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("81c219d7-8d56-4085-bf42-cb8830c80439"), Name = "CircuPool", ImageUrl = "CircuPool.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("5011be4d-b3a8-4883-843d-c075ae756ebc"), Name = "CompuPool", ImageUrl = "CompuPool.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("51715033-0ac8-4325-a297-7e25d5bd0d68"), Name = "CL Free Water System", ImageUrl = "clfree.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("df54ae35-ee54-4fba-bef0-10b1080fb00a"), Name = "Control Lomatic", ImageUrl = "controlomatic.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("03fd8db9-1cb5-4b26-8db3-b118f26f7bb3"), Name = "Dive", ImageUrl = ".png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("07b1f82e-07c4-4148-b7cf-f545433ded5b"), Name = "Eco matic", ImageUrl = "ecomatic.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("67b816a9-cddd-4abd-ae7f-e8775e4c955e"), Name = "Hayward", ImageUrl = "hayward.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("cf02cc43-b215-41ec-a94f-4659467759cb"), Name = "Jandy", ImageUrl = "jandy.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("afbb3887-2fe0-4897-b397-a440f25c355b"), Name = "Optimum", ImageUrl = "optimum.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("269a9e0b-455f-48ca-851a-d8fb44c699ab"), Name = "Orenda", ImageUrl = "orenda.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("335fb8ec-f48e-4189-b25c-ac926d9586e5"), Name = "Pentair", ImageUrl = "pentair.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("543b0a5c-cf4d-4e72-9bfe-3919a5948391"), Name = "resiliense", ImageUrl = "resilience.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("6468ad33-3320-466f-9c55-77cdadc851a8"), Name = "CMD SGS", ImageUrl = "cmdsgs.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("1e337d59-a655-47ab-a1bf-c90d683f0a35"), Name = "Solaxx", ImageUrl = "solaxx.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("b10194ef-eeb4-4f35-9ec1-b59eb9d07ee4"), Name = "Speck pumps", ImageUrl = "speckpumps.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("dfc9415f-51da-4b06-9959-e1460ee5746c"), Name = "Waterco", ImageUrl = "waterco.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("9e55dbc3-69cd-408b-8497-f7ecd41fc75a"), Name = "ZODIAC", ImageUrl = "zodiac.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("efebd1a4-f333-469f-a706-d6a4a67ddd20"), Name = "BLUE ESSENCE", ImageUrl = "blueessene.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("23369724-f2cf-4512-82d8-2c9b7c18bba0"), Name = "Mineral Springs", ImageUrl = "mineralsprings.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                      new ManufactureModel{ Id = Guid.Parse("0e822eca-289d-4c3f-9818-7ac7334da80f"), Name = "SWIMPURE PLUS", ImageUrl = "swinpureplus.png", WebSite = "https://autopilot.com", Description = "Since 1976, it has been our sincere belief that providing you with quality products and standing behind those products is our only path to success. We are the leader in our industry and have more pool and spa salt chlorine generators installed than any of our competitors."},
                  };
            }
        }

        private EquipmentModel _equipment;      
        public EquipmentModel Equipment
        {
            get { return _equipment; }
            set { _equipment = value; OnPropertyChanged("Equipment"); }
        }

        private PoolModel _pool;

        public PoolModel Pool
        {
            get { return _pool; }
            set { _pool = value; OnPropertyChanged("Pool"); }
        }

        public ICommand SaveCommand
        {
            get => new RelayCommand(async()=> Save());
        }

        private async void Save()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (Equipment == null)
                {
                    return;
                }

                // Is new equipment
                if (Equipment.Id == Guid.Empty)
                {
                    Pool.Equipments.Add(Equipment);
                }
                else
                {
                    var equipment = Pool.Equipments.FirstOrDefault(x => x.Id == Equipment.Id);
                    equipment = Equipment;
                    equipment.Modified = DateTime.Now;
                }

                await new PoolController().ModifyWithChildrenAsync(Pool);

                // Close current page

                // Notify refresh
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
            finally 
            {
                IsBusy = false; 
            }
        }

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            try
            {
                if (CurrentPage == null)
                {
                    return;
                }

                var flexlayout = CurrentPage.FindByName<FlexLayout>("EquipmentType");
                flexlayout.Children.Clear();

                switch (Globals.CurrentPage)
                {
                    case Enums.ePage.Home:
                        break;
                    case Enums.ePage.SearchCustomer:
                        break;
                    case Enums.ePage.Customer:
                        break;
                    case Enums.ePage.SelectEquipment:
                        Globals.CurrentPage = Enums.ePage.Customer;
                        await NavigationService.PopPopupAsync(false);
                        Shell.Current.SendBackButtonPressed();
                        break;
                    case Enums.ePage.SelecteManufacture:
                        CurrentPage.Title = "Select Equipment";
                        if (flexlayout == null)
                        {
                            return;
                        }

                        Globals.CurrentPage = Enums.ePage.SelectEquipment;
                        BindableLayout.SetItemsSource(flexlayout, EquipmentTypes);
                        break;
                    case Enums.ePage.Equipment:
                        if (Equipment.Id != Guid.Empty)
                        {
                            Globals.CurrentPage = Enums.ePage.Customer;
                            await NavigationService.PopPopupAsync(false);
                            Shell.Current.SendBackButtonPressed();
                            return;
                        }

                        CurrentPage.Title = "Select Manufacture";
                        if (flexlayout == null)
                        {
                            return;
                        }

                        Globals.CurrentPage = Enums.ePage.SelecteManufacture;
                        BindableLayout.SetItemsSource(flexlayout, Manufactures);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        });

        public ICommand SelectEquipmentTypeCommand
        {
            get => new RelayCommand<EquipmentTypeModel>((model) => SelectEquipmentTypeAsync(model));
        }

        private async void SelectEquipmentTypeAsync(EquipmentTypeModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }

                EquipmentTypes?.ForEach(x => x.Selected = false);
                model.Selected = true;

                if (CurrentPage == null)
                {
                    return;
                }

                SelectedEquipmentType = model;
                CurrentPage.Title = model.Name;
                var flexlayout = CurrentPage.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                Globals.CurrentPage = Enums.ePage.SelecteManufacture;
                BindableLayout.SetItemsSource(flexlayout, Manufactures);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        public ICommand SelectManufactureCommand
        {
            get => new RelayCommand<ManufactureModel>(async(model) => SelectManufactureAsync(model));
        }

        private async void SelectManufactureAsync(ManufactureModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }

                Manufactures?.ForEach(x => x.Selected = false);
                model.Selected = !model.Selected;

                if (CurrentPage == null)
                {
                    return;
                }

                SelectedManufacture = model;
                CurrentPage.Title = "Add Equipment";
                var flexlayout = CurrentPage.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                Equipment.Type = SelectedEquipmentType;
                Equipment.Manufacture = SelectedManufacture;
                Equipment.ImageUrl = SelectedEquipmentType.ImageUrl;

                Globals.CurrentPage = Enums.ePage.Equipment;
                BindableLayout.SetItemsSource(flexlayout, new List<EquipmentModel>() { Equipment });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }

        public ICommand OpenDateCommand
        {
            get => new RelayCommand<string>((controlName) => OpenDate(controlName));
        }

        private void OpenDate(string controlName)
        {
            ToggleFocus(controlName, ControlTypeEnum.DatePicker, true);
        }

        public ICommand SaveEquipmentCommand
        {
            get => new RelayCommand(async()=> await SaveEquipmentAsync());
        }

        private async Task SaveEquipmentAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (FieldValidationHelper.IsFormValid(Equipment, CurrentPage))
                {
                    if (Equipment.Id == Guid.Empty)
                    {
                        Equipment.Id = Guid.NewGuid();
                        Equipment.Created = DateTime.Now;
                        Pool.Equipments.Add(Equipment);
                    }
                    else
                    {
                        var equipment = Pool.Equipments.FirstOrDefault(x => x.Id == Equipment.Id);
                        equipment = Equipment;
                        equipment.Modified = DateTime.Now;
                    }

                    var poolController = new PoolController();
                    await poolController.ModifyWithChildrenAsync(Pool);
                    Notify.RaisePoolAction(new Messages.RefreshMessage());

                    await NavigationService.PopPopupAsync(false);
                    await Shell.Current.Navigation.PopAsync(true);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(Title, e.Message, "Ok");
            }
        }
    }
}