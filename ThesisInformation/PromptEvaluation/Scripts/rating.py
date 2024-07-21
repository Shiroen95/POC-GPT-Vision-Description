import os
import numpy


directory = ""
filename = ""

file = open(f"./{filename[:-3]}_ratings.txt",'a+')

with open(directory+filename) as export:
    lines = export.readlines()
    index = 1
    Extras = {}
    sumRating = numpy.array([0.0,0.0,0.0,0.0,0.0,0.0,0.0])
    for line in lines:
        if(line.find("Rating:") >= 0):
            line = line.strip()
            end = line.find("Extras:")       
            strRating = line[7:]  
            newExtras = ["No Extras"]
            if(end>0):
                newExtras = line[end+len("Extras:"):].replace(".","").split(";")
                strRating = line[7:end]              
            rating = [float(x) for x in strRating.replace(",",".").split(";")]

            for extra in newExtras:
                extra = extra.lower().strip()
                if(not extra):
                    continue 
                if(extra not in Extras.keys()):
                    Extras[extra] = 0
                Extras[extra] += 1
            sumRating += numpy.array(rating)
            print(index)
            index +=1
sortedExtras = sorted(Extras.items(),key=lambda item:item[1])
sortedExtras.reverse()
sumRating = sumRating/10
print(sumRating)
print(len(sortedExtras))
sumExtras = 0
for item in dict(sortedExtras).items():
    print(item)
    sumExtras += int(item[1])
print(sumExtras)