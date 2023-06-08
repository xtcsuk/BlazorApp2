using BlazorApp2.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Pages
{
    public partial class SelectComponent
    {
        private string _currentValue = string.Empty;

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

        private void ItemClicked(string value)
        {
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
