### Shopping Cart App secured with JWT and connected with SQL Server.

The Shopping Cart app provides customers with an intuitive interface to browse and select products effortlessly. Once an item is added to the cart, real-time inventory checks guarantee it is in stock before you checkout. This prevents the irritation of an order being cancelled after the fact because an item is unavailable.
After checkout, you receive a confirmation email with your invoice immediately. This creates a smooth, streamlined experience from start to finish. No more wondering if your order actually went through!

Your privacy is also a top priority. The Shopping Cart app utilizes advanced JWT authorization to keep your personal information safe at all times. You can sign up for an account or simply checkout as a guest.

Behind the scenes, the web app also features a control panel section for super admins to manage the web app. Admins and editors can use this section to manage the inventory and track orders. This allows for efficient and effective management of the app, ensuring that clients have a seamless and hassle-free shopping experience.



</br>

## **Technologies:**
- ASP.NET 7 WebApi
- Database: Microsoft SQL server.
- Framework/ library: Entity framework 

- Generated JWS key : https://8gwifi.org/jwsgen.jsp


## **Features:**
  ### Security:
   - User registration;
   - user login
   - Password hashing;
   - Role-based authorization;
   - Users who signed up authorized by role "User"
   - hide or display different parts of a page based on the user's roles.
   - Endpoints request required authorized access.
   - Login via access token creation;
   - Refresh tokens, to create new access tokens when access tokens expire;
   - cookies to store refresh tokens in it. 
   - Revoking refresh tokens.
   - Secured actions in every controller
   
   
  ### Ordering:
   - Create cart for new registered users once they try to add items to cart
   - Add items to cart.
   - Update quantity of item in the cart.
   - Delete item from cart.
   - Using user's token to add items to cart, delete item, update quantity of item and see items in cart.     
   - Adding items to cart, Delete items and Update quantity effect on the quantity of the  products inventory.
   - Create invoice when user checkout and send it to him in email Using user's token to get his email.
   - Store user's order records in the database.
   - Ability to retrieve order records by the admins of the website authorized by role "Admin" and "SuperAdmin"
   - User ability to retrieve his orders records using token.
   
   
 
 ### Administration:
   - Register admins authorized by role "Admin".
   - Retrieve all the user records.
   - Retrieve user records by email.
   
 

## **Dependencies and packages :**
- Microsoft.AspNetCore.Authorization
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.AspNetCore.Identity
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
- Microsoft.Extensions.Options
- Microsoft.EntityFrameworkCore
- FluentEmail.Core
- FluentEmail.Razor
- FluentEmail.Smtp


</br>

**If you have ideas on how to improve the API or if you want to add a new functionality or fix a bug, please, send a pull request.**
