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

# Underscores/numbers in test method names
Numbers or underscores in the test names only there to follow the MethodName_StateUnderTest_ExpectedBehavior naming convention, for example shown here 
https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html

That is my general reasoning for having numbers in my naming conventions for tests, and it quickly allows me to view from even just the description in the test explorer, what the method is that is being tested, what I am passing in or what state in which im testing it at, and what result I expect to get from the tests, so I can quickly identify the scenarios in which tests are failing and what cases are not being covered.