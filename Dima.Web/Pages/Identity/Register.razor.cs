using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class RegisterPage : ComponentBase
{
    #region Dependencies
    [Inject] 
    public ISnackbar SnackBar { get; set; } = null!;
    
    [Inject]
    public IIdentityHandler Handler { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject] public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    #endregion
    
    #region Properties
    public RegisterRequest RegisterRequest { get; set; } = new();
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
            var result = await Handler.RegisterAsync(RegisterRequest);

            if (result.IsSuccess)
            {
                SnackBar.Add(result.Message ?? "Login realizado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/login");
            }
            else
                SnackBar.Add(result.Message ?? "Ocorreu um erro na tentativa de registro", Severity.Error);
                
            
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