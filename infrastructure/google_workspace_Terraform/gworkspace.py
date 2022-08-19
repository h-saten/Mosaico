import os

from numpy import single
f = open("users.csv","r",encoding="utf-8")

headline = "index,first_name,last_name,group"
users = []
groups = {}
index=1

for line in f:
    users.append(line[:-1])
users.pop(0)

for user_line in users:
    user_line = user_line.split(',')
    user_line.pop(0)
    user = f"{user_line[0]} {user_line[1]}"
    groups[user] = user_line[-1].split(';')

if os.path.exists("./groups.csv"):
  os.remove("./groups.csv")
else:
  print("The file does not exist")

f = open("groups.csv","a+",encoding="utf-8")
f.write(f"{headline}\n")
for user,group in groups.items():
    user = user.split(" ")
    for single_group in group:
      f.write(f"{index},{user[0]},{user[1]},{single_group}\n")
      index += 1 