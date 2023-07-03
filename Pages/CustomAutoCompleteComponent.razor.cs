using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Pages
{
    public partial class CustomAutoCompleteComponent
    {
        [Parameter]
        public EventCallback<string> OnInputCallback { get; set; }

        [Parameter]
        public EventCallback<string> OnSelectedOptionCallback { get; set; }

        [Parameter]
        public string Placeholder { get; set; } = "Placeholder not set";

        [Parameter]
        public IEnumerable<string>? OptionList { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
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
                    selectedOption = OptionList.First();
                }
            }
        }

        IEnumerable<string>? customers;
        string? selectedCustomerId;
        string? selectedOption;

        //async Task HandleInput(ChangeEventArgs e)
        //{
        //    filter = e.Value?.ToString();
        //    if (filter != null)
        //    {
        //        customers = await postcodeSearchService.GetDataAsync(filter, 10);
        //    }
        //}

        void Test(ChangeEventArgs e)
        {

        }
        void SelectCustomer(string id)
        {
            selectedCustomerId = id;
            selectedOption = id;
            customers = null;
        }
    }
}
