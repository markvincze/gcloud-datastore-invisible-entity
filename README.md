# gcloud-datastore-invisible-entity
Some test code illustrating how we sometimes cannot retrieve commited entities from Google Datastore.

## Usage
Edit the values of the `gcloudProject` and `gcloudDatastoreNamespace` fields to contain your Gcloud project name and the namespace you want to use, then simply build and run the project from the `/src/GcloudDatastoreInvisibleEntity` folder.

```
dotnet restore
dotnet run
```

In the output you'll see a warning if the commited entity could not be read with a query afterwards. An example output looks like this.

```
Iteration 0 done.
Iteration 1 done.
-------------------------------------------------
WARNING: Version was not found after insertion!!!
-------------------------------------------------
Iteration 2 done.
Iteration 3 done.
Iteration 4 done.
Iteration 5 done.
-------------------------------------------------
WARNING: Version was not found after insertion!!!
-------------------------------------------------
Iteration 6 done.
Iteration 7 done.
Iteration 8 done.
Iteration 9 done.
Iteration 10 done.
Iteration 11 done.
Iteration 12 done.
Iteration 13 done.
Iteration 14 done.
Iteration 15 done.
Iteration 16 done.
Iteration 17 done.
Iteration 18 done.
Iteration 19 done.
```