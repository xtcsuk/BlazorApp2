using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp2.Pages
{
    public partial class CustomAutoCompleteComponent
    {
        private string? _selectedOption;

        [Parameter]
        public EventCallback<string> OnInputCallback { get; set; }

        [Parameter]
        public EventCallback<string> OnSelectedOptionCallback { get; set; }

        [Parameter]
        public string Placeholder { get; set; } = "Placeholder not set";

        [Parameter]
        public IEnumerable<string>? OptionList { get; set; }

        private async Task DoOnInputCallBack(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && OnInputCallback.HasDelegate)
            {
                await OnInputCallback.InvokeAsync(value);
            }
        }

        private async Task DoOnSelectedOptionCallback(string selectedOption)
        {
            if (!string.IsNullOrWhiteSpace(selectedOption) && OnSelectedOptionCallback.HasDelegate)
            {
                await OnSelectedOptionCallback.InvokeAsync(selectedOption);
                if (OptionList?.Count() == 1)
                {
                    _selectedOption = OptionList.First();
                    OptionList = null;
                }
            }
        }

        private void OnKeyUp(KeyboardEventArgs e)
        {
            if (e.Key.Equals("escape", StringComparison.OrdinalIgnoreCase))
            {
                _selectedOption = null;
                OptionList = null;
            }
        }
    }
}
