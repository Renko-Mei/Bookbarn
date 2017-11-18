#BookBarn
Bookbarn is a web service to connect buyers and sellers of used books. We have currently implemented the functionality listed below.

Future iterations include book item listing pages, admin dashboard, email notifications and search for books.

# Functions:

### Private_Chat
The private_Chat function is created for customers to directly negotiate with sellers. To make the function work, we use websocket and a middleware is also created. This function has not embedded and is still under developing. However, the basic functions are working now. You can go to `localhost:5000/chat` to play with it.
1. you can open multiple pages (`localhost:5000/chat`) in your browser and enter different user name.
2. send message to specific user that you want to talk to.
3. Only you(sender) and the receiver will receive message while other users will receive nothing.

### Search
See http://localhost:5000/search for search bar and dropdown bar. Table gets populated if there are entries in the database.

### See Sales History
In http://localhost:5000/user/salesViz , the user can access the sales history of all the items they have sold in a nice visual chart

### User Authentication & login


### Data model created
We have created our data model & set up the entities and relationships.


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








# Workflow

when change ASP model

1. first change the data type in controller
2. run ef dotnet migration add newchangeName
3. run ef database update
4. run ef dotnet run --project name.csproj

Since we are working with typescript here, any javascript libraries need to be installed like this:

    npm install -S @types/libraryName
