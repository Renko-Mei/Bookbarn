# Introduction
BookBarn is a web service to connect buyers and sellers of used books. We have currently implemented the functionality listed below.

#Login Hint
    We provide a superAdmin here just in case you cannot register:
        UserName: SuperAdmin
        email: superadmin@test.com
        password: Piranha$94
    Howerver, we don't recommand you do that as the email address is a fake one, if you want to try our email system, we highly recommand you register a new account with your real email address.


# Functions

### User Authentication & login
1. We set up `email verification system` when people `register in`. User will receive email varidication to confirm their identity. This means that you must use your` REAL EMAIL ADDRESS` to register!
2. Also set up email verification when users `forget their password`. They have to go their registered email account and check the email sent from `"info@bookbarncanada.com"`
3. After register, when user type the wrong userName or password, they will only be notified as `"invalid login attemp"` instead of specifically telling user whether their password or userName isw'wrong. This is because of the security concern as we don't know who is attemping to login to your account, this increases the difficulty when other people try to login into your account.
4. The `password` requires both Capital letter and digital number with a minimum lenght of 8 characters.
5. The `username` is letter only and no space nor digit allowed.
6. Both `userName` and `email address` are unique.

### AccountInfo
After user login, your name will appear in the navbar. Click your name to see the dropdown and select the Account information you need.
Customers can use AccountInfo to 
1. change their profile, 
2. reset their password
3. edit/create address,
4. see their order history and 
5. sales history

### SaleItems
Click the `sell button` in the navbar to sell your books.
1. provide a `REAL ISBN` number and click the `check ISBN` button on the right side.With the help of googlebook API, all the book information will be loaded automatically.
2. Although the API already provided a cover page for your book, you can also choose to upload an customized picture.
3. Since there are usually two ISBN numbers for each book, once you enter one ISBN number, we will save another ISBN number and no matter what ISBN is used by customers, they can always find your book.
4. Choose the book quality and enter the price that you want to sell.
6. Click `create` and you will be direct to your personal saleItem page and other people don't have permission to view your saleItem page.
7. You can edit/create/delete the saleItems, however, once the Item is sold, it cannot be deleted nor edited. This is designed to save as a sale history.

### SalesItem/Search
Click on the `search button` in the navbar or use the search function in the index page under the brand image.
1. go to http://localhost:5000/SaleItems/Search and all the selling bookings will appear there. However, once the book is sold, they will never appear again.
1. Customers can search by title, ISBN number or author name.
2. Customer can also search by `advanced search` which will help to get more specific search result
3. Customers can determine the order of their search result through `sort by` function. 
    * The `sort` function can only be used before you click the search button.
4. There's also a simplified search bar in the index page without sort function.

### SalesItem/SearchDetails
Cutomsers can see the details of the book information to decide if it is excatly the book that they wanted.
1. From this page, they can also contact the seller, the seller email and your email will be automatically inserted. All you need is to type in your concern.
2. The customer information including customer email, name and phone number will embedded into the email and sent to seller.


### Group_Chat
If User cannot find the book from our webapp, they can talk in the group-chat ask if anyone has the specific book. Click on the `Chat` button on the `navigation bar` to enter into the chatroom.
1. User has to login to use the chatRoom
2. They can view chat history by scrolling up in the chat area
3. online user's name is displayed on the left side
4. System will notify everyone once somebody enter/leave the chat room.
5. They can send emoji while chating.
6. For chatRoom, we use websocket instead of socket.io. As socket.io doesn't support asp.net core very well. 

References:
    We learn websocket mostly from these two articles:
        1. http://gunnarpeipman.com/2017/03/aspnet-core-websocket-chat/
        2. https://radu-matei.com/blog/real-time-aspnet-core/
    The gif image that used as emoji was from a jquery library called: qqFace -->https://github.com/kyo4311/jquery.qqface 



### See Sales History
In http://localhost:5000/user/salesViz , the user can access the sales history of all the items they have sold in a nice visual chart


### Data model created
We have created our data model & set up the entities and relationships.

# How to run
### To interact with the web application
 - Build and run the web application, the instruction is stated in the below section.
 - Open `http://localhost:5000` in your local browser.

### Run locally
 - Open up a command line window.
 - Route to `./src` folder.
 - Run `dotnet run --project BookBarn.csproj`.

### Run in Vagrant with auto run enabled (default)
 - Open up a command line window.
 - Route to `./src` folder.
 - Run `Vagrant up` and wait until the VM is ready to serve.
   It may take up to 5 mins to start the dotnet server after the `vagrant up` or `vagrant provision` command completes!!

### Run in Vagrant with auto run disabled
 - You can disable autorun by commenting out the section in the Chef `default.rb`
 - Open up a command line window
 - Route to `./src` folder
 - Run `Vagrant up` and wait until the VM is ready to serve.
 - After Vagrant successfully provisioned the VM, run `vagrant ssh` to bring up the Linux shell of the VM.
 - Run `cd project` followed by `dotnet run --project BookBarn.csproj`.

### To restart the server with your code changes:
1. `vagrant halt`
2. `vagrant up`

# Technology Stack
 - ASP.NET Core 2.0 MVC
 - PostgreSQL
 - jQuery
 - Bootstrap
 - Chart.js
 - Webpack
 - Babel
 - Vagrant
 - WebSocket
 - MailKit
 - GoogleBookAPI

# Workflow

when change ASP model

1. first change the data type in controller
2. run ef dotnet migration add newchangeName
3. run ef database update
4. run ef dotnet run --project name.csproj


