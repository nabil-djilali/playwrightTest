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
    internal class DataDrivenTests : PageTest
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "https://localhost:7076/counter", "Counter" };
            //yield return new object[] { "https://playwright.dev", "Playwright" };
            yield return new object[] { "https://localhost:7076/weather", "Weather" };
        }


        [Test]
        [TestCaseSource(nameof(TestData))]
        public async Task DataDrivenWithSourceTest(string url, string expectedTitle)
        {
            await Page.GotoAsync(url);

            // Wait for the page to load completely
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Log page content for debugging
            var content = await Page.ContentAsync();
            Console.WriteLine($"Page content for {url}:\n{content}");

            // Wait for specific selector if necessary
            await Page.WaitForSelectorAsync("h1");

            // Fetch and log the title
            var actualTitle = await Page.TitleAsync();
            Console.WriteLine($"Fetched title for {url}: {actualTitle}");

            // Assert the title matches
            Assert.That(actualTitle, Is.EqualTo(expectedTitle), $"Title mismatch for URL: {url}");
        }


        public static IEnumerable<object[]> TestDataOne()
        {
            yield return new object[] { "https://example.com", "Example Domain" };
            yield return new object[] { "https://playwright.dev", "Playwright" };
        }

        [Test]
        [TestCaseSource(nameof(TestDataOne))]
        public async Task DataDrivenWithSourceTestOne(string url, string expectedTitle)
        {
            await Page.GotoAsync(url);
            // Wait for the page to load completely
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            // Assert page title
            var actualTitle = await Page.TitleAsync();
            Assert.That(actualTitle, Is.EqualTo(expectedTitle), $"Title mismatch for URL: {url}");
        }




    }
}
