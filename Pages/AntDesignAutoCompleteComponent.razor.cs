using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp2.Pages
{
    public partial class AntDesignAutoCompleteComponent
    {
        private string? value;
        private IEnumerable<string>? _data;

        [Parameter]
        public string? PlaceholderCaption { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        [Parameter]
        public EventCallback<string> OnSelectionChange { get; set; }

        [Parameter]
        public IEnumerable<string> Data { get; set; } = new List<string>();

        private AutoComplete<string>? _autoComplete;

        protected override void OnParametersSet()
        {
            _data = Data;
        }
        async Task OnChangeAsync(AutoCompleteOption item)
        {
            value = (string)item.Value;

            if (OnSelectionChange.HasDelegate)
            {
                await OnSelectionChange.InvokeAsync(value);
                //_autoComplete?.
                //await _autoComplete?.InputFocus(args);
                _autoComplete!.ShowPanel = true;
                var args = new KeyboardEventArgs { Key = "ArrowDown" };
                await _autoComplete!.InputKeyDown(args);
                value = string.Empty;
            }
        }

        async Task OnInputChangeAsync(ChangeEventArgs e)
        {
            var newValue = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(newValue) || !OnInput.HasDelegate)
            {
                return;
            }

            await OnInput.InvokeAsync(newValue);
            //StateHasChanged();
        }
    }
}
