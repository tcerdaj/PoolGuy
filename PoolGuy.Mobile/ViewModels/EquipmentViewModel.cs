using GalaSoft.MvvmLight.Command;
using PoolGuy.Mobile.CustomControls;
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
using PoolGuy.Mobile.Extensions;

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

        private string  _searchTerm;

        public string  SearchTerm
        {
            get { return _searchTerm; }
            set {
                _searchTerm = value;
                OnPropertyChanged("SearchTerm");
                if (string.IsNullOrEmpty(value))
                {
                    ApplyEquipmentModelFilterAsync();
                }
            }
        }

        public bool ShowSearchTerm
        {
            get { return Globals.CurrentPage == Enums.ePage.EquipmentSelectModel; }
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
                try
                {
                    if (Equipment != null && Equipment.PoolId != Guid.Empty)
                    {
                        
                        Pool = await new PoolController().LoadAsync(Equipment.PoolId).ConfigureAwait(false);
                        if (Pool.Equipments.Any())
                        {
                            Title = "Update Equipment";
                        }
                        else
                        {
                            Title = "Select Equipment";
                        }
                    }
                    else
                    {
                        Title = "Select Equipment";
                        Equipment.Pool.Id = Guid.NewGuid();
                        Equipment.PoolId = Equipment.Pool.Id;
                        Pool = Equipment.Pool;
                    }

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
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Message.DisplayAlertAsync(Title, e.Message, "Ok");
                }
            });
        }

        public async void InitEquipment(FlexLayout flexlayout)
        {
            try
            {
                if (Globals.CurrentPage != Enums.ePage.Equipment)
                {
                    return;
                }

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
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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

        public ICommand GoBackCommand => new RelayCommand(async () =>
        {
            try
            {
                if (CurrentPage == null)
                {
                    return;
                }

                var flexlayout = CurrentPage.Page.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                flexlayout.Children.Clear();

                switch (Globals.CurrentPage)
                {
                    case Enums.ePage.Home:
                        break;
                    case Enums.ePage.SearchCustomer:
                        break;
                    case Enums.ePage.SelectEquipment:
                        Globals.CurrentPage = Enums.ePage.SearchCustomer;
                        Notify.RaiseCustomerAction(new Messages.RefreshMessage());
                        await NavigationService.CloseModal();
                        break;
                    case Enums.ePage.SelectManufacture:
                        Globals.CurrentPage = Enums.ePage.SelectEquipment;
                        Title = Globals.CurrentPage.ToString().SplitWord();
                        BindableLayout.SetItemsSource(flexlayout, EquipmentTypes);
                        break;
                    case Enums.ePage.Equipment:
                    case Enums.ePage.AddEquipment:
                    case Enums.ePage.EquipmentSelectModel:
                        if (Title == "Add Model Equipment")
                        {
                            Globals.CurrentPage = Enums.ePage.EquipmentSelectModel;
                            SelectManufactureCommand.Execute(SelectedManufacture);
                            return;
                        }

                        if (Equipment.Id != Guid.Empty)
                        {
                            Globals.CurrentPage = Enums.ePage.SearchCustomer;
                            Notify.RaiseCustomerAction(new Messages.RefreshMessage());
                            await NavigationService.CloseModal(false);
                            return;
                        }

                        CurrentPage.Page.ToolbarItems.Clear();
                        flexlayout.Direction = FlexDirection.Row;
                        flexlayout.Wrap = FlexWrap.Wrap;
                        Globals.CurrentPage = Enums.ePage.SelectManufacture;
                        Title = Globals.CurrentPage.ToString().SplitWord();
                        BindableLayout.SetItemsSource(flexlayout, Manufactures);
                        break;
                    default:
                        break;
                }

                OnPropertyChanged("ShowSearchTerm");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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
                CurrentPage.Page.Title = model.Name;
                var flexlayout = CurrentPage.Page.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }
                
                Globals.CurrentPage = Enums.ePage.SelectManufacture;
                Title = Globals.CurrentPage.ToString().SplitWord();
                BindableLayout.SetItemsSource(flexlayout, Manufactures);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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
                var flexlayout = CurrentPage.Page.FindByName<FlexLayout>("EquipmentType");
                if (model == null || CurrentPage == null || flexlayout == null)
                {
                    return;
                }

                Manufactures?.ForEach(x => x.Selected = false);
                model.Selected = !model.Selected;
                SelectedManufacture = model;
                
                Equipment.Type = SelectedEquipmentType;
                Equipment.Manufacture = SelectedManufacture;
                Equipment.ImageUrl = SelectedEquipmentType.ImageUrl;
                Globals.CurrentPage = Enums.ePage.AddEquipment;
                Title = Globals.CurrentPage.ToString().SplitWord();
                ApplyEquipmentModelFilterAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
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

        public ICommand NextCommand
        {
            get => new RelayCommand<string>((control) => Next(control));
        }

        private void Next(string control)
        {
            try
            {
                var page = CurrentPage?.Page.FindByName<FlexLayout>("EquipmentType");
                if (page.Children.FirstOrDefault() is ScrollView sv)
                {
                    var element = sv?.FindByName<object>(control);

                    if (element is CustomEntry customEntry)
                    {
                        customEntry.Focus();
                    }

                    if (element is Editor editor)
                    {
                        editor.Focus();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task SaveEquipmentAsync()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                var validationResult = FieldValidationHelper.IsFormValid(Equipment, CurrentPage.Page);

                if (!validationResult.Key)
                {
                    Message.Toast($"Unable to save equipment. {validationResult.Value}");
                    return;
                }

                if (Equipment.Id == Guid.Empty)
                {
                    Equipment.Id = Guid.NewGuid();
                    Equipment.Created = DateTime.Now.ToUniversalTime();
                    Pool.Equipments.Add(Equipment);
                }
                else
                {
                    var index = Pool.Equipments.ToList().FindIndex(x => x.Id == Equipment.Id);
                    Pool.Equipments[index] = Equipment;
                }

                await new PoolController().ModifyWithChildrenAsync(Pool);
                Notify.RaisePoolAction(new Messages.RefreshMessage() { ID = Pool.Id.ToString() });

                await NavigationService.CloseModal(false);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally 
            { 
                IsBusy = false; 
            }
        }

        public ICommand ApplyFilterCommand 
        {
            get => new RelayCommand(async() => ApplyEquipmentModelFilterAsync(true));
        }

        private async void ApplyEquipmentModelFilterAsync(bool searchingModel = false)
        {
            try
            {
                var flexlayout = CurrentPage.Page.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                var criteria = new List<Data.Models.Query.SQLControllerListFilterField>(){
                            new Data.Models.Query.SQLControllerListFilterField{ FieldName = "ManufactureId",
                                ValueLBound = SelectedManufacture.Id.ToString()
                            },
                            new Data.Models.Query.SQLControllerListFilterField{ FieldName = "EquipmentTypeId",
                                ValueLBound = SelectedEquipmentType.Id.ToString()
                            }
                    };

                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    criteria = new List<Data.Models.Query.SQLControllerListFilterField>(){
                            new Data.Models.Query.SQLControllerListFilterField{ FieldName = "ManufactureId",
                                ValueLBound = SelectedManufacture.Id.ToString()
                            },
                            new Data.Models.Query.SQLControllerListFilterField{ FieldName = "EquipmentTypeId",
                                ValueLBound = SelectedEquipmentType.Id.ToString()
                            },
                            new Data.Models.Query.SQLControllerListFilterField{ FieldName = "Model",
                                ValueLBound = SearchTerm,
                                CompareMethod = Data.Models.Query.SQLControllerListFilterField.CompareMethodEnum.ContainsValue
                            }
                       };
                }

                var equipments = await new EquipmentController()
                   .ListWithChildrenAsync(new Data.Models.Query.SQLControllerListCriteriaModel
                   {
                       Filter = criteria
                   });

                if (equipments.Any() || searchingModel)
                {
                    Globals.CurrentPage = Enums.ePage.EquipmentSelectModel;
                    Title = "Select Model";
                    if (!CurrentPage.Page.ToolbarItems.Any())
                    {
                        CurrentPage.Page.ToolbarItems.Add(new ToolbarItem { Text = "Add", Command = AddEquipmentModelCommand });
                    }
                    flexlayout.Direction = FlexDirection.Column;
                    flexlayout.Wrap = FlexWrap.NoWrap;
                    OnPropertyChanged("ShowSearchTerm");
                }
                else
                {
                    Globals.CurrentPage = Enums.ePage.AddEquipment;
                }

                BindableLayout.SetItemsSource(flexlayout, (Globals.CurrentPage == Enums.ePage.EquipmentSelectModel ? equipments : new List<EquipmentModel> { Equipment }));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
        }

        public ICommand AddEquipmentModelCommand
        {
            get => new RelayCommand<string>((model) => AddEquipmentModel(model));
        }

        private void AddEquipmentModel(string model = null)
        {
            try
            {
                var flexlayout = CurrentPage.Page.FindByName<FlexLayout>("EquipmentType");
                if (flexlayout == null)
                {
                    return;
                }

                if(!string.IsNullOrEmpty(model))
                {
                    Equipment.Model = model;
                }

                CurrentPage.Page.ToolbarItems.Clear();
                CurrentPage.Title = $"Add Model Equipment";
                Globals.CurrentPage = Enums.ePage.Equipment;
                OnPropertyChanged("ShowSearchTerm");
                BindableLayout.SetItemsSource(flexlayout,  new List<EquipmentModel> { Equipment });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand DeleteEquipmentCommand
        {
            get => new RelayCommand<EquipmentModel>(async (model) => DeleteEquipment(model));
        }

        private async void DeleteEquipment(EquipmentModel model)
        {
            if (IsBusy) { return; }
            IsBusy = true;

            try
            {
                if (!await Message.DisplayConfirmationAsync("Delete Confirmation", "Are you sure want to delete equipment?", "Delete", "Cancel"))
                {
                    return;
                }

                var obj = Pool.Equipments.FirstOrDefault(x => x.Id == model.Id);
                Pool.Equipments.Remove(obj);
                var pc = new PoolController();
                var pool = await  pc.LoadAsync(Pool.Id);
                pool.Equipments = Pool.Equipments;
                await new PoolController().ModifyWithChildrenAsync(pool);
                Pool.RaiseEquipmentNotification();
                OnPropertyChanged("Pool");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Message.DisplayAlertAsync(Title, e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}