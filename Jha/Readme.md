The solution should be ready to go simply by running the project. If there are any issues, be sure to set both the "JhaServer" and "JhaReactWeb" to be startup projects.

You only need to set Twitter:BearerToken app setting in "JhaServer" project, "appsettings.json" file to a valid bearer token for using twitter's API


Some things I would've done different in a real-world scenario:

* obviously have a more persistent store for tweet statistics rather than in-memory
* used a separate service to run the JhaServer that consumes tweets instead of running it with the API project
* utilized a real-time library like SignalR to push statistics to the client instead of polling, perhaps with a Kafka stream
