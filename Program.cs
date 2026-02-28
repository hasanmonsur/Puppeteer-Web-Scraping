// See https://aka.ms/new-console-template for more information
using PuppeteerSharp;
using System.IO;

Console.WriteLine("Puppeteer Automation Started");

try
{
    // Download Chromium if not already present
    await new BrowserFetcher().DownloadAsync();

    // Launch browser with proper error handling
    var browser = await Puppeteer.LaunchAsync(new LaunchOptions
    {
        Headless = true,
        Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
    });

    // Open new page
    var page = await browser.NewPageAsync();

    // Navigate to URL with timeout
    try
    {
        await page.GoToAsync("https://neitsbd.com", new NavigationOptions { Timeout = 30000 });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Navigation failed: {ex.Message}");
        await browser.CloseAsync();
        return;
    }

    // Take screenshot with validation
    string screenshotPath = "screenshot.png";
    if (!Path.GetInvalidFileNameChars().Any(c => screenshotPath.Contains(c)))
    {
        await page.ScreenshotAsync(screenshotPath);
        Console.WriteLine($"Screenshot saved to: {screenshotPath}");
    }
    else
    {
        Console.WriteLine("Invalid screenshot path");
    }

    // Get page title
    var title = await page.GetTitleAsync();
    Console.WriteLine($"Page title: {title}");

    // Close browser
    await browser.CloseAsync();
    Console.WriteLine("Browser closed successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Puppeteer Automation Completed");
}
