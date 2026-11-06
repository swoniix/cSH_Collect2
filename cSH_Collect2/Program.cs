class BrowserHistory
{
    private readonly LinkedList<string> history = new();
    private LinkedListNode<string>? currentPage;

    public BrowserHistory()
    {
    }

    public BrowserHistory(string initialUrl)
    {
        Visits(initialUrl);
    }

    public void Visits(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty.", nameof(url));
        if (currentPage == null)
        {
            currentPage = history.AddLast(url);
            return;
        }
        // удаление страничек
        while (currentPage.Next != null)
            history.Remove(currentPage.Next);
        currentPage = history.AddAfter(currentPage, url);
    }

    public string Back(int steps)
    {
        ValidateSteps(steps);
        while (steps > 0 && currentPage?.Previous != null)
        {
            currentPage = currentPage.Previous;
            steps--;
        }
        return GetCurrentPage();
    }

    public string Forward(int steps)
    {
        ValidateSteps(steps);
        while (steps > 0 && currentPage?.Next != null)
        {
            currentPage = currentPage.Next;
            steps--;
        }
        return GetCurrentPage();
    }

    public string GetCurrentPage()
    {
        return currentPage?.Value ?? "History is empty.";
    }

    public void ShowHistory()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("History is empty.");
            return;
        }
        var pages = new List<string>();
        for (var node = history.First; node != null; node = node.Next)
        {
            pages.Add(node == currentPage
                ? $"[ {node.Value} ]"
                : node.Value);
        }

        Console.WriteLine(string.Join(" -> ", pages));
    }

    private static void ValidateSteps(int steps)
    {
        if (steps < 0)
            throw new ArgumentOutOfRangeException(
                nameof(steps),
                "Steps cannot be negative.");
    }
}

class Program
{
    static void Main()
    {
        BrowserHistory browser = new BrowserHistory("home.com");
        browser.Visits("google.com");
        browser.Visits("github.com");
        browser.ShowHistory();
        Console.WriteLine(browser.Back(1)); // google.com
        browser.ShowHistory();
        browser.Visits("stackoverflow.com");
        browser.ShowHistory();
        Console.WriteLine(browser.Back(2)); // home.com
        browser.ShowHistory();
        Console.WriteLine(browser.Forward(1)); // google.com
        browser.ShowHistory();
    }
}