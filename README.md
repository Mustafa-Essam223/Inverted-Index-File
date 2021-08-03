# Inverted-Index-File

Research code on Inverted Index/File concept using Files structures and C#

The program allows the user to add new records or view current ones.

Files stores a mapping from record to its location in a file (Data File)

Primary key file stores the id's sorted ascending by default mapped to its reference

Secondary key file contains a column index (City) mapped to number of occurences of this city

Inverted Index file contains three columns (ref-ID-next ref) which are the index of the record, ID of the record , and a ref to the next record with tha same city and terminated by -1
