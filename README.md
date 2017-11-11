Final project repo

# Example model!
Checkout http://localhost:5000/Users for CRUD

You can now use scaffolding for ASP.NET models!

# Dotnet Run
- You can disable autorun by commenting out the section in the Chef `default.rb`


### if autorun enabled:
Please see http://localhost:5000. It may take up to 5 mins to start the dotnet server after the `vagrant up` or `vagrant provision` command completes!! Until that time you will see something like "connection was reset" as an error message.

To restart the server with your code changes:
1. `vagrant halt`
2. `vagrant up`

### if autorun not enabled
1. `vagrant ssh`
2. `cd project/`
3. `dotnet run`

Check localhost:5000 :)



Workflow

when change ASP model

1. first change the data type in controller 
2. run ef dotnet migration add newchangeName
3. run ef database update 
4. run ef dotnet run --project name.csproj