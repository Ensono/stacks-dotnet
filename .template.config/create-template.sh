# Prepare
cp stacks-app-template.proj ../stacks-app-template.proj
cd ..
cp .gitignore _gitignore

# Pack
dotnet pack stacks-app-template.proj
cp ./bin/**/*.nupkg ./

# Cleanup
rm _gitignore
rm stacks-app-template.proj
rm -drf bin/
rm -drf obj/