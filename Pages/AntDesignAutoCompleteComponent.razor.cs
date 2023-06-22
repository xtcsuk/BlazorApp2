using AntDesign;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Pages
{
    public partial class AntDesignAutoCompleteComponent
    {
        private string? value;

        [Parameter]
        public string? PlaceholderCaption { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        [Parameter]
        public IEnumerable<string> Data { get; set; } = new List<string>();

        void OnSelectionChange(AutoCompleteOption item)
        {
            value = (string)item.Value;
        }

        async Task OnInputChangeAsync(ChangeEventArgs e)
        {
            string varlue = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(value) || !OnInput.HasDelegate)
            {
                return;
            }

            await OnInput.InvokeAsync(value);
        }

    }
}
