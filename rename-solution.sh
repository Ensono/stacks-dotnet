if [ $# -eq 0 ]; then echo "please provide a name in the form 'PROJECT.COMPANY' to replace 'xxAMIDOxx.xxSTACKSxx'"; exit 1; fi;

#replace folder names
for directory in `find ./src -iname "*xxAMIDOxx*" -type d ` ; do

	NEWFILE=$(echo "$directory" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$1/g" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$1/g")
	mv -f $directory $NEWFILE

done

#replace file names
for file in ` find ./src -iname "*xxAMIDOxx*" -type f ` ; do
	
    NEWFILE=$(echo "$file" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$1/g")
    mv -f $file $NEWFILE

done

#replace solution name
#for file in ` find ./src -iname "*xxAMIDOxx*sln" -type f ` ; do
#	
#    NEWFILE=$(echo "$file" | sed -re "s/xxAMIDOxx.xxSTACKSxx/$1/g")
#    mv -f $file $NEWFILE
#
#done

#replace file contents (Namespaces, References, Usings)
find ./src -type f -exec sed -i "s/xxAMIDOxx.xxSTACKSxx/$1/g" {} + 