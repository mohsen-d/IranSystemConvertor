#IRANSYSTEM Convertor#
================================

**Purpose**

this class converts a string containing IRANSYSTEM characters into unicode string . note that this just converts alphanumeric characters and ignore others .

**Usage**

<pre>
using IranSystemConvertor;
.
.
.

string iranSystemStr = // an IRANSYSTEM string

string unicodeStr = ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, iranSystemStr); 

</pre>


**Dependencies**

no dependency
