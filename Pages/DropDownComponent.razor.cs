using AntDesign;
using BlazorApp2.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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

        [CascadingParameter]
        private Task<AuthenticationState>? _authenticaionStateTask { get; set; }

        private bool _userIsAuthorized = false;

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrWhiteSpace(AuthorizedRoles))
            {
                _userIsAuthorized = true;
                return;
            }

            if (_authenticaionStateTask != null)
            {
                var user = (await _authenticaionStateTask).User;
                _userIsAuthorized = user.IsInRole(AuthorizedRoles);
            }
        }

        private void OnItemClicked(MenuItem value)
        {
            if (!_userIsAuthorized) return;

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
