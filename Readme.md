# TyDSharp

This version of TyDSharp builds on the version for Software INC and is used inside the [DataSINC](https://github.com/daredloco/DataSINC) Editor for the game.

[Get the official TyDSharp here.](https://github.com/tyd-lang/TyD)

[Read the TyD spec here.](https://github.com/tyd-lang/TyDSharp)

## Changes

* Minor changes within the TydConverter to handle Crashes from the Editor if Variables have multiple types like SoftwareMarkets. If the variable is an array, but it is a TydString inside the file it will create a new TydList with a single item.
* Added ignorenullvalues as parameter to the TydConverter.Serialize function to ignore fields with null as value.
* Added TydAttributes.TydName which is a custom Attribute to give fields other names in the file if using the TydConverter for it.
* Added TydAttributes.TydIgnore which is a custom Attribute to ignore certain fields if using the TydConverter as the NonSerialized attribute wouldn't work very well with WPF.
