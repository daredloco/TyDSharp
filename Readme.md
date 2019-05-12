# TyDSharp

This version of TyDSharp builds on the initial version, specifically to support the development of Software Inc.

[Get the official TyDSharp here.](https://github.com/tyd-lang/TyD)

[Read the TyD spec here.](https://github.com/tyd-lang/TyDSharp)

## Changes

Downgrade to C# 4 for Unity 5 compatibility.

Adds custom attributes with string values, so you can write: (This is not in accordance with the official specification!)

    Thing *someVar "This is a custom attribute" *otherVar
    {
    Values    [ 0; 1; 2 ]
    }

Adds helper functions for easy node enumeration, so you can write:

    node.GetChild<TydCollection>("Values").GetChildValues<int>() //Get all children in the Values record converted to integers
	
Adds helper functions for easy TyD construction, so you can write:

    var table = rootNode.AddChild(new TydTable("Thing"));
	table.AddChild(new TydList("Values")).AddChildren(new TydString(null, "0"), new TydString(null, "1"), new TydString(null, "2"));

## Contributions

All contributions including documentation, pull requests, and bug reports are welcome!

