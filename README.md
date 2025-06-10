Assume file has UTF8 encoding and CRLF new lines

Choose a streaming option as this allows to stream massive files 

Wanted to apply some logic to streaming buffer. For example. Remove trailing spaces after a word that is removed.

From output it is clear that more of these rules are probably needed. For example "Oh dear! Oh dear! Oh dear!" => "!!!" Howerver you would need to understand the sentence. You may not want to remove a full stop. But you may want to remove exclamation mark. Sometimes people use stuff like this?!?!. This would be ok??.

All this could be coded. You could also maybe want to remove carriage returns, if for example, an entire line had been removed. However this would require loading more info in the buffer. And if the file had no carriage returns, you would have memory issues.

Hopefully you get the idea