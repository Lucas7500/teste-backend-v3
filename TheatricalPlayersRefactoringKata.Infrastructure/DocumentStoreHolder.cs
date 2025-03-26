using Raven.Client.Documents;

namespace TheatricalPlayersRefactoringKata.Infrastructure
{
    public static class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new(() =>
            {
                var store = new DocumentStore()
                {
                    Urls = ["http://127.0.0.1:8080"],
                    Database = "TheatricalPlayersDB",
                    Certificate = null
                };

                return store.Initialize();
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}
