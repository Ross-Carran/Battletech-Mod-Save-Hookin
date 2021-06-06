# Battletech-Mod-Save-Hookin
To add save functionality for mods in Battletech using sql-light

Initial work will be to get sql to hook into Battletech with Harmoney.

After that, making it so that it can create saves in sync with the normal Battletech saves.
In, this case creating a new database and/or adding/removing data from it.

Then to find an ideal way to save/load actual game data to it.

Why make this ?

To keep the base Battletech and Mod data seperate from eachother.
The save system that was created for battletech was not designed to recognise new classes or fields.
Battletech uses a template system so that it knows what to serialize and the way to do so.
Anything the battletech serializer does not recognise defaults to unitys base serializer, which gets messy very quickly.


This maybe outside of my ability to do, my understanding of how reflection works is limited.
