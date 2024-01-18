# Creating bots in Telegram and Facebook

## Create a bot in Telegram

1. Find @BotFather in the search engine  
   ![Image1](imgs/img1.PNG)

2. In the dialog with him use the command /newbot  
   ![Image2](imgs/img2.PNG)

3. @BotFather suggests naming the bot somehow, we call it whatever we want to call it

4. Next we need to come up with a username for the bot, make sure it ends with bot, for example, test_twenty_one_bot or brand_items_shop_bot. IMPORTANT, the username must be unique, if it suddenly turns out that the username is busy, @BotFather will tell you about it and ask you to come up with another one

5. After creating a bot you need to use the command /mybots, you will see a menu where there will be buttons (in your case, just one), on the button is written the username of the bot, click on the button to open the bot management menu and click API Token, a window with a token appears, click on the token and it is copied, then it should be put aside in some notebook and save for later, because it will come in handy when hosting the bot.  
   ![Image3](imgs/img3.PNG)  
   ![Image4](imgs/img4.PNG)  
   ![Image5](imgs/img5.PNG)  

## Create a Facebook bot

1. Go to https://developers.facebook.com/, look for the Get Started section on the top right, click on  
   ![Image6](imgs/img6.PNG)

2. Register as a developer, go through all the steps, at the last step specify yourself as a Developer  

3. After registration we will be redirected to the application window (if not redirected, go to https://developers.facebook.com/apps/).  
   ![Image7](imgs/img7.PNG)

4. Click on the Create App button, select Other  
   ![Image8](imgs/img8.PNG)

4.1. select the Business application type  
   ![Image9](imgs/img9.PNG)

4.2 Choose the name of the application, for example testing-features-app-bot and create the application (if confirmation is required, enter the password from the page).  
   ![Image10](imgs/img10.PNG)

5. After creating the app, we are taken to the main page of the created app  
   ![Image11](imgs/img11.PNG)

5.1 On the top of the navigation field, hover over the Tools section and select Graph API Explorer.  
   ![Image12](imgs/img12.PNG)  
   ![Image13](imgs/img13.PNG)

5.2 We get to the API testing window  
   ![Image14](imgs/img14.PNG)

5.3 In this section, select the "User or Page" field and select Get Page AccessToken.  
   ![Image15](imgs/img15.PNG)  
   ![Image16](imgs/img16.PNG)

5.4 We get to such a window. In this window you click Continue as ... 
   ![Image16](imgs/img18.PNG)  
   We get to the next window, in which you select your page  
   ![Image16](imgs/img19.PNG)  

5.5. After that you need to select permissions like in these pictures.  
   ![Image17](imgs/extraimg1.PNG)  
   ![Image18](imgs/extraimg2.PNG)  
   (to add permissions click on the Add a Permission button or in the input field above this button manually enter the names of permissions from the pictures.

5.6 After that click Generate Access Token button, once the token is generated this tool can sometimes generate an incorrect token so as in step 5.3 select our page again, after that the token will be updated, nothing needs to be clicked after selecting the page except the blue icon next to the Access Token.  
   ![Image19](imgs/img20.PNG)

5.7. In the window that appears, click the blue button that starts with the words "Extend", click it, a new token appears, copy it to the same place where you saved the token from the Telegram bot.  
   ![Image20](imgs/img21.PNG)

6. Go to your page, to which we want to parse data from the telegram channel.

7. Select the About tab  
   ![Image21](imgs/img22.PNG)

8. Select the Page Transparency section and look for Page Id there, copy it to the same place where you saved all tokens.  
   ![Image22](imgs/img23.PNG)

9. Preparations are ready! Now we need to install the bot using these data!
