# Hitmul
[![NuGet package](http://img.shields.io/nuget/v/HitmulCore?style=flat&logo=nuget)](https://www.nuget.org/packages/HitmulCore/ "View this project on NuGet")
[![Build Status](https://travis-ci.com/Shamus03/Hitmul.svg?branch=master)](https://travis-ci.com/Shamus03/Hitmul)

A basic formatter that uses FormattableString to automagically escape HTML.

# Use

Use the `Hitmul` class to dynamically build some HTML with interpolated arguments without fear of HTML injection.

Call `ToHtml` on a `Hitmul` to create a string with properly escaped arguments.

```c#
var name = "Shamus";
var html = new Hitmul($"<h1>Hello</h1>")
    .Append($"<p>{name}</p>")
    .ToHtml();
// html = "<h1>Hello</h1>\n<p>Shamus</p>"
```

The `Hitmul` constructor only accepts a `FormattableString` as an argument to prevent accidental HTML injection.  To pass in a raw string, you must explicitly use one of the functions with `Raw` in its name.

```c#
var name = "Shamus";
var fmt = $"<p>{name}</p>";

// Will not compile!  `fmt` is a `string`, not `FormattableString`!
var html = new Hitmul(fmt);

// This will compile, but will be vulnerable to attacks. Use `Raw` sparingly!
var html = Hitmul.Raw(fmt);
```
