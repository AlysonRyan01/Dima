using Dima.Core.Handlers;
using Dima.Web.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class CreateCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateCategoryRequest Request { get; set; } = new();
    
    #endregion
    
    #region Services

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.CreateAsync(Request);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? "Categoria criada com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/categorias");
            }
            else
                Snackbar.Add(result.Message ?? "Erro ao criar a categoria", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    #endregion
}