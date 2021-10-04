# Spreadsheet Application

[![.NETBuild](https://github.com/BenjaminMichaelis/SpreadsheetApplication/actions/workflows/dotnetbuild.yml/badge.svg)](https://github.com/BenjaminMichaelis/SpreadsheetApplication/actions/workflows/dotnetbuild.yml)

Custom C# Spreadsheet application using WinForms to mock some of the functionality of an excel-like application.

## Features
* Circular reference detection
* Can reference cells in equations
* Cells automatically update when referenced cells are changed
* Undo & Redo
* Additional operators may be added quickly and easily.
* Standalone Logic engine (seperate from UI)
* Variables may be defined and used in equations
* Expressions are compiled and only rebuilt when change in expression is detected
* Expressions support parenthetical priority
