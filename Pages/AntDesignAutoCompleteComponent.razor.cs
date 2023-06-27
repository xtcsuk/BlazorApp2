﻿using AntDesign;
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
        public EventCallback<string> OnSelectionChange { get; set; }

        [Parameter]
        public IEnumerable<string> Data { get; set; } = new List<string>();

        async Task OnChangeAsync(AutoCompleteOption item)
        {
            value = (string)item.Value;

            if (OnSelectionChange.HasDelegate)
            {
                await OnSelectionChange.InvokeAsync(value);
                StateHasChanged();
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
            StateHasChanged();
        }
    }
}
