using PoolGuy.Mobile.Services.Interface;
using Rg.Plugins.Popup.Pages;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using PoolGuy.Mobile.Controllers;
using System.Reflection;
using PoolGuy.Mobile.CustomControls;

namespace PoolGuy.Mobile.Services
{
    public class NavigationService : INavigationService
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private PopupPage _popUp;

        private NavigationPage _navigation
        {
            get
            {
                try
                {
                    return ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new NavigationPage();   
                }
            }
        }

        public CustomPage CurrentPage 
        {
            get;set;
        }

        public void Configure(string pageKey, Type pageType)
        {
            try
            {
                lock (_pagesByKey)
                {
                    if (_pagesByKey.ContainsKey(pageKey))
                    {
                        _pagesByKey[pageKey] = pageType;
                    }
                    else
                    {
                        _pagesByKey.Add(pageKey, pageType);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task CloseAllAsync()
        {
            try
            {
                Debug.WriteLine("CloseAllAsync()");
                await PopToRootAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task CloseModal(bool animation = false)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"CloseModal(animation={animation})");
                    
                    Page page = null;
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        page = await _navigation.Navigation.PopModalAsync(false);
                    }
                    else
                    {
                        page = await _navigation.Navigation.PopModalAsync(animation);
                    }

                    if (page != null)
                    {
                        var pageKey = _pagesByKey.First(p => p.Value == page.GetType()).Key;
                        Debug.WriteLine($"CloseModal():PageKey({pageKey})");
                    }

                    CurrentPage = new CustomPage(_navigation.CurrentPage, Data.Models.Enums.ePageType.CloseModal);

                    AppStateController.PopViewState();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public bool? IsOnHomeView()
        {
            try
            {
                Debug.WriteLine($"IsOnHomeView(title={_navigation.Navigation.NavigationStack.Last().Title})");

                return _navigation.Navigation.NavigationStack.Last().Title == "Home" 
                    && _navigation.Navigation.ModalStack.Count == 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task NavigateToDialog(string pageKey)
        {
            await NavigateToDialog(pageKey, null);
        }

        public async Task NavigateToDialog(string pageKey, object parameter, object parameter2 = null)
        {
            await SemaphoreSlim.WaitAsync();
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"NavigateToDialog(pageKey={pageKey}, param1={parameter},param2={parameter2})");

                    var currentPageKey = string.Empty;

                    // Make sure only one modal opens when a navigation button is clicked multiple times
                    if (_navigation.Navigation.ModalStack.Count > 0)
                    {
                        currentPageKey = _pagesByKey.First(p => p.Value == _navigation.Navigation.ModalStack.Last().GetType()).Key;
                        if (currentPageKey == pageKey)
                        {
                            return;
                        }
                    }

                    if (_pagesByKey.ContainsKey(pageKey))
                    {
                        var type = _pagesByKey[pageKey];
                        ConstructorInfo constructor;
                        object[] parameters;

                        if (parameter == null && parameter2 == null)
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(c => !c.GetParameters().Any());

                            parameters = new object[]
                            {
                            };
                        }
                        else if (parameter != null && parameter2 != null)
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(
                                    c =>
                                    {
                                        var p = c.GetParameters();
                                        return p.Count() == 2
                                               && p[0].ParameterType == parameter.GetType()
                                               && p[1].ParameterType == parameter2.GetType();
                                    });

                            parameters = new[]
                            {
                        parameter,
                        parameter2
                    };
                        }
                        else
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(
                                    c =>
                                    {
                                        var p = c.GetParameters();
                                        return p.Count() == 1
                                               && p[0].ParameterType == parameter.GetType();
                                    });

                            parameters = new[]
                            {
                        parameter
                    };
                        }

                        if (constructor == null)
                        {
                            throw new InvalidOperationException(
                                "No suitable constructor found for page " + pageKey);
                        }

                        Debug.WriteLine($"NavigateToDialog() PageKey: {pageKey}");

                        var page = constructor.Invoke(parameters) as Page;
                        CurrentPage = new CustomPage(page, Data.Models.Enums.ePageType.Dialog);

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            await _navigation.Navigation.PushModalAsync(page);
                        }
                        else
                        {
                            await _navigation.Navigation.PushModalAsync(page, false);
                        }

                        SavePreviousPageState(currentPageKey);
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                            nameof(pageKey));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    SemaphoreSlim.Release();
                }
            });
        }

        public async Task NavigateToDialog(Page page)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"NavigateToDialog(page={page.Title})");
            
                    string currentPageKey = string.Empty;
                    string newPageKey = string.Empty;

                    // Make sure only one modal opens when a navigation button is clicked multiple times
                    if (_navigation.Navigation.ModalStack.Count > 0)
                    {
                        currentPageKey = _pagesByKey
                            .First(p => p.Value == _navigation.Navigation.ModalStack.Last().GetType()).Key;
                        newPageKey = _pagesByKey.First(p => p.Value == page.GetType()).Key;
                        if (currentPageKey == newPageKey)
                        {
                            return;
                        }
                    }

                    Debug.WriteLine($"NavigateToDialog() PageKey: {newPageKey}");

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await _navigation.Navigation.PushModalAsync(page);
                    }
                    else
                    {
                        await _navigation.Navigation.PushModalAsync(page, false);
                    }

                    SavePreviousPageState(currentPageKey);
                    CurrentPage = new CustomPage(page, Data.Models.Enums.ePageType.Dialog);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task PopPopupAsync(bool animate = false)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"PopPopupAsync(animate={animate})");

                    try
                    {
                        if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Any())
                        {
                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(animate);
                            _popUp = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Error in NavigationService.PopPopupAsync method invoking Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync. {ex}");
                    }

                    Debug.WriteLine(
                        $"Second try * Pages in memory {Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Count}");

                    if (_popUp != null)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(_popUp, animate);
                        _popUp = null;
                    }

                    CurrentPage = new CustomPage(_navigation.CurrentPage, Data.Models.Enums.ePageType.PopPopup);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(
                        $"Error in NavigationService.PopPopupAsync method invoking Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync. {ex}");
                }
            });
        }

        public async Task PopToRootAsync()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"PopToRootAsync()");

                    while (_navigation.Navigation.ModalStack.Any())
                    {
                        await _navigation.Navigation.PopModalAsync(false);
                    }

                    await _navigation.Navigation.PopToRootAsync(false);

                    CurrentPage = new CustomPage(_navigation.CurrentPage, Data.Models.Enums.ePageType.PopPopup);

                    AppStateController.ClearNavigationMetaStack();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task PushPopupAsync(string popUpPageKey, bool animate = false)
        {
            await PushPopupAsync(popUpPageKey, null, animate);
        }

        public async Task PushPopupAsync(string popUpPageKey, object parameter, bool animate = false, CancellationToken cancelToken = default)
        {
            await SemaphoreSlim.WaitAsync();
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"PushPopupAsync(popUpPageKey={popUpPageKey}, param1={parameter}, animate={animate})");
                    // Make sure only one modal opens when a navigation button is clicked multiple times
                    if (_popUp != null)
                    {
                        return;
                    }

                    if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Count > 0)
                    {
                        var popupPageType = Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Last().GetType();
                        var currentPopupPageKey = _pagesByKey.First(p => p.Value == popupPageType).Key;
                        if (currentPopupPageKey == popUpPageKey)
                        {
                            return;
                        }
                    }

                    if (_pagesByKey.ContainsKey(popUpPageKey))
                    {
                        var type = _pagesByKey[popUpPageKey];
                        ConstructorInfo constructor;
                        object[] parameters;

                        if (parameter == null)
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(c => !c.GetParameters().Any());

                            parameters = new object[]
                            {
                            };
                        }
                        else
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(
                                    c =>
                                    {
                                        var p = c.GetParameters();
                                        return p.Count() == 1
                                               && p[0].ParameterType == parameter.GetType();
                                    });

                            parameters = new[]
                            {
                        parameter
                    };
                        }

                        if (constructor == null)
                        {
                            throw new InvalidOperationException(
                                "No suitable constructor found for popup page " + popUpPageKey);
                        }

                        _popUp = constructor.Invoke(parameters) as Rg.Plugins.Popup.Pages.PopupPage;

                        if (_popUp == null)
                        {
                            return;
                        }

                        // This fixes the issue where the popup will not reopen if clicked outside
                        if (_popUp.CloseWhenBackgroundIsClicked)
                        {
                            _popUp.BackgroundClicked += async (s, o) =>
                            {
                                await RemovePopupAsync(animate, _popUp);
                                _popUp = null;
                                CurrentPage = new CustomPage(_navigation.CurrentPage, Data.Models.Enums.ePageType.PushPopup);
                            };
                        }

                        cancelToken.Register(async () =>
                        {
                            await RemovePopupAsync(animate, _popUp);
                            _popUp = null;
                            CurrentPage = new CustomPage(_navigation.CurrentPage, Data.Models.Enums.ePageType.PushPopup);
                        });

                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(_popUp, animate);
                        CurrentPage = new CustomPage(_popUp);
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"No such popup page: {popUpPageKey}. Did you forget to call NavigationService.Configure?",
                            nameof(popUpPageKey));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                finally
                {
                    SemaphoreSlim.Release();
                }
            });
        }

        public async Task ReplaceRoot(Page page)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    if (page == null)
                    {
                        return;
                    }

                    Debug.WriteLine($"ReplaceRoot(page={page.Title})");

                    var pageKey = _pagesByKey.First(p => p.Value == page.GetType()).Key;

                    if (_navigation.RootPage != null)
                    {
                        if (_navigation.RootPage.Title == page.Title)
                        {
                            return;
                        }

                        _navigation.Navigation.InsertPageBefore(page, _navigation.RootPage);
                        CurrentPage = new CustomPage(page, Data.Models.Enums.ePageType.ReplaceRoot);
                        await PopToRootAsync();
                    }
                    else
                    {
                        await _navigation.PushAsync(page, false);
                    }

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        ((MasterDetailPage)Application.Current.MainPage).Master.IconImageSource = "menu.png";
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task ReplaceRoot(string page)
        {
            try
            {
                Debug.WriteLine($"ReplaceRoot(string page={page})");

                if (_pagesByKey.ContainsKey(page))
                {
                    var type = _pagesByKey[page];
                    ConstructorInfo constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + page);
                    }

                    var viewPage = constructor.Invoke(null) as Page;
                    await ReplaceRoot(viewPage);
                }
                else
                {
                    throw new ArgumentException(
                        $"No such page: {page}. Did you forget to call NavigationService.Configure?",
                        nameof(page));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #region Helpers
        private void SavePreviousPageState(string pageKey)
        {
            Debug.WriteLine($"SavePreviousPageState(pageKey={pageKey})");

            if (string.IsNullOrEmpty(pageKey))
            {
                AppStateController.SaveViewState(((IContentPage)_navigation.CurrentPage)?.OnSleep());
            }
            else
            {
                var page = _navigation.Navigation.ModalStack.FirstOrDefault(x => x.GetType() == _pagesByKey[pageKey]);
                AppStateController.SaveViewState(((IContentPage)page)?.OnSleep());
            }
        }

        async Task RemovePopupAsync(bool animate, PopupPage popupPage)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    Debug.WriteLine($"RemovePopupAsync(animate={animate},popupPage={popupPage})");

                    if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Count > 0)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(popupPage, animate);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }
        #endregion
    }
}