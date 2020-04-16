# Prepare
cp stacks-app-template.proj ../stacks-app-template.proj
cd ..
mv .gitignore _gitignore
mv .gitattributes _gitattributes

# Pack
dotnet pack stacks-app-template.proj
cp ./bin/**/*.nupkg ./

##########
# Revert #
mv _gitignore .gitignore
mv _gitattributes .gitattributes

# Cleanup
rm stacks-app-template.proj
rm -drf bin/
rm -drf obj/