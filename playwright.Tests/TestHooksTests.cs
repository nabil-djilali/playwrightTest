using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playwright.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    internal class TestHooksTests : PageTest
    {
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
            await header.WaitForAsync();  // Await the WaitForAsync method
            var headerText = await header.TextContentAsync();

            // Using NUnit's Assert.That for the assertion
            Assert.That(headerText, Is.EqualTo("Counter"));
        }

        [TearDown]
        public async Task CaptureScreenshotOnFailure()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                await Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"Screenshots/{TestContext.CurrentContext.Test.MethodName}.png"
                });
                Console.WriteLine("Screenshot captured.");
            }
        }

        [TearDown] public async Task CleanupaaAsync() 
        {   await Page.CloseAsync(); 
            await Context.CloseAsync(); 
            await Browser.CloseAsync(); 
        }

        //[Test]
        //public async Task NavigateWithErrorHandlingTest()
        //{
        //    try
        //    {
        //        await Page.GotoAsync("https://example.com");
        //        var header = Page.Locator("h1");
        //        await Expect(header).ToHaveTextAsync("Example Domain");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Test failed: {ex.Message}");
        //    }
        //}

        [Test] public async Task NavigateWithErrorHandlingTest() 
        { 
            try {
                
                await Page.GotoAsync("https://localhost:7076/counter");
                await Page.PauseAsync();
                var header = Page.Locator("h1"); await header.WaitForAsync(); 
                var headerText = await header.TextContentAsync(); 
                Assert.That(headerText, Is.EqualTo("Counter")); 
            } 
            catch (Exception ex) 
            { 
                Console.WriteLine($"Test failed: {ex.Message}"); 
            } 
        }
        [Test]
        [TestCase("https://example.com", "Example Domain")]
        [TestCase("https://playwright.dev", "Playwright")]
        public async Task DataDrivenTest(string url, string expectedTitle)
        {
            await Page.GotoAsync(url);

            // Assert page title
            var actualTitle = await Page.TitleAsync();
            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }

    }
}
