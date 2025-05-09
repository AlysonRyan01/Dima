using System.Reflection.Metadata;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListPage : ComponentBase
{
    #region properties

    public bool IsBusy { get; set; } = false;
    public List<Category> Categories { get; set; } = [];
    public string SearchTerm { get; set; } = String.Empty;

    #endregion
    
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllAsync(request);
            if(result.IsSuccess)
                Categories = result.Data ?? [];
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error: {e.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    #endregion
    
    #region Methods

    public async void OnDeleteButtomClicked(long id, string title)
    {
        var result = await DialogService.ShowMessageBox("ATENÇÃO",
            "Ao prosseguir a categoria {title} será excluida. Esta ação é irreversível! Deseja continuar?"
            , yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            var request = new DeleteCategoryRequest { Id = id };
            await Handler.DeleteAsync(request);
            Categories.RemoveAll(x => x.Id == id);
            Snackbar.Add($"Categoria {title} excluída", Severity.Success);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error: {e.Message}", Severity.Error);
        }
    }

    public Func<Category, bool> Filter => category =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;

        if (category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (category.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (category.Description is not null &&
            category.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion
}