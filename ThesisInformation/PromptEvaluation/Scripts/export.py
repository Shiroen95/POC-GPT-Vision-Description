import os
import json
from datetime import datetime


directory = ''
date_format = '%Y-%m-%dT%H-%M-%S.%f'
date_from= datetime(2024,7,9,00,00,0)
date_to=datetime(2024,7,11,00,00,0)

def canConvert(text:str) -> bool: 
    try:
        datetime.strptime(text, date_format)
        return True
    except ValueError:
        return False

def getDirectoryList(baseDirectory:str) -> list[str]:
    allDirs = filter(canConvert,os.listdir(baseDirectory))
    filteredList = filter(
        lambda x : date_from <= datetime.strptime(x, date_format)  <= date_to 
        ,allDirs)
    return map(lambda x : baseDirectory + x, filteredList)

dirs = getDirectoryList(directory)

file = open(f"./exported_responses_{datetime.now().strftime(date_format[:-3])}.txt",'a+')
i = 1
maxTokens = 0
promptTokens = 0
completeTokens = 0
for dir in dirs:
    with open(dir+"/response.json") as response:
        jsonDic = json.load(response)
        content = jsonDic['choices'][0]['message']['content']
        maxTokens += int(jsonDic['usage']['total_tokens'])
        promptTokens += int(jsonDic['usage']['prompt_tokens'])
        completeTokens += int(jsonDic['usage']['completion_tokens'])
        file.write("Number " + str(i) +" "+dir+"\n")
        file.write(content)
        file.write("\n----------------------------------------\n")
        i +=1

file.close()
print(maxTokens)
print(promptTokens)
print(completeTokens)
