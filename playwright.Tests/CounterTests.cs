using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Legacy;


namespace playwright.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CounterTests : PageTest
    {
        [Test]
        public async Task CounterPageIncrementsCounterWhenButtonClicked()
        {
            // Navigate to the Counter page
            await Page.GotoAsync("https://localhost:7076/counter");

            // Wait for the status element to appear
            var counterDisplay = Page.Locator("p[role='status']");
            await counterDisplay.WaitForAsync();

            // Assert the counter starts at 0
            await Expect(counterDisplay).ToHaveTextAsync("Current count: 0");

            // Find and click the "Increment" button
            var incrementButton = Page.Locator("button.btn.btn-primary");
            await incrementButton.ClickAsync();

            // Assert the counter increments to 1
            await Expect(counterDisplay).ToHaveTextAsync("Current count: 1");
        }

        [Test]
        public async Task TestWithCssSelector()
        {
            // Navigate to example.com
            await Page.GotoAsync("https://localhost:7076/counter");

            // Find a header with a CSS selector
            var header = Page.Locator("h1");
            await header.WaitForAsync();

            // Assert the header text
            await Expect(header).ToHaveTextAsync("Counter");
        }
        [Test]
        public async Task TestWithIdSelector()
        {
            await Page.GotoAsync("https://localhost:7076/counter");

            // Example with an ID selector
            var button = Page.Locator("#counterButton");
            button.WaitForAsync();


            // Perform click action
            await button.ClickAsync();
        }
        /// /////////////////////OLD CODE//////////

        //[Test]
        //public async Task TestWithIdSelector2()
        //{
        //    await Page.GotoAsync("https://localhost:7076/counter");

        //    // Example with an ID selector
        //    var button = Page.Locator("#counterButton");
        //    button.WaitForAsync();


        //    // Perform click action
        //    await Expect(button).ToHaveTextAsync("Click me");
        //}

        ///////////////////NEW CODE//////////// 

        [Test]
        public async Task TestWithIdSelector2()
        {
            await Page.GotoAsync("https://localhost:7076/counter");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var button = Page.Locator("#counterButton");
            await button.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            await Expect(button).ToHaveTextAsync("Click me");
        }

        [Test]
        public async Task TestWithXPathSelector()
        {
            // Navigate to the Weather page
            await Page.GotoAsync("https://localhost:7076/weather");

            // Wait for the page to load completely
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Locate the element using XPath
            var element = Page.Locator("//h1[contains(text(), 'Weather')]");

            // Ensure the element is visible
            await element.WaitForAsync();

            // Assert that the element is visible
            await Expect(element).ToBeVisibleAsync();
        }
        // open ne url 
        //[Test]
        //public async Task HandleNewTabTest()
        //{
        //    // Navigate to the initial page
        //    await Page.GotoAsync("https://localhost:7076/counter");

        //    // Locate the link that opens a new tab
        //    var link = Page.Locator("a[href='weather']"); // Adjust selector as per your markup

        //    ClassicAssert.IsNotNull(link);


        //    // Wait for the new tab to open
        //    var newPageTask = Page.Context.WaitForPageAsync();
        //    await link.ClickAsync();

        //    // Await the new page
        //    var newPage = await newPageTask;

        //    // Ensure the new page has fully loaded
        //    await newPage.WaitForLoadStateAsync();

        //    // Assert URL and content on the new page
        //    Assert.That(newPage.Url, Is.EqualTo("https://localhost:7076/weather"));

        //    // Check the header or any content on the new page
        //    var header = newPage.Locator("h1");
        //    await Expect(header).ToHaveTextAsync("Weather");
        //}

        // close page
        [Test]
        public async Task SwitchBetweenPagesTest()
        {
            await Page.GotoAsync("https://localhost:7076/weather");

            // Open a new tab
            var newPage = await Page.Context.NewPageAsync();
            await newPage.GotoAsync("https://localhost:7076/counter");

            // Interact with the first page
            var firstPageHeader = Page.Locator("h1");
            await firstPageHeader.WaitForAsync();
            await Expect(firstPageHeader).ToHaveTextAsync("Weather");

            // Interact with the second page
            var secondPageHeader = newPage.Locator("h1");
            await secondPageHeader.WaitForAsync();
            await Expect(secondPageHeader).ToHaveTextAsync("Counter");
        }


        [Test]
        public async Task ClosePageTest()
        {
            var newPage = await Page.Context.NewPageAsync();
            await newPage.GotoAsync("https://localhost:7076/counter");

            // Close the new page
            await newPage.CloseAsync();
        }

        /////////////////////TEST HOOKS///////////////
        private IPage Page { get; set; }
        private IBrowser Browser { get; set; }
        private IBrowserContext Context { get; set; }

        //// Runs once before any tests
        //[OneTimeSetUp]
        //public async Task GlobalSetupAsync()
        //{
        //    var playwright = await Playwright.CreateAsync();
        //    Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        //}
        // Runs once before any tests
        [OneTimeSetUp]
        public async Task GlobalSetupAsync()
        {
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync(); // Use CreateAsync()
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        }

        // Runs before each test
        [SetUp]
        public async Task SetupAsync()
        {
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
        }

        // Runs after each test
        [TearDown]
        public async Task CleanupAsync()
        {
            if (Page != null)
            {
                await Page.CloseAsync();
            }
            if (Context != null)
            {
                await Context.CloseAsync();
            }
        }

        // Runs once after all tests
        [OneTimeTearDown]
        public async Task GlobalCleanupAsync()
        {
            if (Browser != null)
            {
                await Browser.CloseAsync();
            }
        }

        [Test]
        public async Task ExampleTest()
        {
            await Page.GotoAsync("https://localhost:7076/counter");
            var header = Page.Locator("h1");
            header.WaitForAsync();
            await Expect(header).ToHaveTextAsync("Counter");
        }

    }
}
