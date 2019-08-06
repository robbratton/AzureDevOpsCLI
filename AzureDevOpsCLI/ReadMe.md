# VSTS Tool

Provides command-line access to Azure DevOps API items.

## Usage

`VSTSTool command itemType [arguments...]`

## Commands

### Copy

Copy an item from with oldName and name the copy as newName. (Clone)

`copy itemType oldName newName`

### Delete

Delete an item.

`delete itemType name`

### Dump

Dump the JSON definition for an item.

`dump itemType name`

### List

List items optionally matching a filter.

`list itemType [RegularExpressionFilter]`

### Rename

Rename an item from oldName to newName.

`rename itemType oldName newName`

### Possible Future Commands

- *To be determined*

## Item Types

- BuildDefinition
- TaskGroup

### Possible Future Item Types

- *VariableGroup*

 