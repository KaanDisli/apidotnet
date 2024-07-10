

import logging
import telebot 
import config
import requests
import json
#from telegram import Update, ForceReply, InlineKeyboardMarkup, InlineKeyboardButton, ParseMode 
#from telegram.ext import Updater, CommandHandler, MessageHandler, Filters, CallbackContext, CallbackQueryHandler
Username = "@Library_BBBBot"


url = "http://127.0.0.1:5011"
token = "7422340883:AAE1VRKkKT4_djXtW6CpRGFDj8MKUaXI-rQ"
bot = telebot.TeleBot(token)
def extract_arg(arg):
    return arg.split()[1:]

#The message to be once the bot gets started
@bot.message_handler(commands=(["start"]))
def start_command(message):

    button1 = telebot.types.InlineKeyboardButton(text="getbook", callback_data="getbook") 
    button2 = telebot.types.InlineKeyboardButton( 
        text="add", callback_data="add") 
    button3 = telebot.types.InlineKeyboardButton(text="modify", callback_data="modify") 
    button4 = telebot.types.InlineKeyboardButton( 
        text="del", callback_data="del") 
    button5 = telebot.types.InlineKeyboardButton(text="getcategory", callback_data="getcategory") 
    button6 = telebot.types.InlineKeyboardButton(text="getserialnumber", callback_data="getserialnumber") 
    keyboard = telebot.types.InlineKeyboardMarkup()
    keyboard.add(button1, button2, button3,button4,button5,button6) 
    bot.send_message(message.chat.id,"You have started the Library Bot, how may I assist you today? To see how to use commands use /help ",reply_markup=keyboard)


@bot.callback_query_handler(func=lambda call:True)
def handle_button(call):
    if call.data == "getbook":
        bot.send_message(call.message.chat.id," use /getbook <bookID>\n Example: /getbook 5")
    if call.data == "getcategory":
        bot.send_message(call.message.chat.id,"Enter a book category\n Example: /getcategory Novel")
    if call.data == "add":
        bot.send_message(call.message.chat.id,"Enter your user ID,a book name, author, price, category and serialNumber\n Example: /add 1 White_Fang Jack_London 10 Novel 978-0-306-40615-9")
    if call.data == "modify":
        bot.send_message(call.message.chat.id,"Enter a book ID , your user ID, a parameter and new value\n Example: /modify 4 1 price=0 ")
    if call.data == "del":
        bot.send_message(call.message.chat.id,"Enter a book ID and your user ID\n Example: /del 4 1 ")
    if call.data == "getserialnumber":
        bot.send_message(call.message.chat.id,"Enter a book serial number \n Example: /getserialnumber 70")

#/help with link to Simbrella website
@bot.message_handler(commands=(["help"]))
def help_command(message):
    keyboard = telebot.types.InlineKeyboardMarkup()
    keyboard.add(telebot.types.InlineKeyboardButton('Contact us if you need any help',url="https://www.simbrella.com/" ))
    bot.send_message(message.chat.id,"1- To get information about a specific book use /getbook <bookID>\n 2- To get information about a category of books use /getcategory <categoryName>\n 3- To delete a book use /del <bookID> <userID>\n 4- To modify a book use /modify <bookID> <userID> <parameter>=<newValue>\n 5- To add a book /add <user_id> <title> <author> <price> <category>\n 6- To get a book by it's serial number use /getserialnumber <serial_number>",reply_markup=keyboard)

@bot.message_handler(commands=(["getserialnumber"]))
def getserialnumber_command(message):
    try:
        parameters = extract_arg(message.text)
        if parameters == []:
            req_response = "Please enter a serial number"
        else:
            serialNumber = parameters[0]
            req_response = requests.get(url + "/api/get/serialNumber/" + serialNumber)
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"Error")
#/getBook <bookID>
@bot.message_handler(commands=(["getbook"]))
def getBook_command(message):
    try: 
        parameters = extract_arg(message.text)
        if parameters == []:
            req_response = "Please enter a bookID"
        else:
            bookID = parameters[0]
            req_response = requests.get(url + "/api/get/book/" + bookID)
            
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"error")

#/getCategory <category_name>
@bot.message_handler(commands=(["getcategory"]))
def getCategory_command(message):
    try:
        parameters = extract_arg(message.text)
        if parameters == []:
            req_response = "Please enter a category"
        else:
            category = parameters[0]
            req_response = requests.get(url + "/api/get/category/" + category)
        if req_response == []:
            bot.send_message(message.chat.id,"No books in this category") 
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"Error")
#/del <book_id> <user_id>
@bot.message_handler(commands=(["del"]))
def delete_command(message):
    try:
        parameters = extract_arg(message.text)
        if len(parameters) < 2:
            req_response = "Missing parameters please enter a bookID and your userID"
        else:    
            bookID = parameters[0]
            userID = parameters[1]
            req_response = requests.delete(url + "/api/delete/" + bookID + "/" + userID )
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"Error")


#/modify <bookID> <parameter>=<newValue>
@bot.message_handler(commands=(["modify"]))
def update_command(message):
    try:
        parameters = extract_arg(message.text)
        if len(parameters) < 3:
            req_response = "Missing parameters please enter a bookID , your userID and the values to be modified"
            
        else: 
            bookID = parameters[0]
            userID = parameters[1]
            modification = parameters[2]
            parts = modification.split("=")
            key = parts[0]
            value = parts[1]
            dico = {key: value}
            req_response = requests.put(url + "/api/update/" + bookID + "/" + userID,json=dico)
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"Error")
#/add <user_id> <title> <author> <price> <category>
@bot.message_handler(commands=(["add"]))
def add_command(message):
    try:
        parameters = extract_arg(message.text)
        if len(parameters) < 6:
            req_response = "Missing parameters please enter <user_id> <title> <author> <price> <category> <serialNumber>"
        else:
            userID = parameters[0]
            title = parameters[1]
            author = parameters[2]
            price = parameters[3]
            category = parameters[4]
            serialNumber = parameters[5]
            dico = {}
            dico["title"] = title
            dico["author"] = author
            dico["price"] = price
            dico["category"] = category
            dico["serialNumber"] = serialNumber
            req_response = requests.post(url +"/api/add/" + userID,json=dico)
        
        
        bot.send_message(message.chat.id,req_response.text)
    except:
        bot.send_message(message.chat.id,"Error")

print("Polling...")
bot.polling(none_stop=True)
