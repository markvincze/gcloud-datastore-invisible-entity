using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;

namespace GcloudDatastoreInvisibleEntity
{
    class Program
    {
        private const string gcloudProject = "my-gcloud-project";
        private const string gcloudDatastoreNamespace = "invisible-entity-test";

        private const string entityKind = "TestEntity";
        private const string entityKey = "EntityKey";

        static void Main(string[] args)
        {
            for(int i = 0; i < 20; i++)
            {
                Cleanup();
                DoTest().Wait();
                Console.WriteLine("Iteration {0} done.", i);
            }
        }

        private static void Cleanup()
        {
            var db = DatastoreDb.Create(gcloudProject, gcloudDatastoreNamespace);

            bool done = false;

            while (!done)
            {
                Query query = new Query(entityKind);

                var versions = db.RunQuery(query);

                // NOTE Google Datastore doesn't allow to delete more than 500 entities in one operation.
                db.Delete(versions.Entities.Take(500).ToList());

                done = versions.Entities.Count <= 500;
            }

        }
        
        private static async Task DoTest()
        {
            var datastoreDb = DatastoreDb.Create(gcloudProject, gcloudDatastoreNamespace);
            
            using(var tran = await datastoreDb.BeginTransactionAsync())
            {
                // Insert a test entity.
                var keyStr = Guid.NewGuid().ToString();
                Key key = datastoreDb.CreateKeyFactory(entityKind).CreateKey(keyStr);

                var entity = new Entity
                {
                    Key = key,
                    ["bar"] = "foo"
                };
                
                tran.Upsert(entity);
                await tran.CommitAsync();
                
                // Entity inserted, check if we can read it.
                var query = new Query(entityKind);
                
                var entities = (await datastoreDb.RunQueryAsync(query)).Entities;

                if (!entities.Any(e => e.Key.Path.Last().Name == keyStr))
                {
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("WARNING: Version was not found after insertion!!!");
                    Console.WriteLine("-------------------------------------------------");
                }
            }
        }
    }
}
