import os
import smtplib,ssl
from email.message import EmailMessage
##Import rodzaju maila
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText

emails = open("users.csv","r")
emails_list=[]
for line in emails:
    line = line.split(",")
    emails_list.append(line[4])
emails_list.pop(0)

##Autoryzacja
# you need credentials_smtp.txt file which contains: first line - smtp domain name X.X.smtp; second line - password to that smtp server
credentials_smtp = open("./credentials_smtp.txt","r")
credentials_list = []
for line in credentials_smtp:
    credentials_list.append(line.strip("\n"))

uname = credentials_list[0]
passwd = credentials_list[1]
 
output_passwords = []
f = open("passwords.txt","r", encoding="utf-16")
for line in f:

    if len(line) == 18:
        line = line[3:-3]
        output_passwords.append(line)

email_password = {}
for index,password in enumerate(output_passwords):

    email_password[emails_list[index]] = password

blacklist_emails = open("users_blacklist.txt","r")
blacklisted = []
for line in blacklist_emails:

    blacklisted.append(line.strip("\n"))

for email in blacklisted:

    if email in emails_list:
        email_password.pop(email)

def email_text(password,recipent):

    message = MIMEMultipart("alternative")
    message['Subject'] = 'Twoje Hasło dostępu dla Google Workspace'
    message['From'] = "noreply@mosaico.ai"
    message['To'] = f"{recipent}"

    text = f"""\n
    Witaj,

    Twoje jednorazowe hasło do usługi Google Workspace to: {password}.
    Po zalogowaniu proszę postępować zgodnie z krokami, oraz wprowadzić nowe hasło.

    Możesz się zalogować pod adresem: https://workspace.google.com/dashboard
    ---
    Wiadomość została wysłana automatycznie, prosimy na nią nie odpowiadać.
    """
    part1 = MIMEText(text, "plain")
    message.attach(part1)
    return message.as_string()

# Turn these into plain/html MIMEText objects

context = ssl.create_default_context()
with smtplib.SMTP('smtp.emaillabs.net.pl', 587) as server:

    server.ehlo()  # Can be omitted
    server.starttls(context=context)
    server.ehlo()  # Can be omitted
    server.login(uname, passwd)

    for index,email in enumerate(email_password):

        server.sendmail("noreply@mosaico.ai", email, email_text(email_password[email],email))

        with open("users_blacklist.txt","a+") as f:
            f.write(f"{email}\n")