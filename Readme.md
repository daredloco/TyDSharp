# TyDSharp

A simple C# implementation of TyD - Tynan's Tiny Data Language.

[Read the TyD spec here.](https://github.com/tyd-lang/TyD)

## Version

TyDSharp is currently in version 0.1.0, supporting TyD version 0.1.0.

TyDSharp version numbers don't necessarily match TyD version numbers.

## Contributions

All contributions including documentation, pull requests, and bug reports are welcome!

## This version

Downgraded to C# 4 for Unity 5 compatibility.

Adds custom attributes with string values, so you can write:
	Thing *someVar "This is a custom attribute" *otherVar
	{
	Values		[ 0; 1; 2 ]
	}


Adds helper functions for easy node enumeration, so you can write:
	node.GetChild<TydCollection>("Values").GetChildValues<int>() //Get all children in the Values record converted to integers