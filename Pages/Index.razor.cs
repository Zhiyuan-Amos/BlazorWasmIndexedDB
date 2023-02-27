using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWasmIndexedDB.Pages;

public partial class Index
{
    private List<Parent>? _parents;
    private long _elapsed;

    [Inject] private IJSRuntime Js { get; init; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var sw = new Stopwatch();
        sw.Start();

        _parents = await Js.InvokeAsync<List<Parent>>("getParents");
        
        _elapsed = sw.ElapsedMilliseconds;

        if (!_parents.Any())
        {
            _parents = SeedData();
            await Js.InvokeVoidAsync("createParents", _parents);
        }

        List<Parent> SeedData()
        {
            List<Parent> toReturn = new();
            var childId = 0;
            for (var parentId = 1; parentId <= 20; parentId++)
            {
                var children = new List<Child>();
                for (var j = 0; j < 20; j++)
                {
                    childId++;
                    children.Add(new Child { Id = childId, ChildValue = "child" });
                }

                toReturn.Add(new Parent { Id = parentId, ParentValue = "parent", Children = children });
            }

            return toReturn;
        }
    }

    private async Task OnExportSelected() => await Js.InvokeVoidAsync("exportDatabase", Constants.DatabaseFileName);
    private async Task OnDeleteDatabaseSelected() => await Js.InvokeVoidAsync("deleteDatabase");
}