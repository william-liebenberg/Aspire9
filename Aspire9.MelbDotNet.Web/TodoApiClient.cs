using Aspire9.MelbDotNet.Shared;

namespace Aspire9.MelbDotNet.Web;

public class TodoApiClient(HttpClient httpClient)
{
    public async Task<TodoEntry[]> GetTodosAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<TodoEntry>? todos = null;

        await foreach (var todo in httpClient.GetFromJsonAsAsyncEnumerable<TodoEntry>("/todos", cancellationToken))
        {
            if (todos?.Count >= maxItems)
            {
                break;
            }

            if (todo is null) continue;
            todos ??= [];
            todos.Add(todo);
        }

        return todos?.ToArray() ?? [];
    }
}
