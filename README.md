# Coin - The Virtual Stock Market

Coin is a virtual stock market. Everything about the shares on this market are real and up-to-date except the money. 
Users use virtual money to buy shares and get the same type of money when they sell them. 
Because the transactions and the money are not real, the users have nothing to earn or lose in real world.
This application is essentially meant for users that are interested in the stock market but do not know much about it, or do not have any experience in the market.

## Features

Users can

- Create account
- Login/logout
- Update password
- Reset Password
- Buy shares
- Sell shares
- Quote for a share
- Review their transaction details
- Review their account summary such as the shares they own, the available cash, the total fund etc.
- Add fund
- Delete account

## Technologies

This application was created on Visual Studio 2019

- C#
- ASP.NET Core 
- JavaScript
- HTML
- CSS
- SQL
- Bootstrap

## Possible Improvements

- The email addresses of the users are confirmed by default. A confirmation email can be sent to users to confirm their emails addresses.
- When users attempt to reset their passwords, it sends the email address and the token to the reset password page. A reset password email can be sent to users to authenticate them. 
- The check out functionality only adds the amount to the users' fund, and even if the users enter their correct payment information, no fee can be charged from their card right now. This could be completed to carry out the full transaction.
- If a user buys shares from the same company again and again, instead of updating the number of the shares on the database, it creates a new entry for each transaction. Becuase of that, shares of the same company may not be sold together at once. For example, if user buys 2 shares and 3 shares of the same company at different times, the user cannot sell the 5 shares at once. They must sell them seperately. This could be improved to allow users to sell as many shares as they want to sell from the same company at once.

