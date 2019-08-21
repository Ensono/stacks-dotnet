#!/bin/bash
me=`basename "$0"`

name=${name:-}
#school=${school:-is out}

while [ $# -gt 0 ]; do

   if [[ $1 == *"--"* ]]; then
        param="${1/--/}"
        declare $param="$2"
        #echo "$1 = $2" # Optional to see the parameter:value result
   fi

  shift
done

if [[ -z $name ]]; then
	echo -e "\e[1m\e[31mA name must be provided! \e[39m\e[0m"
	echo "Usage: "
	echo " $me --name COMPANY.PROJECT"
	exit 1;
else
	echo "Renaming project from 'xxAMIDOxx.xxSTACKSxx' to '$name'"
fi;


#replace folder names
for directory in `find ./src -iname "*xxAMIDOxx*" -type d ` ; do

	NEWFILE=$(echo "$directory" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$name/g" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$name/g")
	mv -f $directory $NEWFILE

done

#replace file names
for file in ` find ./src -iname "*xxAMIDOxx*" -type f ` ; do
	
    NEWFILE=$(echo "$file" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$name/g")
    mv -f $file $NEWFILE

done

#replace file contents (Namespaces, References, Usings)
find ./src -type f -exec sed -i "s/xxAMIDOxx.xxSTACKSxx/$name/g" {} + 

echo "Renaming complete"