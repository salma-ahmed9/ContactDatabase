CREATE MIGRATION m1uwmt2gatie7rlzk4fzig4uy5kjvqiteld5y4qjy24wv5a7xdetcq
    ONTO m1y5dk2w7xvyw5pnrqxztgtvbwo6gioeuvo5vbx6epzph4ecnrmuxq
{
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY password: std::str {
          SET REQUIRED USING ('Untitled');
      };
      CREATE REQUIRED PROPERTY role: std::str {
          SET REQUIRED USING ('Untitled');
      };
      CREATE REQUIRED PROPERTY username: std::str {
          SET REQUIRED USING ('Untitled');
      };
  };
};
