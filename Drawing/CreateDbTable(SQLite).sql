CREATE TABLE [Diagram] (
  [Id] TEXT NOT NULL
, [Name] TEXT NOT NULL unique
, [JsonValue] BLOB NOT NULL
, CONSTRAINT [PK_Diagram] PRIMARY KEY ([Id])
);