using BlazorApp2.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp2.Pages
{
    public partial class SelectComponent
    {
        [Parameter]
        public string? AuthorizedRoles { get; set; }

        [Parameter]
        public IEnumerable<SelectModel> Data { get; set; } = new List<SelectModel>();

        [Parameter]
        public string LabelCaption { get; set; } = string.Empty;

        [Parameter]
        public string CurrentValue { get; set; } = string.Empty;

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public string OptionCssClass { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<SelectModel> OnItemClick { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState>? _authenticaionStateTask { get; set; }

        private bool _userHasAuthorization = true;

        private string _currentValue = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (_authenticaionStateTask != null)
            {
                var user = (await _authenticaionStateTask).User;

                if (!string.IsNullOrWhiteSpace(AuthorizedRoles))
                {
                    _userHasAuthorization = user.IsInRole(AuthorizedRoles);
                }
            }
        }

        private void ItemClicked(string value)
        {
            if (!_userHasAuthorization) return;

            if (value != null && !CurrentValue.Equals(value))
            {
                var selectedModel = Data.Single(m => m.Id.Equals(value));
                var returnModel = new SelectModel
                {
                    Id = selectedModel.Id,
                    Disabled = selectedModel.Disabled,
                    Description = selectedModel.Description,
                    Divider = selectedModel.Divider
                };
                if (OnItemClick.HasDelegate)
                {
                    OnItemClick.InvokeAsync(returnModel);
                }
            }
        }

        protected override void OnParametersSet()
        {
            _currentValue = CurrentValue;
            base.OnParametersSet();
        }
    }
}
