using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    #region Dependecies
    [Inject] 
    public ISnackbar SnackBar { get; set; } = null!;
    
    [Inject]
    public IIdentityHandler Handler { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject] public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    #endregion
    
    #region Properties
    public LoginRequest LoginRequest { get; set; } = new();
    public bool IsBusy { get; set; } = false;
    #endregion
    
    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity != null && user.Identity.IsAuthenticated)
            NavigationManager.NavigateTo("/");
            
    }
    #endregion

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.LoginAsync(LoginRequest);

            if (result.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                NavigationManager.NavigateTo("/");
            }
            else
                SnackBar.Add(result.Message ?? "Falha na tentativa de login", Severity.Error);
        }
        catch (Exception e)
        {
            SnackBar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}