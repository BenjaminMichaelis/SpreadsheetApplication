# CptS321-HWs

Benjamin Michaelis

ID# 11620581

# SA1000: Keywords should be spaced correctly 
Doesn't follow msdn documentation examples (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator)

# SA1309: Field Names Must Not Begin With Underscore
Doesn't follow documentation.
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields
https://github.com/dotnet/runtime/blob/master/docs/coding-guidelines/coding-style.md

# SA1201: A field should not follow a constructor
Field should be by its wrapping property:
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields

# SA1111: Closing parenthesis should be on line of last parameter  
# SA1009: Closing parenthesis should not be preceded by a space
Doesn't allow for a parenthesis to be on a new line which allows a line to be 
commented out without breaking a statement. 
ex: 
                    result = result.Where(item =>
                            item.Name.Contains(word)
                            || item.Id.Contains(word)
                            || item.Description.Contains(word)
                            );
If it was on the preceding line with item.Description then if I wanted to comment out that line to debug it would break.
