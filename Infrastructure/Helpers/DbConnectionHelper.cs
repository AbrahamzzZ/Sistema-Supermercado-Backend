using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Helpers
{
    public static class DbConnectionHelper
    {
        public static async Task<IAsyncDisposable> OpenConnectionScopeAsync(DbContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var closeConnectionOnDispose =
                context.Database.GetDbConnection().State != ConnectionState.Open;

            if (closeConnectionOnDispose)
            {
                await context.Database.OpenConnectionAsync();
            }

            return new ConnectionScope(context, closeConnectionOnDispose);
        }

        private sealed class ConnectionScope : IAsyncDisposable
        {
            private readonly DbContext _context;
            private readonly bool _closeConnectionOnDispose;

            public ConnectionScope(DbContext context, bool closeConnectionOnDispose)
            {
                _context = context;
                _closeConnectionOnDispose = closeConnectionOnDispose;
            }

            public async ValueTask DisposeAsync()
            {
                if (_closeConnectionOnDispose)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }
        }
    }
}
