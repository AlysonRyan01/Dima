using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditorCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateCategoryRequest Request { get; set; }
    
    [Parameter]
    public string Id { get; set; } = string.Empty;

    #endregion
    
    #region Services
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    [Inject] 
    public ISnackbar Snackbar { get; set; } = null!;
    
    #endregion
    
    #region Overrides

    protected async override Task OnInitializedAsync()
    {
        GetCategoryByIdRequest? category = null;
        try
        {
            category = new GetCategoryByIdRequest
            {
                Id = long.Parse(Id)
            };
        }
        catch
        {
            Snackbar.Add("Parametro invalido.", Severity.Error);
        }

        if(category == null)
            return;
        
        IsBusy = true;
        try
        {
            var response = await Handler.GetByIdAsync(category);
            if (response.IsSuccess && response.Data != null)
            {
                Request = new UpdateCategoryRequest
                {
                    Id = response.Data.Id,
                    Title = response.Data.Title,
                    Description = response.Data.Description
                };
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}