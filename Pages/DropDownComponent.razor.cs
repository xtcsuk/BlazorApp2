using AntDesign;
using BlazorApp2.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Pages
{
    public partial class DropDownComponent
    {
        [Parameter]
        public string? AuthorizedRoles { get; set; }

        [Parameter]
        public IEnumerable<DropDownModel>? Data { get; set; }

        [Parameter]
        public string ButtonCaption { get; set; } = "No Caption";

        [Parameter]
        public EventCallback<DropDownModel> OnItemClick { get; set; }

        private void OnItemClicked(MenuItem value)
        {
            if (OnItemClick.HasDelegate)
            {
                var model = new DropDownModel
                {
                    Id = value.Id,
                    Disabled = value.Disabled
                };
                var description = Data?.Single(m => m.Id.Equals(model.Id)).Description;
                model.Description = description ?? string.Empty;
                OnItemClick.InvokeAsync(model);
            }
        }
    }
}
