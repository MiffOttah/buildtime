This is something I needed internally, so I created it.

When ran, it generates a C# file containing a static class with a `BuildTime`
property, a `DateTimeOffset` that returns a static value based on when the
tool was run.

Usage is:

    dotnet BuildTime.dll [-n NAMESPACE] [-c CLASS] [-p] [OUTFILE]

Passing `-n` lets you set the namespace and `-c` lets you set the class name.
By default, there is no namespace and the class is named `_BuildTime`.

Passing `-p` will make the generated class public instead of internal.

If the outfile isn't provided, it creates it in the current directory called
`$CLASS.cs`, where `$CLASS` is the name of the generated class.

This probably doesn't work that well, it wasn't heavally tested. It's just
something I figured I'd share. :)

