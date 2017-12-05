# Introduction
BookBarn is the exciting new web service to connect buyers and sellers of used books. We have currently implemented the functionality listed below.

# How to run
## To interact with the web application
 - Build and run the web application, the instruction is stated in the below section.
 - Open `http://localhost:5000` in your local browser.

## Run locally
 - Open up a command line window.
 - Route to `./src` folder.
 -  `dotnet restore`
 -  `npm install`
 - Run `dotnet run --project BookBarn.csproj`.

## Run in Vagrant with auto run enabled (default)
 - Open up a command line window.
 - Route to `./src` folder.
 - Run `Vagrant up` and wait until the VM is ready to serve.
   It may take up to 20 mins to start the dotnet server after the `vagrant up` or `vagrant provision` command completes!!

## Run in Vagrant with auto run disabled
 - You can disable autorun by commenting out the section in the Chef `default.rb`
 - Open up a command line window
 - Route to `./src` folder
 - Run `Vagrant up` and wait until the VM is ready to serve.
 - After Vagrant successfully provisioned the VM, run `vagrant ssh` to bring up the Linux shell of the VM.
 - Run `cd project` followed by `dotnet run --project BookBarn.csproj`.


### Login
We provide a SuperAdmin here just in case you cannot register:
1. UserName: `SuperAdmin`
2. email: `superadmin@test.com`
3. password: `Piranha$94`
If you want to try our email system, we highly recommend you register a new account with a valid email address.

###  Seed Data
We don't provide seed data here due to the User Id problem. Asp.net Core automatically generates unique UserId for each user account and the fake seed data will face too many conflict.

###  ISBN number example:
Here we provided you some real ISBN numbers you may want to use them for testing:
1. `1617292575`
2. `1491916753`
3. `1491901942`
4. `1430264489`

###  vagrant up
1. It may take `15-20 minutes` to run the vagrant and may another another `3 mintues` to wait the `localhost:5000/` reload
2. The project is running production mode in the dotnet server


# Functions

## User Authentication & login
1. We set up `email verification system` when people `register`. User will receive email verification to confirm their identity. This means that you must use your `REAL EMAIL ADDRESS` to register!
2. There is email verification when the registration is complete. When users `forget their password`. They have to go their registered email account and check the email sent from `"info@bookbarncanada.com"`
3. When users type the wrong user name or password, they will only be notified `"invalid login attempt"` instead of which of their password or user name is wrong. This is because of the security concern as we don't know who is attempting to login to the user account.
4. `password` requires both Capital letter and digital number with a minimum lenght of 8 characters.
5. `username` is letter only and no space nor digit allowed.
6. Both `userName` and `email address` are unique.

## AccountInfo
After user login, their name will appear in the navbar. Click on their name to see the dropdown and select the Account information they need.
Customers can use AccountInfo to 
1. change their profile, 
2. reset their password
3. edit/create address,
4. see their order history with information about total purchase cost and purchased date 
5. sales history
    the user can access the sales history of all the items they have sold in a visual chart

## SaleItems
Click the `sell button` in the navbar to sell their books.
1. provide a `REAL ISBN` number and click the `check ISBN` button on the right side. With the help of googlebook API, all the book information will be loaded automatically.
2. Although the API already provided a cover page for their book, they can also choose to upload an customized picture.
3. Since there are usually two ISBN numbers for each book, once you enter one ISBN number, we will save another ISBN number and no matter what ISBN is used by customers, they can always find their book.
4. Choose the book quality and enter the price that you want to sell.
6. Click `create` and you will be direct to your personal sale item page. Proper authentication is needed to access this page.
7. You can edit/create/delete the sale item, however, once the item is sold, it cannot be deleted nor edited. This is designed to save as a sale history.

## SalesItem/Search
Click on the `search button` in the navbar or use the search function in the index page under the brand image.
1. go to http://localhost:5000/SaleItems/Search and all the listed books will appear there. Of course, once the book is sold, they will never appear again.
1. Customers can search by title, ISBN number or author name.
2. Customer can also search by `advanced search` which will help to get more specific search result
3. Customers can determine the order of their search result through `sort by` function. 
    * The `sort` function can only be used before you click the search button.
4. There's also a simplified search bar in the index page without sort function.

## SalesItem/SearchDetails
Cutomsers can see the book information to decide if it is excatly the book that they wanted.
1. From this page, they can also contact the seller, the seller email and your email will be automatically inserted. All you need is to type in your inquiry.
2. The customer information including customer email, name and phone number will embedded into the email and sent to seller.

## Group_Chat
If User cannot find the book from our webapp, they can talk in the group-chat ask if anyone has the specific book. Click on the `Chat` button on the `navigation bar` to enter into the chatroom.
1. User has to login to use the chatRoom
2. They can view chat history by scrolling up in the chat area
3. online user's name is displayed on the left side
4. System will notify everyone once somebody enter/leave the chat room.
5. They can send emoji while chating.
6. For chatRoom, we use websocket instead of socket.io. As socket.io doesn't support asp.net core very well. 

### References:
    We learn websocket mostly from these two articles:
        1. http://gunnarpeipman.com/2017/03/aspnet-core-websocket-chat/
        2. https://radu-matei.com/blog/real-time-aspnet-core/
    The gif image that used as emoji was from a jquery library called: qqFace -->https://github.com/kyo4311/jquery.qqface 

## ShoppingCart http://localhost:5000/ShoppingCarts
1. User can add the books that they wanted to their shopping cart only after login.

## Checkout  http://localhost:5000/Orders/Checkout
Once user decide to checkout, they will be asked to enter into their address information. 
2. If they already saved their address previously, their address information will be automatically there and no need to type in again.
3. Even though customers have already saved their address, they can change their address there and the address changes will be updated in the database automatically.
3. If it is the first time to enter address, the address will be saved automatically once user clicks the checkout button. 
4. Once the checkout button is clicked, 
    1. books in the shopping cart will be deleted. 
    2. order history will be updated
    3. seller's sale histroy with visual chart will also be updated
    4. seller will receive an email with the buyer information and purchased item information.
    5. purchased item will be deleted from the search result and other people won't find it anymore

## Paypal
We have scaffolded a service for using paypal for payment, however, we didn't continue finishing this completely. As we decided not using real paypal due to extra security issues.

# What is not working
Before starting the project, we hoped that people can search the course book by typing course title in the search box. However, we failed to do so as the API provided by school does not provide all course informations. The API that we planned to use is SFU library API, it can only supply the ISBN number for the text books that are currently in the library.
We have thought an alternative way. As school book store provides a book list for all courses at the beginning of each semester. We can do it manually instead of using APIs. This can be implemented in future versions of this project.

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





