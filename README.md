IRANSYSTEM Convertor
================================

**Purpose**

This class converts a string containing IRANSYSTEM characters into a unicode string. Note that this just converts alphanumeric characters and ignores the rest.

**Usage**

<pre>
using IranSystemConvertor;
.
.
.

string iranSystemStr = "an IRANSYSTEM string"

string unicodeStr = ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, iranSystemStr); 

</pre>


**Dependencies**

No dependency
