Beerbase
========

Sample ASP .NET Core project.

The project uses EFCore (over SQLite) as a repository. As the project is quite small,
it's possible to mock this directly rather than use a separate repository layer.

Building & Running
==================

Build the solution as usual. Create a local copy of the database in Package manager console:

```
cd beerbase
dotnet ef database update
```

The database is SQLite, created in local appdata. Run the project as usual e.g. F5 in Visual Studio.

Further Work
============

- Clarify requirements
  - should it be possible to create a beer without a brewery?
  - API schema perhaps implies many-many brewery/beers - contrasts with db schema
  - should beer return brewery details
  - validation - allow addition of bar/brewery/beer with same name?
- Static analysis
- Lint (stylecop)
- Increase unit test coverage
  - add metrics
  - extend to bar/barbeer
- Add integration/smoke tests
- review logging (vestigal from template)