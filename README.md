# Subway map render

A render for creating Minecraft subway map.

## Work in progress

Features

- [ ] Support attach line (Add a property in DataStruct.LineItem to indicate which line is current line's parent).
- [ ] Support multi-language of render result(the website).
- [ ] Support render config(to indicate what should be drawed on map. For example, Name, ID, Subtitle or Icons...).
- [ ] Support Icon feature(Use Icon to describe what is contained by current station. For example, Food, Spawn point or Airplane...).
- [ ] Support drawing curve.
- [ ] Support display position(Real-world position is ugly for display, maybe...).
- [ ] Support draw Ocean and Island(Add some property and class in DataStruct.SubwayMap).
- [ ] Support small scale (Press shift and mouse wheel)

Bugs

- [ ] Svg scale bug.
- [ ] Line overlay bug(The rail which have high Y value should overlay the rail which have low Y value).

Optmize

- [ ] Optimize the code of Command.cs(Its code just like sh*t).

