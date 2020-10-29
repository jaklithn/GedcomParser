# Gedcom Parser
This is a utility to parse GEDCOM 5.5.5 files. It is intended to be very simple to use and still true to the format.

## GEDCOM
GEDCOM is a file format to hold Genealogy data. It was first created 1984 and is widely adopted by genealogists worldwide. It is a flat file format that looks simple but is surprisingly complex. The structure is easy to read but the complexity is caused by all tags that can be combined in many ways and tries to cover all possible genealogy needs. Typically different users and different software just uses a selected subset of the available variations.
There have been several attempts to redesign the file format to cover new needs like flexible event handling and varied family forms. But this format is widely adopted and so far has continued to be the de facto standard.
The formal specification can be found here: [https://www.gedcom.org/](https://www.gedcom.org/)
General background can be found here: [https://en.wikipedia.org/wiki/GEDCOM](https://en.wikipedia.org/wiki/GEDCOM)

## Comparison
I know there are already several projects with the same intention :)
But for C# there are just a couple and they all have approaches that don't fit my need. Either they make a detour by building complex XML structures or they create a complete relational data structure. Some are too limited and just include the very basic properties.

## Purpose
The intention of this project is just to parse the GEDCOM file content into simple POCO objects that reflect the data in a reliable way. Then it is up to the client to process the data in whatever way is appropriate. My current usecase is to load the data into graph databases for further handling. In this project I therefore avoid creating additional intelligent tree structures. Even if they are interesting for some, they are not needed for me as the graph database will take care of that in a much better way! If tree structure is still preferred it is very easy to extend the current data model.

## Disclaimer
Most GEDCOM software only uses a limited amount of the tags. It would take a lot of time to cover all tags in their variations and it would be of little use. The approach taken here is to cover all normally used tags. Some tags are deliberately skipped as they are irrelevant for my current need. They will instead be returned as warnings. Unusual tags that are not handled will be returned as errors. This approach will make it easy to gradually extend the logic and handle missing tags when they are actually requested. After parsing a file it is a good practice to review the Error and Warning collections to ensure important tags are not missing.

## Internal Design
The GEDCOM file holds simple lines. They basically all have the same structure:

    Level [Id] Type [Data] [Reference]

The parser will first read all lines into a collection of GedcomLines.
By using the Level property they are then structured into GedcomChunks. Each GedcomChunk represents a GedcomLine and a subcollection of GedcomChunks.
The data is then parsed by processing all top chunks and their internal children. All data is interpreted according to its Type.
The main chunks represent either individuals or families. All other chunks are sub structures or reference information.
Person maps directly to a regular person with all needed attributes.
Family is a hub to describe the internal relations of a nuclear family.
The parser produces two resulting collections named Persons and Relations. They can then be used for further high level processing. When targeting graph databases they are ideal as they directly map to nodes and relations without much additional effort.
